using Google.Cloud.Firestore;

namespace documents_ms.Data.Firestore;

[FirestoreData]
public class FSComisaryCase
{
    [FirestoreProperty]
    public string? Id { get; set; }
    [FirestoreProperty]
    public string? Title { get; set; }
    [FirestoreProperty]
    public string? PatientId { get; set; }
    [FirestoreProperty]
    public string? OffenderId { get; set; }
    [FirestoreProperty]
    public List<string>? ProcessesList { get; set; }
    [FirestoreProperty]
    public DateTime DateCreated { get; set; }
    [FirestoreProperty]
    public DateTime DateUpdated { get; set; }
    [FirestoreProperty]
    public bool? OpenCase { get; set; }
}
