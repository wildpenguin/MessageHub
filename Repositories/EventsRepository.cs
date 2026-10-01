using Google.Cloud.Firestore;
using MessageHub.Models;

namespace MessageHub.Repositories;

public interface IEventsRepository: IRepository<Events>
{
    
}

public class EventsRepository : FirestoreRepository<Events>, IEventsRepository
{
    public EventsRepository(FirestoreDb db) : base(db, "events") { }
   
}