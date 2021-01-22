resource "azurerm_sql_server" "sql_svr" {
  name                         = "${local.prefix}-sqlsrv-${terraform.workspace}"
  resource_group_name          = data.azurerm_resource_group.rg.name
  location                     = data.azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_login
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_mssql_database" "sql_db" {
  name           = "${local.prefix}-sqldb-${terraform.workspace}"
  server_id      = azurerm_sql_server.sql_svr.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 1
  read_scale     = false
  sku_name       = "Basic"
  zone_redundant = false

  tags = local.tags
}