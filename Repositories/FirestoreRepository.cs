using Google.Cloud.Firestore;
using Grpc.Core;
using MessageHub.Models;

namespace MessageHub.Repositories;

public abstract class FirestoreRepository<T> : IRepository<T> where T: class, IFirestoreEntity
{
    protected readonly FirestoreDb Db;
    protected readonly CollectionReference Collection;

    protected FirestoreRepository(FirestoreDb db, string collectionName)
    {
        Db = db;
        Collection = db.Collection(collectionName);
    }

    public virtual async Task<T?> GetByIdAsync(string Id)
    {
        DocumentSnapshot snapshot = await Collection.Document(Id).GetSnapshotAsync();
        return snapshot.Exists ? snapshot.ConvertTo<T>() : null;
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(int limit=100)
    {
        QuerySnapshot snapshot = await Collection.Limit(limit).GetSnapshotAsync();
        return snapshot.Documents.Select(d => d.ConvertTo<T>()).ToList();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        DocumentReference docRef = await Collection.AddAsync(entity);

        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        return snapshot.ConvertTo<T>();
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        DocumentReference docRef = Collection.Document(entity.Id);

        return await Db.RunTransactionAsync( async tx =>
        {
            DocumentSnapshot snap = await tx.GetSnapshotAsync(docRef);
            if (!snap.Exists) return false;

            tx.Set(docRef, entity);
            return true;
        });
    }

    public virtual async Task<bool> DeleteAsync(string Id)
    {
        try
        {
            await Collection.Document(Id).DeleteAsync(Precondition.MustExist);
            return true;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return false;
        }
    }

}

