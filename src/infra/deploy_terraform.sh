#!/usr/bin/env bash

echo "Starting terraform deployment..."
echo "Key variables:"
echo "Workspace: $DEPLOY_WORKSPACE"
echo "Container Image Tag: $GITHUB_RUN_NUMBER"

terraform init
terraform workspace select $DEPLOY_WORKSPACE

terraform plan \
    -var "container_image_tag=$GITHUB_RUN_NUMBER"

terraform apply -auto-approve \
    -var "container_image_tag=$GITHUB_RUN_NUMBER"