using MongoDB.Bson;

namespace Main.Utils
{
    public interface IEncryptor
    {
        public string Encrypt(string? str);
        public string Encrypt(ObjectId? id);
        public string Decrypt(string str);
        public ObjectId DecryptObjectId(string str);
    }
}
