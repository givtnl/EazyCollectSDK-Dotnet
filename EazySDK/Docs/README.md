
# Table of contents


- [Configuration](#configuration)
  -  [Available settings](#available-settings)
      - [currentEnvironment](#currentenvironmentenvironment)
      - [sandboxClientDetails](#sandboxclientdetails)
      - [ecm3ClientDetails](#ecm3clientdetails)
      - [directDebitProcessingDays](#directdebitprocessingdays)
      - [contracts](#contracts)
      - [payments](#payments)
      - [warnings](#warnings)
      - [other](#other)
- [Functions](#funcions)
  - [Get](#get)
      - [CallbackUrl](#callbackurl)
      - [Customers](#customers)
      - [Contracts](#contracts-1)
      - [Payments](#payments-1)
      - [PaymentsSingle](#paymentsSingle)
      - [Schedules](#schedules)
  - [Post](#post)
      - [CallbackUrl](#callbackurl-1)
      - [Customer](#customer)
      - [Contract](#contract)
      - [Payment](#payment)
  - [Patch](#patch)
    - [Customer](#customer-1)
    - [ContractAmount](#contractamount)
    - [ContractDateMonthly](#contractdatemonthly)
    - [ContractDateAnnually](#contractdateannually)
    - [Payment](#payment-1)
  - [Delete](#delete)
    - [Callback_url](#callback_url-2)
    - [Payment](#payment-2)
- [Exceptions](#Exceptions)
  - [EazySDKException](#eazysdkexception)
    - [UnsupportedHTTPMethodError](#unsupportedhttpmethoderror)
    - [SDKNotEnabledError](#sdknotenablederror)
    - [ResourceNotFoundError](#resourcenotfounderror)
    - [InvalidParameterError](#invalidparametererror)
    - [EmptyRequiredParameterError](#emptyrequiredparametererror)
    - [ParameterNotAllowedError](#parameternotallowederror)
    - [InvalidEnvironmentError](#invalidenvironmenterror)
    - [InvalidStartDateError](#invalidstartdateerror)
    - [InvalidPaymentDateError](#invalidpaymentdateerror)
    - [RecordAlreadyExistsError](#recordalreadyexistserror)
    - [InvalidSettingsConfiguration](#invalidsettingsconfiguration)



## Configuration

Configuring EazySDK is simple, and can be done in two ways. The settings file, `appsettings.json` for EazySDK is generated in the relative root directory of the project, or to configure the settings inline, it's as easy as `Settings.GetSection(Root)[Setting] = Varaible;`.

### Available settings

#### currentEnvironment[Environment]

Defines the current working environment of EazySDK. As many of our clients own a sandbox test account, the ability to switch between the live and sandbox environment at ease is crucial.

##### Acceptable arguments

*sandbox*

The sandbox test environment. All data submitted to this environment will not be processed by BACS, and no collections will be taken from customers.

*ecm3*

The live working environment. All data submitted to this environment will be processed by BACS, sending test information to the live environment may result in a client being charged.

#### sandboxClientDetails

The API authorisation details for the sandbox test environment.

##### Acceptable arguments

*ClientCode*

The sandbox environment client code used when making API calls to Eazy Customer Manager.

*ApiKey*

The sandbox environment API key used when making API calls to Eazy Customer Manager.


#### ecm3ClientDetails

The API authorisation details for the live working environment.

##### Acceptable arguments

*ClientCode*

The live working environment client code used when making API calls to Eazy Customer Manager.

*ApiKey*

The live working environment API key used when making API calls to Eazy Customer Manager.


#### directDebitProcessingDays

The number of days the SDK expects a Direct Debit take in processing. **NOTE:** This will not reduce the number of days any Direct Debits take to process, it will only change the number of days the SDK *expects* a Direct Debit to take in processing.  Changing any sub settings within this setting  ***will*** cause collections to be missed without prior arrangements being made. We ***STRONGLY*** advise these settings are not changed without prior consultation with Eazy Collect.

##### Acceptable arguments

*InitialProcessingDays*

The length of time in days a Direct Debit takes to process when it is initially created. By default, this is set to `10` working days.

*OngoingProcessingDays*

The length of time in days a Direct Debit takes to process on subsequent payments. By default, this is set to `5` working days.

#### contracts

A collection of settings to automatically fix common mistakes made when creating contracts in Eazy Customer Manager.

##### Acceptable arguments

*AutoFixStartDate*

If the `StartDate` is too soon, and this is set to `TRUE`, the SDK will automatically set the `StartDate` to the next available. By default, this is set to `FALSE`.

*AutoFixTerminationTypeAdHoc*

Ad-Hoc contracts mandate that the `TerminationType` is set to `Until further notice`. If this is set to `TRUE`, the SDK will automatically set the `TerminationType` to `Until further notice`. By default, this is set to `FALSE`.

*AutoFixAtTheEndAdHoc*

Ad-Hoc contracts mandate that `AtTheEnd` is set to `Switch to further notice`. If this is set to `TRUE`, the SDK will automatically set `AtTheEnd` to `Switch to further notice`. By default, this is set to `FALSE`.

*AutoFixPaymentDayInMonth*

If the `PaymentDayInMonth` does not match the day in `StartDate`, and this is set to`TRUE`, the SDK will automatically set the `PaymentDayInMonth` to the day in `StartDate`. By default, this is set to `FALSE`.

*AutoFixPaymentMonthInYear*

If the `PaymentMonthInYear` does not match the year in `StartDate`, and this is set to`TRUE`, the SDK will automatically set the `PaymentMonthInYear` to the year in `StartDate`. By default, this is set to `FALSE`.


#### payments

A collection of settings to automatically fix common mistakes made when creating payments in Eazy Customer Manager.

##### Acceptable arguments

*AutoFixPaymentDate*

If the `PaymentDate` is too soon,  and this is set to`TRUE`, the SDK will automatically set the `PaymentDate` to the next available. By default, this is set to `FALSE`.

*IsCreditAllowed*

Defines whether or not a client can credit a customer. **NOTE:** This does not change whether or not a client can credit a customer, but whether or not the SDK *expects* a client can credit a customer. By default, this is set to `FALSE`.


#### warnings

A collection of warnings to alert the client of potential consequences of using EazySDK.

##### Acceptable arguments

*CustomerSearch*

If set to `TRUE`, this will alert the client when searching for customers without any parameters that EazySDK may take some time to retrieve all customers belonging to the client. By default, this is set to `TRUE`.


#### other

A collection of utilities functions used to streamline the process of using EazySDK.

##### Acceptable arguments

*BankHolidayUpdateDays*

Defines the number of days EazySDK will wait before pinging the [bank holidays json file](https://www.gov.uk/bank-holidays.json). By default this is set to `30`. We recommend leaving this setting at `30`.

*ForceUpdateSchedulesOnRun*

If this is set to `true`, every time a call is made through EazySDK which interacts with schedules EazySDK will call `Get.Schedules()` in the background, and save the contents to the `./Includes` folder. We recommend leaving this setting as is, though if you are experiencing issues with schedules, this is a useful diagnostic tool.



## Functions

EazySDK has been designed with simplicity in mind. All functions used in the base framework originate from four different HTTP methods, `GET`, `POST`, `PATCH` and `DELETE`. If you'd like to delete a payment, it's as simple as `Delete.Payment({params})`. A list of all functions can be found below, but all also have a useful summary which can be accessed by viewing the source code on GitHub.

### Get

Sends a `GET` request to EazyCustomerManager. Optionally, parameters may be passed. Returns a JSON object or a string.

#### CallbackUrl

Get the currently active callback URL for data returned from EazyCustomerManager.

*Example*

Get.CallbackUrl()

*Returns*

string - 'The callback URL is {url}'


#### Customers

Search EazyCustomerManager for a single or a set of customers. **Note** if the setting `warnings['customer_search']` is set to `TRUE`, you will be warned if attempting to search for customers without passing any parameters.

*Optional parameters*

 - *Email* - The full email address of a single customer
 - *Title* - The title of a customer or a set of customers
 - *SearchFrom* - Search for customers created on or after this date
 - *SearchTo* - Search for customers created on or before this date
 - *DateOfBirth* - The full date of birth of a customer or a set of customers
 - *CustomerReference* - The full or partial customer reference of a customer or a set of customers
 - *FirstName* - The full or partial first name of a customer or a set of customers
 - *Surname* -  The full or partial surname of a customer or a set  of customers
 - *CompanyName* - The full or partial company name of a customer or a set of customers
 - *PostCode* - The full or partial post code of a customer or a set of customers
 - *AccountNumber* - The full bank  account number of a customer  
- *SoirtCode* - The full sort code of a customer or a set of  customers
 - *AccountHolderName* - The full or partial account holder name of a customer or a set of customers
 -  *HomePhone* - The full or partial home phone number of a customer or a set of customers
 - *WorkPhone* - The full or partial work telephone number of a customer or a set of customers
 - *MobilePhone* - The full or partial mobile phone number of a  customer or a set of customers

*Example*

Get.Customers(PostCode: "GL52 2NF");

*Returns*

string - customers JObject

#### Contracts

Search EazyCustomerManager for a list of contracts owned by a specific customer.

*Required parameters*

 - *Customer* - The GUID of the customer being searched against

*Example*

Get.Contracts("a1ddc068-51dx-4c6d-bf9c-7866a71c6c43");

*Returns*

string - contracts JObject


#### Payments

Search EazyCustomerManager for a list of all payments belonging to a specific contract.

*Required parameters*

 - *Contract* - The GUID of the contract being searched against


*Optional parameters*

- *NumberOfRows* - The number of payments to return from a contract. By default, this is set to `100`. 

*Example*

Get.Payments("311228a5-98f5-4bd8-b1b6-023d09ca8b32",  "10");

*Returns*

string - payments JObject

#### PaymentsSingle

Search EazyCustomerManager for a specific payment owned by a specific contract

*Required parameters*

 - *Contract* - The GUID of the contract being searched against
 - *Payment* - The GUID of the payment being searched against

*Example*

Get.PaymentsSingle("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "13bdf192-86f1-4979-ae00-250a4722e20b")

*Returns*

string - Payment JObject


#### Schedules

Search EazyCustomerManager for all available schedules. **Note:** These can be found without making this call, by  viewing either `sandboxscheduleslist.json` or `ecm3scheduleslist.json` in the `./Includes` directory.

*Example*

Get.Schedules()

*Returns*

string - Schedules JObject


### Post

Sends a `POST` request to EazyCustomerManager. Optionally, parameters may be passed. Returns a JSON object or a string.

#### CallbackUrl

Create or update the callback URL used to return data from EazyCustomerManager. **Note:** Although it isn't required, we strongly recommend using a URL secured by the HTTPS protocol. 

*Required parameters*

- *CallbackUrl* - The url of the new location data will be returned to from EazyCustomerManager

*Example*

Post.CallbackUrl("https://eazycollect.co.uk")

*Returns*

string - 'The new callback URL is {url}'


#### Customer

Create a customer within EazyCustomerManager. **Note**: The SDK will not perform any modulus checks when creating a customer. This is the responsibility of the client, however we can offer a pay-per-use API to achieve this. For more details, contact our sales department via `01242 650052`.

*Required parameters*

- *Email* - The email address of the newly created customer. This must be unique.
- *Title* - The title of the newly created customer
- *CustomerReference* - The customer reference of the newly created customer. This must be unique.
- *FrstName* - The first name of the newly created customer  
- Surname - The surname of the newly created customer  
- *Line1* - The first line of the newly created customers address
- *PostCode* - The post code of the newly created customers address
- *AccountNumber* - The bank account number of the newly created customer
- *SortCode* - The sort code of the newly created customer
- *AccountHolderName* - The name as it appears on the newly created customers bank account

*Optional parameters*

- *Line2* - The second line of the newly created customers address
- *Line3* - The third line of the newly created customers address  
- *Line4* - The fourth line of the newly created customers address  
- *CompanyName* - The name of the company the newly created customer represents
- *DateOfBirth* - The date of birth of the newly created customer
- *HomePhone* - The home phone number of the newly created customer  
- *WorkPhone* - The work phone number of the newly created customer  
- *MobilePhone* - The mobile phone number of the newly created customer

*Example*

Post.Customer("test@email.com", "Mr", "test-0001", "Test", "Ing", "1 Tebbit Mews", "GL52 2NF", "00000000", "000000", "MR TEST ING", work_phone: "00000000000");

*Returns*

customer JSON object

#### Contract

Create a contract within EazyCustomerManager. It is important to note that due to the complexity of contracts, there are several rules dictating the general flow of contract creation. To aid this, we've created the `contracts` set of settings, which helps by automatically fixing some of the common issues found when creating contracts.

*Required parameters*

- *Customer* - The GUID of the customer the newly created contract will belong to
- *ScheduleName* - The name of the schedule the newly created contract will belong to
- *StartDate* - The date the newly created contract will start
- *GiftAid* - Whether the newly created contract is eligible for gift aid or not
- *TerminationType* - The method of termination for the newly created contract
- *AtTheEnd* - What happens to the newly created contract after the termination event has been triggered

*Optional parameters*

- *NumberOfDebits* - Mandatory if `TerminationType` is set to `Collect certain number of debits`
- *Frequency* - Mandatory if the newly created contract is not ad-hoc. This parameter allows you to skip periods (e.g a value of 2 would collect every 2 months rather than every 1 month)
- *InitialAmount* - Called if the first collection amount is different from the regular `PaymentAmount`. Not to be used on ad-hoc contracts.
- *ExtraInitialAmount* - Called if any additional charges are present, such as a gym registration fee. Not to be used on ad-hoc contracts.
- *PaymentAmount* - Mandatory if the contract is not ad-hoc. The regular collection amount for the newly created contract
- FinalAmount- Used if the final payment amount differs from the rest. Not to be used on ad-hoc contracts.
- *PaymentMonthInYear* - The collection month for annual contracts. Jan =` 1`, Dec = `12`
- *PaymentDayInMonth* - The collection day for monthly contracts. Accepts `1`-`28` or `last day of month`
- *PaymentDayInWeek* - The collection day for weekly contracts. Monday = `1`, Friday = `5`
- *TerminationDate* - The termination date of the newly created contract. Mandatory if `TerminationType` is set to `End on exact date`
- *AdditionalReference* - An additional reference for the newly created contract.
- *CustomDDReference* - A custom Direct Debit reference for the newly created contract.

*Example*

Post.Contract("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "adhoc_free", "2019-06-15", false, "Until further notice", "Switch to further notice")

*Returns*

contract JSON object

#### Payment

Create a payment against a contract in EazyCustomerManager.

*Required parameters*

- *Contract* - The GUID of the contract the newly created payment will belong to
- *PaymentAmount* - The total amount the newly created payment will collect
- *CollectionDate* - The desired collection date date of the newly created payment
- *Comment* - A comment to supplement the newly created payment

*Optional parameters*

- *IsCredit* - Passed if the payment is a credit to the customer, rather than a debit. **Note:** If the setting `payment['IsCreditAllowed']` is set to `FALSE`, this will automatically be set to `FALSE`, regardless of whether it is set to `TRUE` or not..


*Example*

Post.Payment("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "10.50", "2019-06-15", "Customer needs to pay £10.50");

*Returns*

payment JSON object


### Patch

Sends a `PATCH` request to EazyCustomerManager. Optionally, parameters may be passed. Returns a JSON object.

#### Customer

Modify a customer within EazyCustomerManager.


*Required parameters*
- *Customer* - The GUID of the customer to be modified

*Optional parameters*

- *Email* - The new email address of the customer. This must be unique.
- *Title* - The new title of the customer
- *DateOfBirth* - The amended date of birth of the customer
- *FirstName* - The amended first name of the customer  
- *Surname* - The amended surname of the customer  
- *Line1* - The new first line of the customers address
- *PostCode* - The new post code of the customers address
- *AccountNumber* - The new bank account number of the customer
- *SortCode* - The new sort code of the customer
- *AccountHolderName* - The amended name as it appears on the customers bank account
- *Line2* - The new second line of the customers address
- *Line3* - The new third line of the customers address  
- *Line4* - The new fourth line of the customers address  
- *CompanyName* - The name of the new company the customer represents
- *HomePhone* - The new home phone number of the customer  
- *WorkPhone* - The new work phone number of the customer  
- *MobilePhone* - The new mobile phone number of the customer

*Example*

Patch.Customer("311228a5-98f5-4bd8-b1b6-023d09ca8b32", AccountNumber: "00000000", SortCode: "000000");

*Returns*

string - 'The customer {customer} has been updated successfully'

#### ContractAmount

Modify the collection amount of a contract. **Note** Any amendments to the contract amount will not take action on the next payment if the next payment is within the standard amount of days to collect a payment.


*Required parameters*
- *Contract* - The GUID of the contract to be modified
- *PaymentAmount* - The new collection amount of the contract
- *Comment* - A comment as to why the contract collection amount was changed

*Example*

Patch.ContractAmount("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "10.0", "A customer");

*Returns*

string - 'The contract {Contract} has been updated with the new regular collection amount of {PaymentAmount}'

#### ContractDayMonthly

Modify the collection day on a monthly contract. **Note** Any amendments to the contract amount will not take action on the next payment if the next payment is within the standard amount of days to collect a payment.


*Required parameters*

- *Contract* - The GUID of the contract to be modified
- *NewPaymentDay* - The new collection day of the contract
- *Comment* - A comment as to why the contract collection amount was changed
- *AmendNextPayment* - Whether or not the contract collection amount will change

*Optional parameters*

- *NextPaymentAmount* - If `AmendNextPayment` is `TRUE`, the collection amount of the following monthly collection

*Example*

Patch.ContractDayMonthly("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "15", "A customer", false);

*Returns*

'contract {contract} day has been updated to {new_day}'


#### ContractDayAnnually

Modify the collection day on an annual contract. **Note** Any amendments to the contract amount will not take action on the next payment if the next payment is within the standard amount of days to collect a payment.


*Required parameters*
- *Contract* - The GUID of the contract to be modified
- *NewPaymentDay* - The new collection day of the contract
- *NewPaymentMonth* - The new collection month of the contract
- *Comment* - A comment as to why the contract collection amount was changed
- *AmendNextPayment* - Whether or not the contract collection amount will change

*Optional parameters*
- *NextPaymentAmount* - If `AmendNextPayment` is `TRUE`, the collection amount of the following monthly collection

*Example*

Patch.ContractDayAnnually("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "15", "6", "A customer", false);

*Returns*

string  - 'The contract {0} has been updated with the new regular collection day of {NewPaymentDay} and regular collection month of {NewPaymentMonth}'

#### Payment

Modify a payment belonging to a contract. **Note** Payments can not be amended after they have been submitted to BACS.


*Required parameters*
- *Contract* - The GUID of the contract to be modified
- *Payment* - The GUID of the payment to be modified
- *PaymentAmount* - The modified collection amount of the specified payment
- *PaymentDay* - The modified collection date of the specified payment
- *Comment*- A comment as to why the payment was changed

*Example*

Patch.Payment("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "13bdf192-86f1-4979-ae00-250a4722e20b", "10.50", "2019-06-15", "A payment");

*Returns*

string - 'The payment {Payment} has been updated with the new collection day of {PaymentDay} and collection amount of {PaymentAmount}'


### Delete

Sends a `DELETE` request to EazyCustomerManager. Optionally, parameters may be passed. Returns a JSON object or a string.

#### CallbackUrl

Remove the current `CallbackUrl` from EazyCustomerManager

*Example*

Delete.Callback_url()

*Returns*

string - 'Callback URL deleted'

#### Payment

Delete a payment from EazyCustomerManager providing it hasn't already been submitted to BACS for processing.


*Required parameters*
- *Contract* - The GUID of the contract the payment belongs to
- *Payment* - The GUID of the payment to be deleted
- *Comment* - A comment as to why the payment is being deleted

*Example*

Delete.Payment("311228a5-98f5-4bd8-b1b6-023d09ca8b32", "13bdf192-86f1-4979-ae00-250a4722e20b", "A payment");

*Returns*

string - 'Payment {Payment} deleted'

## Exceptions
EazySDK employs custom exceptions in an effort to give concise, descriptive reasoning in any situation.

### EazySDKException
All exceptions thrown by EazySDK derrive from the EazySDKException base exception

#### UnsupportedHTTPMethodError
`UnsupportedHTTPMethodError` is a generic error. Several causes of `UnsupportedHTTPMethodError` include using an unsupported HTTP method, such as `DELETE` on a contract, a mandatory field has been missed (And `EmptyRequiredParameterError` is not raised) or the base URL is incorrect.

#### SDKNotEnabledError
`SDKNotEnabledError` is a generic error. Several causes of `SDKNotEnabledError` include the API key being incorrect, the API not being enabled, or a record not existing (Where `ResourceNotFoundError` is not raised).

#### ResourceNotFoundError
`ResourceNotFoundError` is raised when the requested resource could not be found. This doesn't necesarily mean the resource does not exist. It could also mean the request is malformed, and EazyCustomerManager has not received the expected input.

#### InvalidParameterError
`InvalidParameterError` is raised when one or more the parameters passed into a call are malformed. For example, `Until further note` is a malformed `termination_type`, which should be `Until further notice`.

#### EmptyRequiredParameterError
`EmptyRequiredParameterError` is thrown when a parameter which is required for the call has not been passed.

#### ParameterNotAllowedError
`ParameterNotAllowedError` is thrown when a parameter not allowed for the call has been passed.

#### InvalidEnvironmentError
`InvalidEnvironmentError` is thrown when the environment set in `settings.current_environment` is not set to `sandbox` or `ecm3`.

#### InvalidStartDateError
`InvalidStartDateError` is thrown when the start date is not valid. This could mean the start date is too soon, or it is malformed.

#### InvalidPaymentDateError
`InvalidPaymentDateError` is thrown when the payment date is not valid. This could mean the payment date is too soon, or it is malformed.

#### RecordAlreadyExistsError
`RecordAlreadyExistsError` is thrown when a record already exists in the case of creating or patching a record.

#### InvalidSettingsConfiguration
`InvalidSettingsConfiguration` is thrown when a setting is not valid.
