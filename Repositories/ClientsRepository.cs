using Google.Cloud.Firestore;
using MessageHub.Models;

namespace MessageHub.Repositories;

public interface IClientsRepository : IRepository<Clients>
{
    Task<IReadOnlyList<Clients>> GetByNameAsync(string name);
    
}

public class ClientsRepository : FirestoreRepository<Clients>, IClientsRepository
{
    public ClientsRepository(FirestoreDb db) : base(db, "clients") { }

    public async Task<IReadOnlyList<Clients>> GetByNameAsync(string name)
    {
        Query query = Collection
            .WhereEqualTo("name", name)
            .OrderBy("name");
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        
        return [.. snapshot.Documents.Select(d => d.ConvertTo<Clients>())];
    }
}