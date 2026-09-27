using Google.Cloud.Firestore;

namespace MessageHub.Models;

[FirestoreData]
public class Clients: IFirestoreEntity
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("name")]
    public string? Name { get; set; }

    [FirestoreProperty("email")]
    public string? Email { get; set; }

    [FirestoreProperty("phone")]
    public string? Phone { get; set; }
}