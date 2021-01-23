#!/usr/bin/env bash

set -e

echo "Writing GitHub secrets to app-settings.json..."

jq -n '$ARGS.named' \
    --arg APP_DATA_SUPPLIERS_SHEET "$APP_DATA_SUPPLIERS_SHEET" \
    --arg APP_EMAIL_FROMADDRESS "$APP_EMAIL_FROMADDRESS" \
    --arg APP_EMAIL_SENDGRIDKEY "$APP_EMAIL_SENDGRIDKEY" \
    --arg APP_EMAIL_SENDGRIDUSER "$APP_EMAIL_SENDGRIDUSER" \
    --arg APP_EMAIL_TOADDRESS "$APP_EMAIL_TOADDRESS" \
    --arg APP_MAPBOX_TOKEN "$APP_MAPBOX_TOKEN" \
    --arg APP_RECAPTCHA_SECRETKEY "$APP_RECAPTCHA_SECRETKEY" \
    --arg APP_RECAPTCHA_SITEKEY "$APP_RECAPTCHA_SITEKEY" \
    --arg APP_DATACONTEXT "$APP_DATACONTEXT" \
    --arg WEB_USERSECRETSID "$WEB_USERSECRETSID" > ./app-settings.json

echo "Initializing Terraform..."

terraform init

echo "Selecting Terraform Workspace ($DEPLOY_WORKSPACE)..."

terraform workspace select $DEPLOY_WORKSPACE

echo "Planning Terraform Deployment..."

terraform plan \
    -var "container_image_tag=$GITHUB_RUN_NUMBER"

echo "Applying Terraform Deployment..."

terraform apply -auto-approve \
    -var "container_image_tag=$GITHUB_RUN_NUMBER"