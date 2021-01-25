# FrontLineLive

Front Line Live code and target operating model

- Description of the platorm
- Who built it
- Why it's important
- How to install your own instance

## GitHub Secrets

The following secrets should be configured for each Environment:

### Deployment secrets

|Secret Name|Description|
|-|-|
|ARM_SUBSCRIPTION_ID|The Azure subscription ID you wish the use.|
|ARM_ACCESS_KEY|Azure Storage Account used for TF remote state. See [here](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage).|
|AZURE_CREDENTIALS|See [here](https://github.com/marketplace/actions/azure-login#configure-deployment-credentials).|
|ACR_USERNAME|Username for ACR's [Admin Account](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication#admin-account).|
|ACR_PASSWORD|Password for ACR's [Admin Account](https://docs.microsoft.com/en-us/azure/container-registry/container-registry-authentication#admin-account).|
|ACR_SERVER|The login server for ACR|

### Application secrets

|Secret Name|Description|
|-|-|
|APP_DATA_SUPPLIERS_SHEET|-|
|APP_EMAIL_FROMADDRESS|-|
|APP_EMAIL_SENDGRIDKEY|-|
|APP_EMAIL_SENDGRIDUSER|-|
|APP_EMAIL_TOADDRESS|-|
|APP_MAPBOX_TOKEN|-|
|APP_RECAPTCHA_SECRETKEY|-|
|APP_RECAPTCHA_SITEKEY|-|
|APP_DATACONTEXT|-|
|WEB_USERSECRETSID|-|