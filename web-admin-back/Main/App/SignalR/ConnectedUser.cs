using MongoDB.Bson;

namespace Main.App.SignalR
{
    internal class ConnectedUser
    {
        public string? ConnectionId { get; set; }
        public ObjectId IdUser { get; set; }

        public ConnectedUser(string connectionId, ObjectId idUser)
        {
            ConnectionId = connectionId;
            IdUser = idUser;
        }
    }
}