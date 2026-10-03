using Google.Cloud.Firestore;
using MessageHub.Models;

namespace MessageHub.Repositories;

public interface INotificationsRepository : IRepository<Notifications>
{
    /// <summary>Saves all notifications for an event in batched writes and returns them with their new ids.</summary>
    Task<IReadOnlyList<Notifications>> AddRangeAsync(IEnumerable<Notifications> notifications);

    /// <summary>Notifications still waiting to be sent, for the background worker.</summary>
    Task<IReadOnlyList<Notifications>> GetPendingAsync(int limit = 50);

    /// <summary>Delivery history of a single event.</summary>
    Task<IReadOnlyList<Notifications>> GetByEventIdAsync(string eventId);
}

public class NotificationsRepository : FirestoreRepository<Notifications>, INotificationsRepository
{
    // Firestore allows at most 500 writes in one batch.
    private const int MaxBatchSize = 500;

    public NotificationsRepository(FirestoreDb db) : base(db, "notifications") { }

    public async Task<IReadOnlyList<Notifications>> AddRangeAsync(IEnumerable<Notifications> notifications)
    {
        var saved = new List<Notifications>();

        foreach (var chunk in notifications.Chunk(MaxBatchSize))
        {
            WriteBatch batch = Db.StartBatch();

            foreach (var notification in chunk)
            {
                DocumentReference docRef = Collection.Document();
                notification.Id = docRef.Id;
                batch.Create(docRef, notification);
                saved.Add(notification);
            }

            await batch.CommitAsync();
        }

        return saved;
    }

    public async Task<IReadOnlyList<Notifications>> GetPendingAsync(int limit = 50)
    {
        // No OrderBy: combining it with this filter would need a composite index in Firestore.
        Query query = Collection
            .WhereEqualTo("status", nameof(NotificationStatus.Pending))
            .Limit(limit);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        return [.. snapshot.Documents.Select(d => d.ConvertTo<Notifications>())];
    }

    public async Task<IReadOnlyList<Notifications>> GetByEventIdAsync(string eventId)
    {
        QuerySnapshot snapshot = await Collection
            .WhereEqualTo("eventId", eventId)
            .GetSnapshotAsync();

        return [.. snapshot.Documents.Select(d => d.ConvertTo<Notifications>())];
    }
}
