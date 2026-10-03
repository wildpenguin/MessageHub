using Google.Cloud.Firestore;

namespace MessageHub.Models;

public enum NotificationChannel
{
    Email,
    Sms
}

public enum NotificationStatus
{
    Pending,
    Sent,
    Failed
}

/// <summary>
/// One message to one recipient over one channel, created when an event is published.
/// A background worker picks up Pending notifications, sends them and records the outcome.
/// See "Notifications" in README.md for the full flow.
/// </summary>
[FirestoreData]
public class Notifications : IFirestoreEntity
{
    [FirestoreDocumentId]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty("eventId")]
    public string EventId { get; set; } = string.Empty;

    [FirestoreProperty("clientId")]
    public string ClientId { get; set; } = string.Empty;

    [FirestoreProperty("channel", ConverterType = typeof(FirestoreEnumNameConverter<NotificationChannel>))]
    public NotificationChannel Channel { get; set; }

    // Email address or phone number, copied from the client when the notification is
    // created so later edits to the client don't change who an old message went to.
    [FirestoreProperty("recipient")]
    public string Recipient { get; set; } = string.Empty;

    [FirestoreProperty("subject")]
    public string Subject { get; set; } = string.Empty;

    [FirestoreProperty("body")]
    public string Body { get; set; } = string.Empty;

    [FirestoreProperty("status", ConverterType = typeof(FirestoreEnumNameConverter<NotificationStatus>))]
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

    [FirestoreProperty("attempts")]
    public int Attempts { get; set; }

    [FirestoreProperty("lastError")]
    public string? LastError { get; set; }

    [FirestoreProperty("sentAt")]
    public Timestamp? SentAt { get; set; }

    [FirestoreDocumentCreateTimestamp]
    public Timestamp CreatedAt { get; set; }
}
