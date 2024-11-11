# modules/Firestore/main.tf
resource "google_firestore_database" "database" {
  name        = "docufam-docs-db"  # El nombre de la base de datos para Firestore siempre es "(default)"
  location_id = var.region         # La ubicación de la base de datos en la región us-east4
  type        = "FIRESTORE_NATIVE" # Configuración para que la base de datos opere en modo Native (orientado a documentos, sin restricciones de particiones)
}
