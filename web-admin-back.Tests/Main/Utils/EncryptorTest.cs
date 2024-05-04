using FluentAssertions;
using MongoDB.Bson;
using Main.Utils;

public class EncryptorTest
{
    private readonly Encryptor _sutEncryptor;

    public EncryptorTest()
    {
        _sutEncryptor = new Encryptor();
    }

    [Fact]
    public void Encryptor_Encrypt_WithObjectId_ReturnEncryptedString()
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

    [Fact]
    public void Encryptor_Encrypt_WithString_ReturnEncryptedString()
    {
        // Arrange
        string str = "asdfqwe1234!@#$!@#!@$%%&%$#{}?";
        
        // Act
        var result = _sutEncryptor.Encrypt(str);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeOfType<string>();
        result.Length.Should().BeGreaterThan(10);
    }

    [Fact]
    public void Encryptor_Decrypt_ReturnString()
    {
        // Arrange
        string str = "asdfqwe1234!@#$!@#!@$%%&%$#{}?";
        string encrypted = _sutEncryptor.Encrypt(str);
        
        // Act
        var result = _sutEncryptor.Decrypt(encrypted);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeOfType<string>();
        result.Length.Should().BeGreaterThan(10);
        result.Should().Be(str);
    }

    [Fact]
    public void Encryptor_DecryptObjectId_ReturnObjectId()
    {
        // Arrange
        string id = ObjectId.GenerateNewId().ToString();
        string encrypted = _sutEncryptor.Encrypt(id);
        
        // Act
        var result = _sutEncryptor.DecryptObjectId(encrypted);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ObjectId>();
        result.Should().Be(ObjectId.Parse(id));
        
    }



}