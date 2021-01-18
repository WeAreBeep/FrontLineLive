terraform {
  backend "azurerm" {
    resource_group_name  = "lm-devops-rg"
    storage_account_name = "lmdevopsstor"
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

  env_variables = {
    DOCKER_REGISTRY_SERVER_URL      = "https://${data.azurerm_container_registry.acr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME = data.azurerm_container_registry.acr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD = data.azurerm_container_registry.acr.admin_password
  }

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

resource "azurerm_app_service_plan" "asp" {
  name                = "${local.prefix}-asp-${var.env}"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  kind                = "Linux"
  reserved            = true
  tags                = local.tags

  sku {
    tier     = "Standard"
    size     = "S1"
    capacity = "2"
  }
}

resource "azurerm_app_service" "web" {
  name                = "${local.prefix}-web-${var.env}"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id
  tags                = local.tags

  site_config {
    always_on        = "true"
    linux_fx_version = "DOCKER|${local.container_image}"
  }

  app_settings = merge(local.env_variables, {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.insights.instrumentation_key
  })
}

resource "azurerm_app_service_slot" "slot" {
  name                = "${local.prefix}-web-${var.env}-slot"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id
  app_service_name    = azurerm_app_service.web.name
  tags                = local.tags

  site_config {
    always_on = "true"
  }

  app_settings = local.env_variables
}

resource "azurerm_application_insights" "insights" {
  name                = "${local.prefix}-appi-${var.env}"
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  application_type    = "web"
  tags                = local.tags
}
