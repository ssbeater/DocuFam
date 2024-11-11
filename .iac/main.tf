provider "google" {
  project = var.google_project_id
  region  = var.region
}

data "google_client_config" "default" {}

module "APIs" {
  source = "./modules/Apis"

  google_project_id = var.google_project_id
}

module "Registry" {
  source = "./modules/Registry"

  repository_id = var.registry_repo_id
  project_id    = var.google_project_id
  region        = var.region

  depends_on = [module.APIs]
}

module "CloudSQL" {
  source = "./modules/CloudSQL"

  region        = var.region
  isbn_password = var.isbn_db_pass

  depends_on = [module.APIs]
}

module "Firestore" {
  source = "./modules/Firestore"

  region = var.region

  depends_on = [module.APIs]
}

module "GKE" {
  source = "./modules/GKE"

  region        = var.region_gke
  network_id    = var.network_id
  subnetwork_id = var.subnetwork_id

  depends_on = [module.APIs, module.Registry]
}

resource "null_resource" "wait_for_cluster" {
  depends_on = [module.GKE]
}

provider "kubernetes" {
  host                   = module.GKE.endpoint
  token                  = data.google_client_config.default.access_token
  cluster_ca_certificate = base64decode(module.GKE.cluster_ca_certificate)
}

module "K8deploy" {
  source = "./modules/K8deploy"

  google_project_id = var.google_project_id
  network_id        = var.network_id
  isbn_db_pass      = var.isbn_db_pass

  docufam_db_instance_ip = module.CloudSQL.docufam_pp_db_instance_ip
  artifact_registry_url  = module.Registry.registry_url

  depends_on = [module.APIs, module.Registry, module.CloudSQL, module.GKE, null_resource.wait_for_cluster]
}
