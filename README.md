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
<<<<<<< HEAD
|APP_MAPBOX_TOKEN*|-|
=======
|APP_MAPBOX_TOKEN*|Map Token|
>>>>>>> upstream/master
|APP_RECAPTCHA_SECRETKEY*|-|
|APP_RECAPTCHA_SITEKEY*|-|
|APP_DATACONTEXT*|-|
|WEB_USERSECRETSID*|-|
<<<<<<< HEAD
|SQL_ADMIN_PASSWORD*|-|
=======
|SQL_ADMIN_PASSWORD*|-|
|CONTENTFUL_DELIVERY_ACCESS_TOKEN|Content Delivery API - access token|
|CONTENTFUL_SPACE_ID|Contentful Space ID|
|CONTENTFUL_ENVIRONMENT|Name of Contentful environment in use|

## Local development

### Technologies
- Docker
- Docker Compose
- .NET core 3.1.401
- SQL Server
- Azure Data Studio (Non-Windows environment only)
- GNU Make (For Windows user, you may install it using [Chocolatey](https://chocolatey.org/packages/make))

### Setup (Using GNU Make)
```sh
# Prepare local development settings
$ make setup

# Start servers in Docker
$ make dev

# For those who want to start development on website without Docker
#
# You can start depending services in docker
$ make -f Makefile.nodocker.mk docker-start-db
# And then run the dotnet process in local
$ NO_DOCKER=1 MODULE=Web make dev
```
After running above commands, visit http://localhost:3000 on browser and you should be able to see the web page.

### Setup (Using Visual Studio, Windows only)
TODO: Provide Powershell setup script
1. Copy `Web/appsettings.Development.json.template` to `Web/appsettings.Development.json`
2. Start `db` container specified in `docker-compose.dev.yml` by docker-compose command
>>>>>>> upstream/master
