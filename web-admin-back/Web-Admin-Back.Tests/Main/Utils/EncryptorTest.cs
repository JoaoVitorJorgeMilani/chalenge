// using FluentAssertions;
// using Main.Utils;
// using MongoDB.Bson;
// using Xunit;

// public class EncryptorTest
// {
//     private readonly Encryptor _sutEncryptor;

//     public EncryptorTest()
//     {
//         _sutEncryptor = new Encryptor();
//     }

//     [Fact]
//     public void Encryptor_Encrypt_ReturnEncryptedString()
//     {
//         #region Arrange
//         ObjectId id = ObjectId.GenerateNewId();

//         #endregion

//         #region Act
//         var result = _sutEncryptor.Encrypt(id);

//         #endregion

//         #region Assert
//         result.Should().NotBeNullOrEmpty();
//         result.Should().BeOfType<string>();
//         result.Length.Should().BeGreaterThan(10);
        
//         #endregion

//     }
    
// }