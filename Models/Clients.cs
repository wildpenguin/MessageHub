using Google.Cloud.Firestore;

namespace MessageHub.Models;

[FirestoreData]
public class Clients: IFirestoreEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("name")]
    public string Name { get; set; } = string.Empty;

    [FirestoreProperty("email")]
    public string? Email { get; set; }

    [FirestoreProperty("phone")]
    public string? Phone { get; set; }
}