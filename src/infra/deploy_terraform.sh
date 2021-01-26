#!/usr/bin/env bash

set -e

echo "Writing GitHub secrets to app-settings.json..."



jq -n '$ARGS.named' \
    --arg APP_DATA_SUPPLIERS_SHEET "$APP_DATA_SUPPLIERS_SHEET" \
    --arg Emails__FromAddress "$APP_EMAIL_FROMADDRESS" \
    --arg Emails__SendGridKey "$APP_EMAIL_SENDGRIDKEY" \
    --arg Emails__ToAddresses "$APP_EMAIL_TOADDRESS" \
    --arg AuthMessageSender__SendGridUser "$APP_EMAIL_SENDGRIDUSER" \
    --arg AuthMessageSender__SendGridKey "$APP_EMAIL_SENDGRIDKEY" \
    --arg APP_MAPBOX_TOKEN "$APP_MAPBOX_TOKEN" \
    --arg ReCaptcha__SecretKey "$APP_RECAPTCHA_SECRETKEY" \
    --arg ReCaptcha__SiteKey "$APP_RECAPTCHA_SITEKEY" \
    --arg APP_DATACONTEXT "$APP_DATACONTEXT" \
    --arg WEB_USERSECRETSID "$WEB_USERSECRETSID" > ./app-settings.json

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