# FrontLineLive

Front Line Live code and target operating model

- Description of the platorm
- Who built it
- Why it's important
- How to install your own instance

## GitHub Secrets

\* Should be environment specific.

### Deployment secrets

|Secret Name|Description|
|-|-|
|ARM_ACCESS_KEY|Azure Storage Account used for TF remote state. See [here](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage).|
|ARM_CLIENT_ID*||
|ARM_CLIENT_SECRET*||
|ARM_SUBSCRIPTION_ID*||
|ARM_TENANT_ID*||
|ACR_USERNAME|Username for ACR's [Admin Account](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication#admin-account).|
|ACR_PASSWORD|Password for ACR's [Admin Account](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication#admin-account).|
|ACR_SERVER|The login server for ACR|

### Application secrets

|Secret Name|Description|
|-|-|
|APP_DATA_SUPPLIERS_SHEET*|-|
|APP_EMAIL_FROMADDRESS*|-|
|APP_EMAIL_SENDGRIDKEY*|-|
|APP_EMAIL_SENDGRIDUSER*|-|
|APP_EMAIL_TOADDRESS*|-|
|APP_MAPBOX_TOKEN*|-|
|APP_RECAPTCHA_SECRETKEY*|-|
|APP_RECAPTCHA_SITEKEY*|-|
|APP_DATACONTEXT*|-|
|WEB_USERSECRETSID*|-|
|SQL_ADMIN_PASSWORD*|-|