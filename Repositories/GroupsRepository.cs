using Google.Cloud.Firestore;
using MessageHub.Models;

namespace MessageHub.Repositories;

public interface IGroupsRepository : IRepository<Groups>
{
}

public class GroupsRepository : FirestoreRepository<Groups>, IGroupsRepository
{
    public GroupsRepository(FirestoreDb db) : base(db, "groups") { }
}