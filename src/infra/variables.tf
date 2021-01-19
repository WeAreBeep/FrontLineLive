variable "env" {
  type    = string
  default = "dev"
}

variable "container_registry_name" {
  type    = string
  default = "frontlineacr"
}

variable "container_image_name" {
  type    = string
  default = "web"
}

variable "container_image_tag" {
  type    = string
}