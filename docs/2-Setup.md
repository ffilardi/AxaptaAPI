### Steps

1. Download the source code to a local repository;
2. Open the project on Visual Studio and configure/update the service references pointing to your AOS/Port;
3. Rebuild the project to download the external packages automatically.

**Note**: This sample utilizes the `ExpenseServices` and `UserSessionServices`. Check if both are deployed and enabled on AIF Inbound ports before configure the service reference.


### Authenticating

There are three ways to authenticate your users:

**Mode 1: Multi-User (Active Directory)**

The credential is sent via HTTPS header on each request. Any existing user on Dynamics AX can access the API under the same security rules as acessing via rich client. Credential settings available in Web.Config are ignored. Recommended for intranet/internal services running on a secure network.

**Mode 2: Single-User (Active Directory)**

A single credential must be specified in Web.Config file, through which all users will use to connect. Credential sent via HTTPS header request will be ignored. Recommended for services that exposes only shared or non-sensitive data.

**Mode 3: Third-Party Provider**

A Trusted Intermediary user credential must be specified in Web.Config file. The Claims User must be created on AX and the Trusted Intermediary user must be enabled on each exposed AIF Inbound Port. The authenticated user account is automaticall identified by the API and must match the Claim User details on AX. Recommended for services running on internet that contains individual and sensitive data.

###  Web.Config file

Open the `Web.Config` file using the following parameters to setup a pre-defined user (when required). If not specified, the credentials must be passed via request header.

    <appSettings>
        <add key="API_AUTH_MODE" value="" />
        <add key="API_AUTH_USER_DOMAIN" value="" />
        <add key="API_AUTH_USER_NAME" value="" />
        <add key="API_AUTH_USER_PASSWORD" value="" />
    </appSettings>`

`API_AUTH_MODE`  
Valid values: "_1_", "_2_" or "_3_", related to the available authentication modes.

`API_AUTH_USER_DOMAIN`, `API_AUTH_USER_NAME` and `API_AUTH_USER_PASSWORD`  
Dynamics AX user credentials for Mode 2 (Single-User) or Mode 3 (Trusted Intermediary User).


### Multi-User

If you enabled Mode 1 (Multi-User), the credential must be sent via HTTPS header request using the Basic Auth key. The username must include the domain. E.g.:

**Postman Request**  
![Postman Request](images/Postman-Request.jpg)

**HTTP Request Header** 
 
    GET /user HTTP/1.1  
    Host: localhost:44300  
    Authorization: Basic Q09OVE9TT1xhZG1pbjpwYXNzQHdvcmQx  


### Claims User

When Mode 3 (Third-Party Provider) is enabled, there are two steps to be setup on AX:

**1. Enable the Trusted Intermediary User**

The advanced settings are available only for **Enhanced** Inbound Ports - if you have a service port deployed as Basic (created through Service Groups) you need to redeploy it manually.

![Trusted Intermediary Setup](images/AX-Trusted-Intermediary_1.jpg)

![Trusted Intermediary User](images/AX-Trusted-Intermediary_2.jpg)

For more information check this link:

AIF trusted intermediaries (form) [AX 2012]  
https://technet.microsoft.com/en-us/library/hh209572.aspx

**2. Add the Claims User**

The first step to add the claims user is to identify the information that should be used on AX.

Open the authentication checking available at:
> https://\<_your-webapp-url:port_\>/auth/

`GET` https://localhost:44300/auth

For an account authenticated by Google, for example, the first three lines are:

    "IsAuthenticated": "True",
    "AuthenticationType": "google",
    "Name": "testuser@gmail.com"

The '_Network domain_' on AX should match with '_AuthenticationType_' field from API, adn the '_Alias_' field on AX should match with '_Name_' field from API.

![Claims User](images/AX-Claims_User.jpg)

Set the '_Account type_' to '_Claims user_', select any '_User ID_' and '_User name_' (they are not relevant for the authentication process), check the '_Enabled_' option, define the '_Default company_' and the user roles.
