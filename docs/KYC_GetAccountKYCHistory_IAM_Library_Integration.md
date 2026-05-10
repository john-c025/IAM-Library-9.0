# KYC GetAccountKYCHistory — IAM_Library integration guide

This document summarizes what was **added** to **IAM_Library** for **`GET /v1/KYC/GetAccountKYCHistory`**, so app teams (e.g. **IAM-Wallet** / Android / Blazor) can consume account KYC history from the **apiutil** host.

**Upstream reference:** IAM-Utilities `KYCController.GetAccountKYCHistory` — query parameter **`accountid`** (lowercase).

**Example URL:** `https://apiutil.iam-worldwidecorp.com/v1/KYC/GetAccountKYCHistory?accountid=88888888`

---

## 1. Models (`IAM_Library.appWallet.models.dashboard`)

**File:** `appWallet/models/dashboard/MainAccountInfo.cs`

| Type | Purpose |
|------|---------|
| `GetAccountKYCHistoryApiResponse` | Deserializes the **success** JSON wrapper: `status`, `total`, `history`. |
| `WalletAccountKYCHistoryItem` | One row in `history` (upload metadata, ID info, file type/path, status, nested `logs`). |
| `WalletKYCHistoryLog` | One entry inside `logs` (update date, tran no, status, remarks, verifier, etc.). |

Property names use **camelCase** to align with typical ASP.NET Core JSON output (`history`, `dateUploaded`, `fileTypeID`, `logs`, …).

---

## 2. `WalletAccountsApiClient` (`appWallet/main/account/WalletAccountApiClient.cs`)

Construct with wallet base URL, `WalletAuthResponseData`, and shared `HttpClient` (same constructor pattern as other wallet calls).

### `LoadAccountKYCHistory`

```csharp
Task<ApiResponseModel<List<WalletAccountKYCHistoryItem>>> LoadAccountKYCHistory(string accountId)
```

- **Base URL:** `Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC)` (e.g. `https://apiutil.iam-worldwidecorp.com`) — **not** `baseUrlWallet`.
- **Path:** `_wallet_endpoints.GetAccountKYCHistory` (plain string, see §4).
- **Full URL:** `{baseUrlWalletKYC}{GetAccountKYCHistory}?accountid={Uri.EscapeDataString(accountId)}`
- **Method:** GET.
- **Auth:** `ApiKey` header only — `DefaultRequestHeaders.Clear()` then `Add("ApiKey", "...")` (same hardcoded utilities key pattern as `POSTUploadKYC` / `LoadMainAccountKYCLevel` in this client). No wallet Bearer on this call.

**Behavior**

- Empty or whitespace `accountId` → returns `IsSuccess = false`, `StatusCode = 400`, no HTTP call.
- **HTTP 200:** Deserializes `GetAccountKYCHistoryApiResponse`, returns `Data = history` (or empty list if `history` is null).
- **Non-success (e.g. 404 Not Found, 400 Bad Request):** `IsSuccess = false`, `StatusCode` = HTTP status, `Description` = raw response body (often JSON from the API).

---

## 3. `WalletAccountDetailLoader` (`appWallet/main/account/WalletAccountDetailLoader.cs`)

Thin wrapper: constructs `WalletAccountsApiClient` with `Encryption.decodeString(_wallet_endpoints.baseUrlWallet)`, credentials, and `HttpClient`, then delegates.

| Method | Calls |
|--------|--------|
| `LoadAccountKYCHistoryLoader(credentials, httpClient, accountId)` | `LoadAccountKYCHistory` |

On exception, returns `ApiResponseModel` with `IsSuccess = false`, `StatusCode = 500`, `Description = ex.Message`.

**Note:** The loader passes the usual **wallet** base URL into the client constructor for consistency with other loaders; the **KYC history** method itself still uses **`baseUrlWalletKYC`** inside `WalletAccountsApiClient` when building the request URL (same pattern as `LoadMainAccountKYCLevel`).

---

## 4. Endpoints configuration

**File:** `appWallet/main/api/Endpoints.cs`

| Constant | Value | Notes |
|----------|--------|--------|
| `GetAccountKYCHistory` | `/v1/KYC/GetAccountKYCHistory` | Plain path (no base64/encryption). Concatenated after `baseUrlWalletKYC`. |

| Related | Purpose |
|---------|--------|
| `baseUrlWalletKYC` | Decodes to the **apiutil** host (e.g. `https://apiutil.iam-worldwidecorp.com`). |

---

## 5. What the consuming app should do

1. **Account ID:** Pass the same account identifier the API expects in **`accountid`** (query), typically the wallet account ID string used elsewhere in KYC flows.
2. **Credentials / `HttpClient`:** Use the same pattern as other `WalletAccountDetailLoader` calls (valid `WalletAuthResponseData` + app `HttpClient`). The **ApiKey** is applied inside the client for this endpoint; the user token is not sent on this util call.
3. **Success UI:** Bind `response.Data` (`List<WalletAccountKYCHistoryItem>`); iterate `logs` per row if you need audit / status history.
4. **Empty history:** The API may return **404** with a JSON body when no rows exist; treat `IsSuccess == false` and show a friendly “no history” message using `Description` or parsed JSON if you add app-side parsing.
5. **ApiKey alignment:** If IAM-Utilities `appsettings` `ApiKey` differs from the hardcoded string in `WalletAccountApiClient`, update the client to use `Encryption.decodeString(_wallet_endpoints.api_key)` or align server configuration.

---

## 6. API response shape (reference)

**Success (200):** Body is approximately:

```json
{
  "status": "success",
  "total": 1,
  "history": [ { "...": "..." } ]
}
```

The library maps this to `GetAccountKYCHistoryApiResponse` and exposes **`history`** as `List<WalletAccountKYCHistoryItem>` in `ApiResponseModel.Data`.

**Errors:** Shape varies (e.g. `NotFound` / `BadRequest` objects from the controller). The library surfaces the raw body in `Description` on failure.

---

## 7. Quick reference — namespaces

- Models: `IAM_Library.appWallet.models.dashboard`
- Client: `IAM_Library.appWallet.account.WalletAccountsApiClient`
- Loader: `IAM_Library.appWallet.account.WalletAccountDetailLoader`
