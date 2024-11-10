# modules/Registry/main.tf
resource "google_artifact_registry_repository" "docufam" {
  repository_id = var.repository_id
  format        = "DOCKER"
  location      = "us-east4"
  mode          = "STANDARD_REPOSITORY"
  description   = "Registro de contenedores Docker para el servicio DocuFam"
}
