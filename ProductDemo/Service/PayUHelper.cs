using System.Security.Cryptography;
using System.Text;

public static class PayUHelper
{
    public static string GenerateHash(string text)
    {
        using (var sha512 = SHA512.Create())
        {
            var hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(text));
            return ByteArrayToHexString(hashBytes);
        }
    }

    private static string ByteArrayToHexString(byte[] byteArray)
    {
        return BitConverter.ToString(byteArray).Replace("-", "").ToLower();
    }
}
