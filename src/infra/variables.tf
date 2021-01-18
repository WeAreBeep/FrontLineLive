variable "env" {
  type    = string
  default = "dev"
}

variable "container_registry" {
  type    = string
}

variable "container_registry_username" {
  type    = string
}

variable "container_registry_password" {
  type    = string
  sensitive = true
}

variable "container_image_name" {
  type    = string
  default = "web"
}

variable "container_image_tag" {
  type    = string
}