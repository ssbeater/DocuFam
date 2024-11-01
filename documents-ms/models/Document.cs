using Google.Cloud.Firestore;

namespace documents_ms.models;

[FirestoreData]
public class Document
{

    [FirestoreProperty]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? PatientId { get; set; }

    [FirestoreProperty]
    public string? DocumentType { get; set; }

    [FirestoreProperty]
    public string? Content { get; set; }

    [FirestoreProperty]
    public DateTime DateCreated { get; set; }
}
