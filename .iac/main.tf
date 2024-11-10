provider "google" {
  project =  var.google_project_id
  region  = "us-east4"
}

module "APIs" {
    source = "./modules/Apis"
    google_project_id = var.google_project_id
}

module "Registry" {
    source = "./modules/Registry"
    repository_id = var.registry_repo_id

    depends_on = [ module.APIs ]
}

module "CloudSQL" {
    source = "./modules/CloudSQL"
    region = var.region

    depends_on = [ module.APIs ]
}

module "Firestore" {
    source = "./modules/Firestore"
    region = var.region

    depends_on = [ module.APIs ]
}
