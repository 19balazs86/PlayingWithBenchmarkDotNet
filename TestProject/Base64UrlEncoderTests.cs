using PlayingWithBenchmarkDotNet.Code;
using System.Text;

namespace TestProject;

public class Base64UrlEncoderTests
{
    [Theory]
    [ClassData(typeof(EncodingData))]
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
    [ClassData(typeof(EncodingData))]
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
    [ClassData(typeof(EncodingData))]
    public void Encode_Decode_UrlFriendly_Simplest(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Simplest(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Simplest(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [ClassData(typeof(EncodingData))]
    public void Encode_Decode_UrlFriendly_Simple(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Simple(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Simple(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }

    [Theory]
    [ClassData(typeof(EncodingData))]
    // [MemberData(nameof(ClassForTheoryData.TestField1), MemberType = typeof(ClassForTheoryData))]
    public void Encode_Decode_UrlFriendly_Complex(string input)
    {
        // Act
        string encodedText = Base64UrlEncoder.UrlFriendlyEncode_Complex(input);

        string decodedText = Base64UrlEncoder.UrlFriendlyDecode_Complex(encodedText);

        // Assert
        Assert.Equal(input, decodedText);
    }
}

// Simplifying test data with Xunit.Combinatorial (I just keep this link here)
// https://andrewlock.net/simplifying-theory-test-data-with-xunit-combinatorial

// https://www.milanjovanovic.tech/blog/creating-data-driven-tests-with-xunit
file sealed class EncodingData : TheoryData<string>
{
    public EncodingData()
    {
        Add("=");
        Add("==");
        Add("===");
        Add("====");
        Add("");
        Add("1");
        Add("12");
        Add("123");
        Add("1234");
        Add("12345");
        Add("123456");
        Add("1234567");
        Add("12345678");
        Add("123456789");
        Add("1234567890");
        Add("\"\\~ˇ^˘°˛`˙´˝¨¸×÷¤ß$Łł*>;}{@&#><");
        Add("§'+!%/=()?:_");
        Add("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        Add("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");
    }
};

file sealed class ClassForTheoryData
{
    // You can use it with: [MemberData(nameof(ClassForTheoryData.TestField1), MemberType = typeof(ClassForTheoryData))]
    // MemberType: only needed if the field is not in the same class

    public static TheoryData<string> TestField1 { get; } = new TheoryData<string>
    {
        "=",
        "==",
        "==="
    };

    public static TheoryData<EncodeInputs> TestField2 { get; } = new TheoryData<EncodeInputs>
    {
        new EncodeInputs("input1", 1),
        new EncodeInputs("input2", 2),
        new EncodeInputs("input3", 3)
    };
};

file sealed record EncodeInputs(string Input, int Number);