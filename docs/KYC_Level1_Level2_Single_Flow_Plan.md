# Plan: KYC Level 1 + Level 2 in One User Flow (Review Before Implementation)

**Status:** Planning only — no IAM_Library code changes were made for this document.  
**Related spec:** `IAM-Utilities/KYC_Level1_Level2_Submission.md`  
**Relevant API:** `IAM-Utilities/API/Controllers/KYCController/KYCController.cs` — `POST v1/KYC/UploadKYC`  
**Relevant DTO:** `IAM-Utilities/DTO/UtilitiesMdl/IDC/IDentificationCard.cs` — `UploadKYCDetails` (single `IFormFile FilePath`)

---

## 1. What the backend allows today

- `UploadKYC(int option, [FromForm] UploadKYCDetails kyc)` accepts **one** multipart upload per HTTP request: one `FilePath` file plus the scalar form fields (`KYCID`, `FileTypeID`, `AccountID`, `IDCode`, `CardNumber`, names, dates, etc.).
- There is **no** combined “upload ID + selfie in one request” endpoint unless you add a new API later.
- **Therefore:** “One Submit button / one flow” for the user is implemented as **two HTTP calls** (Level 1, then Level 2), orchestrated by the client.

**Mapping (from internal docs / service behavior):**

| Part        | `KYCID` | `FileTypeID` | Typical capture UI      |
|------------|---------|--------------|-------------------------|
| Level 1 ID | `1`     | `0`          | ID document (e.g. `KycCamId`) |
| Level 2 selfie | `2` | `1`          | Selfie (e.g. `KycCamSelfie`)   |

**Shared fields** on both requests (must match): `AccountID`, `IDCode`, `CardNumber`, `CardFname`, `CardMname`, `CardSname`, plus sensible `SvrDate` / `UpdateDate`, `Status`, `VerifiedBy` as today.

---

## 2. What IAM_Library already provides (no change required for a minimal rollout)

| Layer | Responsibility |
|--------|----------------|
| `WalletAccountsApiClient.POSTUploadKYC(KYCUploadModel postData)` | Builds multipart to KYC base URL + `UploadKYC` endpoint; supports **file path** or **base64** in `FilePath`; uses **ApiKey** header on the KYC client (not the same as wallet Bearer on other calls). |
| `WalletAccountDetailLoader.POSTSendKYCForVerification(...)` | Validates credentials / `HttpClient` / non-empty `FilePath`; constructs `WalletAccountsApiClient`; returns `ApiResponseModel<KYCUploadResponse>`. |

The app already calls this through **`IAM_AppServices.UploadKYC`** → **`POSTSendKYCForVerification`** (see `IAM-Wallet` `KycReview.razor`).

**Action item for implementation:** Confirm the resolved KYC URL includes the **`option` query parameter** expected by `KYCController.UploadKYC(int option, ...)` (the markdown example uses `?option=1`). If the encrypted endpoint string does not include it, the bridge or app must append it consistently.

---

## 3. Frontend-only vs IAM_Library “bundle” — recommendation

### Preferred for this release: **app / Blazor (and optionally thin `IAM_AppServices`) only**

**Reasons:**

1. The library already exposes everything needed: call `POSTSendKYCForVerification` (or `UploadKYC` on the app service) **twice** with two different `KYCUploadModel` instances.
2. Orchestration (order, progress text, partial-failure UX) is **UI and app-layer** concern; duplicating it inside IAM_Library does not remove the need for the UI to show “ID OK, selfie failed”.
3. Fewer moving parts: one repo touch (wallet app) vs library version bump + app update.

### When adding a **library bundle** would make sense (optional later)

Add something like `POSTUploadKYCBundle` / `WalletAccountDetailLoader` wrapper only if:

- Multiple apps (not only Blazor) must share **identical** sequencing, validation, and “treat duplicate as success” rules; or
- You want **one** call from the app into the library and a **single** structured result type (e.g. `KYCBundleUploadResponse` with `Level1` / `Level2` / `IsSuccess` / summary `Message`).

This matches the *optional* deliverable described in `KYC_Level1_Level2_Submission.md` for the middleware team; it is **not** required if only the MAUI Blazor app needs the feature.

---

## 4. Execution strategy (when you implement)

**Recommended:** **Sequential** uploads (Level 1 first, then Level 2 if Level 1 succeeded).

- Clearer errors (“Selfie failed—retry selfie” without re-uploading ID).
- Aligns with `KYC_Level1_Level2_Submission.md`.

**Optional:** `Task.WhenAll` for speed — only if product accepts more complex partial-success handling.

**Retries / “already uploaded”:** If the API returns a non-fatal “already uploaded” style message, the bundle/orchestrator should treat it as **success for that part** so the user can continue (per the markdown).

---

## 5. What to change in the Blazor app (`IAM-Wallet/Components/Pages/KYC`)

**Problem today (high level):** `KycReview.razor` builds **one** `KYCUploadModel` from `staticKYCChoices.filePath` and derives `KYCID` from `DetermineNextKYCID(currentActiveLevel)`. That fits “upload the **next** single level” but **not** “collect both ID and selfie, then submit once” when both are required in one flow.

### 5.1 State: hold two payloads

In `Models/User/UserBasicDetails.cs`, extend **`staticKYCChoices`** (or replace with a small scoped state service if you prefer testability):

