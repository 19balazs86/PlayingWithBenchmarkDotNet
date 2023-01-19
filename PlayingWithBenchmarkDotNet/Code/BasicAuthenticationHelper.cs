using System.Text;

namespace PlayingWithBenchmarkDotNet.Code;

public static class BasicAuthenticationHelper
{
    public const string ValidUsername = "MyVeryLongValidUsernameCanGoesOn";
    public const string ValidPassword = "MyVeryLongValidPasswordCanGoesOn";

    private const string _basic = "Basic";

    public static readonly string TestAuthHeaderInput;

    static BasicAuthenticationHelper()
    {
        string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ValidUsername}:{ValidPassword}"));

        TestAuthHeaderInput = $"{_basic} {token}";
    }

    public static bool CheckBasicAuthorizationHeader(string authHeader)
    {
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith(_basic))
            return false;

        string token = authHeader[6..].Trim();

        string credentialsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        string[] credentials = credentialsString.Split(':');

        return credentials[0] == ValidUsername && credentials[1] == ValidPassword;
    }

    public static bool CheckBasicAuthorizationHeaderWithSpan(ReadOnlySpan<char> authHeader)
    {
        if (authHeader is not { Length: > 6 })
            return false;

        if (!authHeader[..5].Equals(_basic, StringComparison.InvariantCulture))
            return false;

        // No allocation
        ReadOnlySpan<char> token = authHeader[6..].Trim();

        // Prepare resulting buffer that receives the data.
        // In Base64 every char encodes 6 bits, so 4 chars = 3 bytes
        Span<byte> buffer = stackalloc byte[(token.Length * 3 + 3) / 4];

        if (!Convert.TryFromBase64Chars(token, buffer, out int bytesWritten))
            return false;

        Span<char> credentials = stackalloc char[bytesWritten];

        // No allocation
        Encoding.UTF8.GetChars(buffer[..bytesWritten], credentials);

        int positionOfColon = credentials.IndexOf(':');

        if (positionOfColon == -1)
            return false;

        Span<char> username = credentials[..positionOfColon];
        Span<char> password = credentials[(positionOfColon + 1)..];

        return MemoryExtensions.Equals(username, ValidUsername, StringComparison.InvariantCulture) &&
               MemoryExtensions.Equals(password, ValidPassword, StringComparison.InvariantCulture);
    }
}
