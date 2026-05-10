# KYC Level 1 + Level 2 “Single Flow” Submission (2 Calls)

## Goal
Users want to complete **KYC Level 1 (ID)** and **KYC Level 2 (Selfie)** in **one user flow** (one “Submit” action).

**Constraint**: Current backend `UploadKYC` endpoint only accepts **one** KYC item per request (one `KYCID` + one `FileTypeID` + one `FilePath`).

**Decision**: Implement “single flow” by performing **two `UploadKYC` calls** from the client/middleware layer, and treating it as a **bundle** (success/failure per part).

## Backend/API Reality (why it must be 2 calls)
`POST v1/KYC/UploadKYC?option=1` accepts a single `[FromForm] UploadKYCDetails` with one `IFormFile FilePath`.

The service behavior is keyed by `(KYCID, FileTypeID)`:
- **Level 1 ID upload**: `KYCID = 1`, `FileTypeID = 0`
- **Level 2 Selfie upload**: `KYCID = 2`, `FileTypeID = 1`

So “Level 1 + Level 2 in one submission” cannot be done as **one HTTP request** without adding a new backend endpoint. We will keep current backend and do **two HTTP requests**.

---

## What to tell the middleware developer (IAM_Library)

### 1) Add a bundle helper in `IAM_Library` (or `WalletAccountDetailLoader`)
Today, the library already supports a single upload via:
- `WalletAccountsApiClient.POSTUploadKYC(KYCUploadModel postData)`
- Loader wrapper: `WalletAccountDetailLoader.POSTSendKYCForVerification(..., KYCUploadModel postData)` (single item)

**Needed**: A new method that uploads both items and returns a combined result.

Suggested signature (example):
- `Task<ApiResponseModel<KYCBundleUploadResponse>> POSTUploadKYCBundle(WalletAuthResponseData creds, HttpClient http, KYCUploadModel level1, KYCUploadModel level2)`

Where `KYCBundleUploadResponse` includes:
- `Level1`: `ApiResponseModel<KYCUploadResponse>`
- `Level2`: `ApiResponseModel<KYCUploadResponse>`
- `IsSuccess`: true only if both succeeded
- `Message`: user-friendly summary (“ID uploaded, selfie failed: <reason>”)

### 2) Enforce correct mapping and validation in one place
Hard rules:
- Level 1 request must be forced to `KYCID=1`, `FileTypeID=0`
- Level 2 request must be forced to `KYCID=2`, `FileTypeID=1`
- Require `AccountID`, `IDCode`, `CardNumber`, `CardFname`, `CardSname` on both
- Each request has its own `FilePath` (base64 or physical path as already supported by `POSTUploadKYC`)

### 3) Execution strategy: prefer sequential
Recommend **sequential** for stability and clearer UX:
1. Upload Level 1 (ID)
2. If Level 1 succeeds, upload Level 2 (selfie)

Reason: reduces partial submissions and makes error handling clearer (“please retry selfie”).

If product insists on speed, parallel via `Task.WhenAll(...)` is possible, but then you must handle partial success cleanly.

### 4) Make retries safe (idempotency-lite)
Current server logic can return “already uploaded…” (duplicate check). That’s good, but not perfect idempotency.

Ask: if possible, add (later) a `ClientRequestId` field to the API so repeated requests are recognized.  
For now, the bundle helper should:
- Treat “already uploaded” style messages as **non-fatal** (so user can proceed)
- Return per-part status so UI can retry only failed part

### 5) Deliverable from middleware team
- New bundle method as above
- Unit-test/basic test harness calling bundle upload
- Clear return contract that frontend can display per-part results

---

## What to tell the frontend developer (Blazor `.razor` / `.razor.cs`)

### 1) Frontend must collect and retain BOTH files before “Submit”
Right now `KycReview.razor` constructs a **single** `KYCUploadModel` using:
- `KYCID = nextKYCID` based on `staticKYCChoices.currentActiveLevel`
- `FileTypeID = staticKYCChoices.chosenFileType`
- `FilePath = staticKYCChoices.filePath`

That design only uploads **one** item per submission.

**Needed**: Store two file payloads in state, e.g.:
- `staticKYCChoices.filePathLevel1Id` (base64 or path)
- `staticKYCChoices.filePathLevel2Selfie` (base64 or path)

And store a “bundle ready” flag:
- `staticKYCChoices.hasLevel1Id`
- `staticKYCChoices.hasLevel2Selfie`

### 2) On “Submit for Review” call 2 uploads (recommended sequential)
In `KycReview.razor` `UploadKYC()`:
- Build **two** `KYCUploadModel`s with shared identity fields but different `(KYCID, FileTypeID, FilePath)`

**Level 1 model**
- `KYCID = 1`
- `FileTypeID = 0`
- `FilePath = staticKYCChoices.filePathLevel1Id`

**Level 2 model**
- `KYCID = 2`
- `FileTypeID = 1`
- `FilePath = staticKYCChoices.filePathLevel2Selfie`

Call sequence:
1. `resp1 = await appService.UploadKYC(..., level1Model)`
2. If success (and `resp1.Data.statusCode != 0` per current logic), then:
3. `resp2 = await appService.UploadKYC(..., level2Model)`

UI rules:
- Show progress: “Uploading ID…”, then “Uploading selfie…”
- If ID succeeds but selfie fails: show message “ID uploaded; selfie failed—please retry selfie”
- Provide “Retry Selfie Upload” action (no need to redo ID)

### 3) Don’t depend on `currentActiveLevel` to decide what to upload
For bundle submission, `currentActiveLevel` is only for **eligibility** (e.g., don’t show flow if already Level 2 max).
It should NOT drive which file is uploaded in the final step.

### 4) Acceptance criteria for the frontend
- User completes one flow and taps Submit once
- App sends up to 2 uploads
- App can recover from partial failures without losing the already uploaded file
- After both succeed, redirect to `/kycsubmit` success page

---

## Optional improvement (later): true single backend request
If you ever want a real “single request submission”:
- Add a new backend endpoint that accepts **two file parts** (ID + selfie) in one multipart request
- Return a bundle response with per-part statuses

Not required for this release since we’re proceeding with 2 calls.

