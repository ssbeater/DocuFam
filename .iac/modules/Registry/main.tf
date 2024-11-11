# modules/Registry/main.tf
resource "google_artifact_registry_repository" "docufam" {
  repository_id = var.repository_id
  location      = var.region
  format        = "DOCKER"
  mode          = "STANDARD_REPOSITORY"

  description = "Registro de contenedores Docker para el servicio DocuFam"
}

output "registry_url" {
  value       = "${var.region}-docker.pkg.dev/${var.project_id}/${var.repository_id}"
  description = "URL del registro Docker en Google Artifact Registry"
}
