using FluentAssertions;
using MongoDB.Bson;
using Xunit;
using Main.Utils;

public class EncryptorTest
{
    private readonly Encryptor _sutEncryptor;

    public EncryptorTest()
    {
        _sutEncryptor = new Encryptor();
    }

    [Fact]
    public void Encryptor_Encrypt_ReturnEncryptedString()
    {
        // Arrange
        ObjectId id = ObjectId.GenerateNewId();
        
        // Act
        var result = _sutEncryptor.Encrypt(id);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeOfType<string>();
        result.Length.Should().BeGreaterThan(10);
    }
    
}