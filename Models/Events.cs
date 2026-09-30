using Google.Cloud.Firestore;

namespace MessageHub.Models;

[FirestoreData]
public class Events: IFirestoreEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("eventType")]
    public string? EventType { get; set; } 
    
    [FirestoreProperty("eventTitle")]
    public string? EventTitle { get; set; }

    [FirestoreProperty("eventText")]
    public string? EventText { get; set; }

    [FirestoreDocumentCreateTimestamp]
    public Timestamp CreatedAt { get; set; }
}