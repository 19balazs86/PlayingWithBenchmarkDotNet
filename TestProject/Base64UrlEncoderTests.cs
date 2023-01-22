using PlayingWithBenchmarkDotNet.Code;
using System.Text;

namespace TestProject;

public class Base64UrlEncoderTests
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void Encode_Decode_Simple(string input)
    {
        // Arrange
        string expectedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        // Act
        string encodedText = Base64UrlEncoder.Encode_Simple(input);

        // Assert
        Assert.Equal(expectedText, encodedText);

        // Act
        string decodedText = Base64UrlEncoder.Decode_Simple(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Encode_Decode_Complex(string input)
    {
        // Arrange
        string expectedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        // Act
        string encodedText = Base64UrlEncoder.Encode_Complex(input);

        // Assert
        Assert.Equal(expectedText, encodedText);


        // Act
        string decodedText = Base64UrlEncoder.Decode_Complex(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Encode_Decode_UrlFriendly_Simplest(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Simplest(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Simplest(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Encode_Decode_UrlFriendly_Simple(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Simple(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Simple(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void Encode_Decode_UrlFriendly_Complex(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Complex(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Complex(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    public static IEnumerable<object[]> GetData()
    {
        return new List<object[]>
        {
            new object[] { "=" },
            new object[] { "==" },
            new object[] { "===" },
            new object[] { "====" },
            new object[] { "" },
            new object[] { "1" },
            new object[] { "12" },
            new object[] { "123" },
            new object[] { "1234" },
            new object[] { "12345" },
            new object[] { "123456" },
            new object[] { "1234567" },
            new object[] { "12345678" },
            new object[] { "123456789" },
            new object[] { "1234567890" },
            new object[] { "\"\\~ˇ^˘°˛`˙´˝¨¸×÷¤ß$Łł*>;}{@&#><" },
            new object[] { "§'+!%/=()?:_" },
            new object[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" },
            new object[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" }
        };
    }
}