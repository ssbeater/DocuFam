resource "kubernetes_namespace" "app_ns" {
  metadata {
    name = "docufam"
  }
}

resource "kubernetes_service" "api_gateway" {
  metadata {
    name      = "api-gateway"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    type = "LoadBalancer"

    selector = {
      app = "api-gateway"
    }

    port {
      port        = 6010
      target_port = 6010
    }
  }
}

resource "kubernetes_deployment" "people-ms" {
  metadata {
    name      = "people-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "people-ms"
      }
    }
    template {
      metadata {
        labels = {
          app = "people-ms"
        }
      }
      spec {
        container {
          name  = "people-ms"
          image = "us-east4-docker.pkg.dev/cbse-2024ii-438402/docufam-reg/people-ms:latest"
          env {
            name  = "ConnectionStrings__DefaultConnection"
            value = "Server=34.150.141.218;Port=3306;Database=people-db;User=isbn;Password=123;"
          }
          port {
            container_port = 5010
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "people_ms_svc" {
  metadata {
    name      = "people-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    selector = {
      app = "people-ms"
    }
    port {
      port        = 5010
      target_port = 5010
    }
  }
}

resource "kubernetes_deployment" "place-ms" {
  metadata {
    name      = "place-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "place-ms"
      }
    }
    template {
      metadata {
        labels = {
          app = "place-ms"
        }
      }
      spec {
        container {
          name  = "place-ms"
          image = "us-east4-docker.pkg.dev/cbse-2024ii-438402/docufam-reg/place-ms:latest"
          env {
            name  = "ConnectionStrings__DefaultConnection"
            value = "Server=34.150.141.218;Port=3306;Database=place-db;User=isbn;Password=123;"
          }
          port {
            container_port = 5020
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "place_ms_svc" {
  metadata {
    name      = "place-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    selector = {
      app = "place-ms"
    }
    port {
      port        = 5020
      target_port = 5020
    }
  }
}

resource "kubernetes_deployment" "documents-ms" {
  metadata {
    name      = "documents-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "documents-ms"
      }
    }
    template {
      metadata {
        labels = {
          app = "documents-ms"
        }
      }
      spec {
        container {
          name  = "documents-ms"
          image = "us-east4-docker.pkg.dev/cbse-2024ii-438402/docufam-reg/documents-ms:latest"
          env {
            name  = "GOOGLE_APPLICATION_CREDENTIALS"
            value = "/app/Secrets/firestore-admin.json"
          }
          env {
            name  = "FirestoreSettings__CredentialsPath"
            value = "/app/Secrets/firestore-admin.json" # GCP Credentials Path
          }
          env {
            name  = "FirestoreSettings__UseEmulator"
            value = "false"
          }
          env {
            name  = "FirestoreSettings__EmulatorHost"
            value = "localhost:6001"
          }
          env {
            name  = "FirestoreSettings__ProjectId"
            value = "cbse-2024ii-438402"
          }
          env {
            name  = "FirestoreSettings__DatabaseId"
            value = "docufam-docs-db"
          }
          env {
            name  = "ApiSetting__PeopleBaseUrl"
            value = "http://people-ms.docufam.svc.cluster.local/api"
          }
          port {
            container_port = 5030
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "documents_ms_svc" {
  metadata {
    name      = "documents-ms"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    selector = {
      app = "documents-ms"
    }
    port {
      port        = 5030
      target_port = 5030
    }
  }
}

resource "kubernetes_deployment" "api_gateway" {
  metadata {
    name      = "api-gateway"
    namespace = kubernetes_namespace.app_ns.metadata[0].name
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "api-gateway"
      }
    }
    template {
      metadata {
        labels = {
          app = "api-gateway"
        }
      }
      spec {
        container {
          name  = "api-gateway"
          image = "us-east4-docker.pkg.dev/cbse-2024ii-438402/docufam-reg/api-gw:latest"
          env {
            name  = "PEOPLE_MS_HOST"
            value = "http://people-ms.docufam.svc.cluster.local"
          }
          env {
            name  = "PEOPLE_MS_PORT"
            value = "80"
          }
          env {
            name  = "PLACE_MS_HOST"
            value = "http://place-ms.docufam.svc.cluster.local"
          }
          env {
            name  = "PLACE_MS_PORT"
            value = "80"
          }
          env {
            name  = "DOCS_MS_HOST"
            value = "http://documents-ms.docufam.svc.cluster.local"
          }
          env {
            name  = "DOCS_MS_PORT"
            value = "80"
          }
          port {
            container_port = 6010
          }
        }
      }
    }
  }
}

resource "google_compute_firewall" "allow_api_gateway" {
  name    = "allow-api-gateway"
  network = var.network_id

  allow {
    protocol = "tcp"
    ports    = ["6010"]
  }

  source_ranges = ["0.0.0.0/0"]
}
