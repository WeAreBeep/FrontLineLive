resource "azurerm_sql_server" "sql_svr" {
  name                         = "${local.prefix}-sqlsrv-${var.env}"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_login
  administrator_login_password = var.sql_admin_password
}

resource "azurerm_mssql_database" "sql_db" {
  name           = "${local.prefix}-sqldb-${var.env}"
  server_id      = azurerm_sql_server.sql_svr.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 4
  sku_name       = "GP_S_Gen5"
  zone_redundant = false
  read_scale = false
  auto_pause_delay_in_minutes = 60
  min_capacity = 0.5

  tags = local.tags
}