# modules/CloudSQL/main.tf
resource "google_sql_database_instance" "docufam_pp_db" {
  name             = "docufam-pp-db"
  database_version = "MYSQL_8_0"
  region           = var.region

  settings {
    tier = "db-custom-1-3840"

    backup_configuration {
      enabled = true
    }

    ip_configuration {
      ipv4_enabled = true
      authorized_networks {
        name  = "allow-all"
        value = "0.0.0.0/0"
      }
    }

    availability_type = "ZONAL" #
  }
  deletion_protection = false
}

resource "google_sql_database" "place_db" {
  name      = "place-db"
  instance  = google_sql_database_instance.docufam_pp_db.name
  charset   = "utf8"
  collation = "utf8_general_ci"
}

resource "google_sql_database" "people_db" {
  name      = "people-db"
  instance  = google_sql_database_instance.docufam_pp_db.name
  charset   = "utf8"
  collation = "utf8_general_ci"
}

resource "google_sql_user" "isbn_user" {
  name     = "isbn"
  instance = google_sql_database_instance.docufam_pp_db.name
  password = "123"
  host     = "%"
}
