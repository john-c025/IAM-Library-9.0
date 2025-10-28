## Documentation for the IAM_Library v0.0.1

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Blazor](https://img.shields.io/badge/blazor-%235C2D91.svg?style=for-the-badge&logo=blazor&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)



#### Overview

The `IAM Library` serves as a crucial component in the infrastructure of the `IAM Mobile App`, focusing primarily on abstracting away complexities related to data retrieval, validation, security, and other intricacies inherent to the app's functionality.


## v0.0.1 `Authentication` `Account Details` `Reports`

#### Methods [Based on the current sample implementation]

##### `SignIn()`
- This method initiates the authentication process.
- Makes use of the AuthBridge class of the IAM_Library for Authentication
- To encode the username and password, use `AuthBridge.AuthEncode`.
- to authenticate the user with the encoded credentials use `AuthBridge.Authenticate`.
- Upon successful authentication, it retrieves additional authentication response data.


##### `LoadAccountData(AccountDetailLoader detailLoader, AuthApiResponseData data, HttpClient httpClient)`
- Loads account data using the provided `AccountDetailLoader`.
- Returns an `AccountDetailData` object containing account details.

##### `LoadCoverageData(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient)`
- Loads coverage data using the provided `ReportLoader`.
- Returns a list of `ReportsDataCommissionCoverage` objects.

##### `LoadReportComissionSummary(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission summary report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionSummary` objects.

##### `LoadReportComissionHistory(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission history report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionHistory` objects.

##### `LoadReportReferralCommission(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads referral commission report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- `1` for `referral commission`
- `2` for `DSC commission`
- `3` for `ISC commission`
- `4` for `J4U commission`
- `5` for `GM5 commission`
- Returns a list of `commission` objects based on option parameter.

#### Note on Loaders
- Loaders such as `AccountDetailLoader` and `ReportLoader` are used to abstract away the details of data retrieval.
- They facilitate the loading of various types of data (account details, reports) from the API.
- Error handling are also abstracted within the loaders

#### Current Working Loaders, Functions, Bridge
### AccountDetailLoader
  - `LoadAccountData(AuthApiResponseData credentials, HttpClient _httpClient)`
###  ReportLoader
  - `LoadCoverageList`
  - `LoadCoverageListAsDict`
  - `LoadCommissionSummary(string option, string date_from, string date_to)`
  - `LoadCommissionSummaryAsDict(string option, string date_from, string date_to)`
  - `LoadCommissionHistory(string option, string date_from, string date_to)`
  - `LoadCommissionHistoryAsDict(string option, string date_from, string date_to)`
  - `LoadReferralCommissionAsDict(string option, string date_from, string date_to)`
###  AuthBridge
  - `AuthEncode(string username, string password)`
  - `Authenticate(CredentialModel creds,HttpClient httpClient)`
  - `AuthenticateWithResponseNodel(CredentialModel credentials, HttpClient httpClient)`
  - `GetAuthResponse(AuthApiResponseData authResponse, CredentialModel cred)`

## Current Progress:

***

### Phase I - Authentication, Account Details, Reports  

![](https://geps.dev/progress/75)

- [X] Auth
- [X] Account Details
- [X] Reports - [Referral Commission]
- [X] Reports - [DSC Commission]
- [X] Reports - [ISC Commission]
- [X] Reports - [J4U Commission]
- [X] Reports - [GM5 Commission]
- [X] Reports - [Pairs History]
- [X] Reports - [Unilevel Commission]


### Phase II - Genealogy

- [X] Genealogy
- [X] 

***
## Documentation for the IAM_Library v0.0.1

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Blazor](https://img.shields.io/badge/blazor-%235C2D91.svg?style=for-the-badge&logo=blazor&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=for-the-badge&logo=android&logoColor=white)



#### Overview

The `IAM Library` serves as a crucial component in the infrastructure of the `IAM Mobile App`, focusing primarily on abstracting away complexities related to data retrieval, validation, security, and other intricacies inherent to the app's functionality.


## v0.0.1 `Authentication` `Account Details` `Reports`

#### Methods [Based on the current sample implementation]

##### `SignIn()`
- This method initiates the authentication process.
- Makes use of the AuthBridge class of the IAM_Library for Authentication
- To encode the username and password, use `AuthBridge.AuthEncode`.
- to authenticate the user with the encoded credentials use `AuthBridge.Authenticate`.
- Upon successful authentication, it retrieves additional authentication response data.


##### `LoadAccountData(AccountDetailLoader detailLoader, AuthApiResponseData data, HttpClient httpClient)`
- Loads account data using the provided `AccountDetailLoader`.
- Returns an `AccountDetailData` object containing account details.

##### `LoadCoverageData(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient)`
- Loads coverage data using the provided `ReportLoader`.
- Returns a list of `ReportsDataCommissionCoverage` objects.

##### `LoadReportComissionSummary(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission summary report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionSummary` objects.

##### `LoadReportComissionHistory(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission history report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionHistory` objects.

##### `LoadReportReferralCommission(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads referral commission report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- `1` for `referral commission`
- `2` for `DSC commission`
- `3` for `ISC commission`
- `4` for `J4U commission`
- `5` for `GM5 commission`
- Returns a list of `commission` objects based on option parameter.

#### Note on Loaders
- Loaders such as `AccountDetailLoader` and `ReportLoader` are used to abstract away the details of data retrieval.
- They facilitate the loading of various types of data (account details, reports) from the API.
- Error handling are also abstracted within the loaders

#### Current Working Loaders, Functions, Bridge
### AccountDetailLoader
  - `LoadAccountData(AuthApiResponseData credentials, HttpClient _httpClient)`
###  ReportLoader
  - `LoadCoverageList`
  - `LoadCoverageListAsDict`
  - `LoadCommissionSummary(string option, string date_from, string date_to)`
  - `LoadCommissionSummaryAsDict(string option, string date_from, string date_to)`
  - `LoadCommissionHistory(string option, string date_from, string date_to)`
  - `LoadCommissionHistoryAsDict(string option, string date_from, string date_to)`
  - `LoadReferralCommissionAsDict(string option, string date_from, string date_to)`
###  AuthBridge
  - `AuthEncode(string username, string password)`
  - `Authenticate(CredentialModel creds,HttpClient httpClient)`
  - `AuthenticateWithResponseNodel(CredentialModel credentials, HttpClient httpClient)`
  - `GetAuthResponse(AuthApiResponseData authResponse, CredentialModel cred)`

## Current Progress:

***

### Phase I - Authentication, Account Details, Reports  

![](https://geps.dev/progress/75)

- [X] Auth
- [X] Account Details
- [X] Reports - [Referral Commission]
- [X] Reports - [DSC Commission]
- [X] Reports - [ISC Commission]
- [X] Reports - [J4U Commission]
- [X] Reports - [GM5 Commission]
- [X] Reports - [Pairs History]
- [X] Reports - [Unilevel Commission]


### Phase II - Genealogy

- [X] Genealogy
- [X] 

***