- `filePathLevel1Id` (or equivalent): base64 or path for **Level 1** (`KYCID=1`, `FileTypeID=0`).
- `filePathLevel2Selfie`: base64 or path for **Level 2** (`KYCID=2`, `FileTypeID=1`).
- Flags such as `hasLevel1Id`, `hasLevel2Selfie` (or infer from non-empty paths).

Keep existing `filePath` only if you still need backward compatibility during migration; otherwise migrate all readers to the two explicit fields.

### 5.2 Capture pages — write to the correct slot

| Page | Change |
|------|--------|
| `KycCamId.razor` (and any ID-only path) | After capture, set **`filePathLevel1Id`** (and clear or leave selfie unchanged). |
| `KycCamSelfie.razor` | After capture, set **`filePathLevel2Selfie`**. |
| `KycId.razor.cs` / navigation | Ensure users who must complete **both** in one flow visit both capture steps **before** `/kycreview`, or block review until both flags are set. |

Do **not** rely on a single `filePath` for both, or the second capture will overwrite the first.

### 5.3 `KycReview.razor` — orchestrate two uploads on one button

On **Submit for Review**:

1. Validate auth (`accountKey`, `signature`) as today.
2. Decide **which uploads are required** from `staticKYCChoices.currentActiveLevel`:
   - **Level 0:** require both Level 1 and Level 2 files → two models, two calls.
   - **Level 1:** only Level 2 (selfie) → one call with `KYCID=2`, `FileTypeID=1`.
   - **Level 2:** no upload (or show “already verified”).
3. Build **Level 1 model** (when needed): fixed `KYCID=1`, `FileTypeID=0`, `FilePath=filePathLevel1Id`, same identity fields as today from `staticKYCChoices`.
4. `await appService.UploadKYC(..., level1Model)`; check `response.Data?.statusCode != 0` (same semantics as current page).
5. If step 4 OK, build **Level 2 model** with `KYCID=2`, `FileTypeID=1`, `FilePath=filePathLevel2Selfie`; call `UploadKYC` again.
6. UI: show progress strings (“Uploading ID…”, “Uploading selfie…”), spinner, disable button while either call runs.
7. **Partial failure:** if Level 1 OK and Level 2 fails, show a message that allows **retry selfie only** without clearing Level 1.
8. **Full success:** navigate to `/kycsubmit` as today (possibly after refreshing KYC level from `LoadMainAccountKYCLevel` / home logic).

### 5.4 Do not use `DetermineNextKYCID` alone for the dual upload

For a true L1+L2 bundle from **level 0**, the submit step should **force** the two `(KYCID, FileTypeID)` pairs above, not a single “next” level derived from `currentActiveLevel + 1`. Keep `currentActiveLevel` for **eligibility** (whether to show the flow), not as the sole driver of which file is sent in the final dual submission.

### 5.5 Other KYC pages

- **`KycInfo.razor` / `KycUserInfo.razor`:** Ensure navigation into the capture chain matches the product rule (both captures required before review when starting from level 0).
- **`KycSubmit.razor`:** No API change; success page after both parts succeed.
- **Video paths (`KycCamVid.razor`, `KycVid.razor`):** If Level 2 can be video (`FileTypeID=2`), align with `POSTUploadKYC` content-type branches and API expectations; the “L1+L2 single flow” doc focuses on ID + selfie images—confirm with backend if video is in scope.

---

## 6. Optional IAM_Library follow-up (after review)

Only if multiple consumers need the same orchestration:

1. Add types, e.g. `KYCBundleUploadResult` with nested `ApiResponseModel<KYCUploadResponse>` per level + aggregate `IsSuccess` + `Message`.
2. Add `WalletAccountDetailLoader.POSTSendKYCBundleAsync(..., KYCUploadModel level1, KYCUploadModel level2)` that:
   - Forces `KYCID`/`FileTypeID` for L1 and L2;
   - Validates shared identity fields once;
   - Runs sequential `POSTUploadKYC` calls;
   - Maps “already uploaded” responses to non-fatal success if agreed with API team.
3. Unit test or integration test harness against staging KYC API.

---

## 7. Checklist before enacting

- [ ] Confirm `UploadKYC` URL includes correct **`option`** query parameter.
- [ ] Confirm duplicate / idempotent responses from `KYCServices.UploadKYC` for retries.
- [ ] Product decision: sequential vs parallel second upload.
- [ ] Blazor: two storage fields + `KycReview` dual call path + partial-failure UX.
- [ ] Decide whether library bundle is in scope for v1 or deferred.

---

## 8. File reference index

| Area | Path |
|------|------|
| API | `IAM-Utilities/API/Controllers/KYCController/KYCController.cs` |
| DTO | `IAM-Utilities/DTO/UtilitiesMdl/IDC/IDentificationCard.cs` (`UploadKYCDetails`) |
| Service | `IAM-Utilities/DSS/Services/KYCServices/KYCServices.cs` |
| Library client | `IAM_Library/appWallet/main/account/WalletAccountApiClient.cs` — `POSTUploadKYC` |
| Library loader | `IAM_Library/appWallet/main/account/WalletAccountDetailLoader.cs` — `POSTSendKYCForVerification` |
| App service | `IAM-Wallet/Services/IAM_AppServices.cs` — `UploadKYC` |
| UI | `IAM-Wallet/Components/Pages/KYC/KycReview.razor`, capture pages under same folder |
| Shared KYC state | `IAM-Wallet/Models/User/UserBasicDetails.cs` — `staticKYCChoices` |
