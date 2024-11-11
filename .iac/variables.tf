variable "region" {
    type = string
    default = "region"
}

variable "region_gke" {
    type = string
    default = "region"
}


variable "google_project_id" {
    type = string
}

variable "registry_repo_id" {
    type = string
    default = "docufam_reg"
}

variable "network_id" {
  type = string
}

variable "subnetwork_id" {
  type = string
}
