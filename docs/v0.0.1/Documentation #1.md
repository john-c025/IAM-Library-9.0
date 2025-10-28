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
- Handles exceptions by providing default or invalid data.

##### `LoadCoverageData(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient)`
- Loads coverage data using the provided `ReportLoader`.
- Returns a list of `ReportsDataCommissionCoverage` objects.
- No error handling is implemented in this method.

##### `LoadReportComissionSummary(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission summary report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionSummary` objects.
- No error handling is implemented in this method.

##### `LoadReportComissionHistory(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads commission history report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- Returns a list of `ReportsDataCommissionHistory` objects.
- No error handling is implemented in this method.

##### `LoadReportReferralCommission(ReportLoader loader, AuthApiResponseData data, HttpClient httpClient, string option, string date_from, string date_to)`
- Loads referral commission report data using the provided `ReportLoader`.
- Accepts additional parameters for specifying options and date range.
- `1` for `referral comission`
- `2` for `DSC comission`
- `3` for `ISC comission`
- `4` for `J4U comission`
- `5` for `GM5 comission`
- Returns a list of `ReportsDataCommissionHistory` objects.

#### Note on Loaders
- Loaders such as `AccountDetailLoader` and `ReportLoader` are used to abstract away the details of data retrieval.
- They facilitate the loading of various types of data (account details, reports) from external sources.
- Error handling are also abstracted within the loaders



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
- [ ] Reports - [Pairs History]
- [ ] Reports - [Unilevel Commission]


### Phase II - Genealogy

- [ ] Genealogy -

***