using Google.Cloud.Firestore;

namespace MessageHub.Models;

[FirestoreData]
public class Events: IFirestoreEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("eventType")]
    public string EventType { get; set; } = string.Empty;
    
    [FirestoreProperty("eventTitle")]
    public string EventTitle { get; set; } = string.Empty;

    [FirestoreProperty("eventText")]
    public string EventText { get; set; } = string.Empty;

    [FirestoreProperty("groups")]
    public List<string> Groups { get; set; } = new();

    [FirestoreDocumentCreateTimestamp]
    public Timestamp CreatedAt { get; set; }
}