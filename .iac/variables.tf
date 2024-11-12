variable "region" {
  type    = string
  default = "region"
}

variable "region_gke" {
  type    = string
  default = "region"
}


variable "google_project_id" {
  type = string
}

variable "registry_repo_id" {
  type    = string
  default = "docufam_reg"
}

variable "isbn_db_pass" {
  type = string
}

variable "network_id" {
  type = string
}

variable "subnetwork_id" {
  type = string
}

variable "api_gw_url" {
  type = string
  default = "http://localhost:6010/gateway"
}
