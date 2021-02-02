variable "environment_name" {
  type  = string
}

variable "container_registry_name" {
  type    = string
  default = "frontlineacr2"
}

variable "container_image_name" {
  type    = string
  default = "web"
}

variable "container_image_tag" {
  type    = string
}

variable "sql_admin_login" {
  type = string
  default = "frontline"
}

variable "sql_admin_password" {
  type = string
  sensitive = true
}