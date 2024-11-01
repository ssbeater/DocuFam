using Google.Cloud.Firestore;

namespace documents_ms.Data.Firestore;

[FirestoreData]
public class FSProcess
{
    [FirestoreProperty]
    public string? Id { get; set; }
    [FirestoreProperty]
    public string? CaseId { get; set; }
    [FirestoreProperty]
    public string? type { get; set; }
    [FirestoreProperty]
    public List<ProcessPeople>? relatedPeople { get; set; }
    [FirestoreProperty]
    public List<ProcessPeople>? contributors { get; set; }
    [FirestoreProperty]
    public DateTime DateCreated { get; set; }
    [FirestoreProperty]
    public DateTime DateUpdated { get; set; }
    [FirestoreProperty]
    public bool? OpenProcess { get; set; }
}

[FirestoreData]
public class ProcessPeople {
    [FirestoreProperty]
    public string? Id { get; set; }
    [FirestoreProperty]
    public string? Role { get; set;}
}
