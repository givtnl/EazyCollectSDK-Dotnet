
# EazySDK - .NET Client
Welcome to the **EazySDK** repository.  EazySDK is an integration of the 
[Eazy Collect API version 3](https://eazycollectservices.github.io/EazyCollectAPIv3/) built in .NET Standard 2.0. Its core purpose is to provide a framework for developers already working with Eazy Collect to integrate Eazy Customer Manager into their platform. The framework provides functions designed to speed up the integration process between a developers Customer Relationship Manager and Eazy Collect. Getting started is as simple as providing user specific settings, and making your first call to Eazy Customer Manager should take less than a minute.

## Dependencies
 - [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) (12.0.2>=)
 -  [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) (>= 2.2.0)
 - [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/) (>= 2.2.0)

## Integrating EazySDK into your application
The integration process is simple, and involves importing EazySDK into your 
 virtual environment and configuring some settings. The most basic 
configuration can be seen below.

    >> PM> Install-Package EazySDK
     
    using EasySDK;
    using Microsoft.Extentions.Configuration;
    
    EazySDK.ClientHandler Handler = new ClientHandler();
    IConfiguration Settings = Handler.Settings();
    
    Settings.GetSection("currentEnvironment")["Environment"] = "sandbox"; 
    Settings.GetSection('sandboxClientDetails')["ApiKey"] = "{api_key}";
    Settings.GetSection('sandboxClientDetails')["ClientCode"] = "{client_code}";
    
    EazySDK.Get EazyGet = new EazySDK.Get(Settings);
    string Response = EazyGet.Customers();
    Console.WriteLine(Response);

## Documentation
All functions in EazySDK possess their own documentation, and can be viewed by viewing the `<summary>` associated with the selected function. The documentation can also be [found on GitHub](https://github.com/EazyCollectServices/EazyCollectSDK-DotNet/tree/master/EazySDK/docs), or in the /docs directory of the package.

## Issues
If you find any issues with EazySDK, please [raise an issue on GitHub](https://github.com/EazyCollectServices/EazyCollectSDK-DotNet/issues/new) detailing the issue. If this is not possible, alternatively email help@eazycollect.co.uk with as much information as you are able to provide.