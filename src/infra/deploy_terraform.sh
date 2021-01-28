#!/usr/bin/env bash

set -e

echo "Writing GitHub secrets to app-settings.json..."

echo $ARM_ACCESS_KEY;
echo "length is ${#ARM_ACCESS_KEY}"

echo "{
    \"APP_DATA_SUPPLIERS_SHEET\": \"$APP_DATA_SUPPLIERS_SHEET\",
    \"Emails__FromAddress\": \"$APP_EMAIL_FROMADDRESS\",
    \"Emails__SendGridKey\": \"$APP_EMAIL_SENDGRIDKEY\",
    \"Emails__ToAddresses\": \"$APP_EMAIL_TOADDRESS\",
    \"AuthMessageSender__SendGridUser\": \"$APP_EMAIL_SENDGRIDUSER\",
    \"AuthMessageSender__SendGridKey\": \"$APP_EMAIL_SENDGRIDKEY\",
    \"APP_MAPBOX_TOKEN\": \"$APP_MAPBOX_TOKEN\",
    \"ReCaptcha__SecretKey\": \"$APP_RECAPTCHA_SECRETKEY\",
    \"ReCaptcha__SiteKey\": \"$APP_RECAPTCHA_SITEKEY\",
    \"APP_DATACONTEXT\": \"$APP_DATACONTEXT\",
    \"WEB_USERSECRETSID\": \"$WEB_USERSECRETSID\"
}" > ./app-settings.json

echo "Initializing Terraform..."

terraform init

echo "Selecting Terraform Workspace ($DEPLOY_WORKSPACE)..."

terraform workspace select $DEPLOY_WORKSPACE

echo "Planning Terraform Deployment..."

terraform plan \
    -var "container_image_tag=$GITHUB_RUN_NUMBER" \
    -var "environment_name=$ENVIRONMENT_NAME"

echo "Applying Terraform Deployment..."

terraform apply -auto-approve \
    -var "container_image_tag=$GITHUB_RUN_NUMBER" \
    -var "environment_name=$ENVIRONMENT_NAME"