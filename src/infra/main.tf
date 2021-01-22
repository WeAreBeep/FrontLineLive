terraform {
  backend "azurerm" {
    resource_group_name  = "frontline-rg-dev"
    storage_account_name = "frontlinedevops"
    container_name       = "tf-state"
    key                  = "frontline.tfstate"
  }
}

provider "azurerm" {
  version = "~>2.0"
  features {}
}

locals {
  prefix = "frontline"

  container_image = "${data.azurerm_container_registry.acr.login_server}/${var.container_image_name}:${var.container_image_tag}"

  tags = {
    owner = "terraform"
    site = "Frontline Live"
  }
}

data "azurerm_resource_group" "rg" {
  name     = "${local.prefix}-rg-${var.env}"
}

data "azurerm_container_registry" "acr" {
  name                = var.container_registry_name
  resource_group_name = data.azurerm_resource_group.rg.name
}

resource "azurerm_storage_account" "sa" {
  name                     = "${local.prefix}sa${var.env}"
  resource_group_name      = data.azurerm_resource_group.rg.name
  location                 = data.azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}