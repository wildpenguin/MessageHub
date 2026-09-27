using Google.Cloud.Firestore;

namespace MessageHub.Models;


[FirestoreData]
public class Groups : IFirestoreEntity
{
    [FirestoreProperty]
    public string Id {get; set; } = string.Empty;
    
    [FirestoreProperty("groupName")]
    public string? GroupName { get; set; }
    
    [FirestoreProperty("groupType")]
    public string? GroupType { get; set; }
    
    [FirestoreProperty("groupColorCode")]
    public string? GroupColorCode { get; set; }

    [FirestoreProperty("members")]
    public List<string>? Members { get; set; }
}