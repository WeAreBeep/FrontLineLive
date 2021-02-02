terraform {
  backend "azurerm" {
    resource_group_name  = "frontline-rg-baseline"
    storage_account_name = "frontlinesa2"
    container_name       = "tf-state"
    key                  = "frontline.tfstate"
  }
}

provider "azurerm" {
  features {}
}

locals {
  prefix = "frontline"
  
  # if you update this - please update the Terraform backend too (see line #3).
  baseline_resource_group = "frontline-rg-baseline"

  container_image = "${data.azurerm_container_registry.acr.login_server}/${var.container_image_name}:${var.container_image_tag}"

  tags = {
    owner = "terraform"
    site = "frontline.live"
  }
}

data "azurerm_resource_group" "rg" {
  name     = "${local.prefix}-rg-${terraform.workspace}"
}

data "azurerm_container_registry" "acr" {
  name                = var.container_registry_name
  resource_group_name = local.baseline_resource_group
}

resource "azurerm_storage_account" "sa" {
  name                     = "${local.prefix}sa${terraform.workspace}"
  resource_group_name      = data.azurerm_resource_group.rg.name
  location                 = data.azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}