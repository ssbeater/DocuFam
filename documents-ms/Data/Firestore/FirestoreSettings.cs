namespace documents_ms.Data.Firestore;

public class FirestoreSettings
{
    public bool UseEmulator { get; set; }
    public string? EmulatorHost { get; set; }
    public string? CredentialPath { get; set; }
    public string? ProjectId { get; set; }
    public string? DatabaseId { get; set; }
}
