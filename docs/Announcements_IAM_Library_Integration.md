# Announcements API — IAM_Library integration guide

This document summarizes what was added to **IAM_Library** for the wallet announcements feature so app teams (e.g. **IAM-Wallet** / Blazor) can wire endpoints, auth, and UI.

**Prerequisite:** Populate the plain-string GET endpoints in `appWallet/main/api/Endpoints.cs` (`GetActiveAnnouncements`, `GetAllAnnouncements`, `GetAnnouncementById`) to match the wallet API host routes (see IAM-Utilities `AnnouncementsController` routes under `v1/Announcements/...`).

---

## 1. Models (`IAM_Library.appWallet.models.dashboard`)

**File:** `appWallet/models/dashboard/AnnouncementModels.cs`

| Type | Purpose |
|------|---------|
| `WalletAnnouncementItem` | One row from GET list endpoints (id, title, type, content, `mediaFiles` string, urgency, dates, status, `updatedAt`, `updatedBy`). |
| `AnnouncementOperationResponse` | Create/Update JSON body: `statusCode` (1 = success, 0 = error), `message`. |
| `UpdateAnnouncementRequest` | PUT JSON body for `UpdateAnnouncement`. |
| `CreateAnnouncementRequest` | Multipart fields (`Title`, `AnnouncementType`, …) plus `MediaFilePaths` — local file paths; each existing file is sent as form part name **`MediaFiles`**. |

**Note:** Server DTOs may use nullable `DateTo`; if you need “no end date”, consider aligning `WalletAnnouncementItem` / `UpdateAnnouncementRequest` with `DateTime?` in a future revision.

---

## 2. `WalletAccountsApiClient` (`appWallet/main/account/WalletAccountApiClient.cs`)

Construct with wallet base URL, `WalletAuthResponseData`, and shared `HttpClient` (same pattern as other wallet calls).

### 2.1 `GetActiveAnnouncements`

```csharp
Task<ApiResponseModel<List<WalletAnnouncementItem>>> GetActiveAnnouncements(
    int? announcementType = null,
    int? urgencyType = null)
```

1 = System Announcement, 2 = Advertisement, 3 = Company Memo lang
 -- 1 = Highest Priority, 2 = Medium Priority, 3 = Low Priority

- **URL:** `_apiBaseUrl` + `GetActiveAnnouncements` + optional query string.
- **Auth:** `ApiKey` header only (same pattern as `POSTUploadKYC` in `WalletAccountApiClient` — `DefaultRequestHeaders.Clear()` then `Add("ApiKey", "...")`). No wallet Bearer on these calls.

### 2.2 `GetAllAnnouncements`

```csharp
Task<ApiResponseModel<List<WalletAnnouncementItem>>> GetAllAnnouncements(
    int? announcementType = null,
    bool? status = null)
```

- **URL:** `_apiBaseUrl` + `GetAllAnnouncements` + optional query string.
- **Auth:** Same `ApiKey` header as `GetActiveAnnouncements`.

### 2.3 `GetAnnouncementById`

```csharp
Task<ApiResponseModel<WalletAnnouncementItem>> GetAnnouncementById(int announcementId)
```

- **URL:** `_apiBaseUrl` + `GetAnnouncementById` + `{announcementId}`.
- **Auth:** Same `ApiKey` header as `GetActiveAnnouncements`.

---

## 3. `WalletAccountDetailLoader` (`appWallet/main/account/WalletAccountDetailLoader.cs`)

Thin wrappers: construct `WalletAccountsApiClient` with `Encryption.decodeString(_wallet_endpoints.baseUrlWallet)`, credentials, and `HttpClient`, then delegate.

| Method | Calls |
|--------|--------|
| `LoadActiveAnnouncementsLoader(credentials, httpClient, announcementType?, urgencyType?)` | `GetActiveAnnouncements` |
| `LoadAllAnnouncementsLoader(credentials, httpClient, announcementType?, status?)` | `GetAllAnnouncements` |
| `LoadAnnouncementByIdLoader(credentials, httpClient, announcementId)` | `GetAnnouncementById` |

On exception, loaders return `ApiResponseModel` with `IsSuccess = false`, `StatusCode = 500`, `Description = ex.Message`.

---

## 4. Endpoints configuration

**File:** `appWallet/main/api/Endpoints.cs`

| Constant | Intended route (after decode) | Notes |
|----------|------------------------------|--------|
| `GetActiveAnnouncements` | `/v1/Announcements/GetActiveAnnouncements` | GET (optional query: `announcementType`, `urgencyType`). |
| `GetAllAnnouncements` | `/v1/Announcements/GetAllAnnouncements` | GET (optional query: `announcementType`, `status`). |
| `GetAnnouncementById` | `/v1/Announcements/GetAnnouncementById/` | GET + `{announcementId}`. |

Until these are set, decoded paths are empty and calls will fail at runtime.

---

## 5. What the consuming app should do

1. **Credentials:** Pass the same `WalletAuthResponseData` used elsewhere (account key + token/signature).
2. **HttpClient:** Prefer the app-registered client with appropriate base address if your architecture merges base URL + path; the library builds URLs as `walletBaseUrl + endpointPath` (same as other wallet APIs).
3. **Read path:** Call `LoadAnnouncementsLoader` (or client directly), bind `Data` to UI; parse `mediaFiles` JSON string on the app side if you need a list of URLs/paths.
4. **Admin / create-update:** Only if the wallet app exposes announcement management; pass `CreateAnnouncementRequest` / `UpdateAnnouncementRequest`. For **Create**, ensure `MediaFilePaths` are readable paths on the device (library uses `File.ReadAllBytes`); MAUI may need to copy gallery picks to a temp file first.
5. **ApiKey value:** Announcements use the same hardcoded utilities key string as `POSTUploadKYC` in this file. If your IAM-Utilities `appsettings` `ApiKey` differs, switch this client to `Encryption.decodeString(_wallet_endpoints.api_key)` or align configuration.

---

## 6. Not implemented in this library slice

The IAM-Utilities API also exposes (no IAM_Library client methods yet unless added later):

- `DeleteAnnouncement/{id}`
- `UploadAnnouncementMedia` (multipart)
- `DeleteAnnouncementMedia` (query params)

Add matching `Endpoints` entries + client + loader methods if the app needs them.

---

## 7. Quick reference — namespaces

- Models: `IAM_Library.appWallet.models.dashboard`
- Client: `IAM_Library.appWallet.account.WalletAccountsApiClient`
- Loader: `IAM_Library.appWallet.account.WalletAccountDetailLoader`
