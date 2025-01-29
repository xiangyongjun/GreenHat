using System;
using System.Security.Cryptography;

namespace GreenHat.Utils
{
    public class TOTP
    {
        private readonly byte[] _secretKey;
        private readonly int _digits;
        private readonly int _interval;
        private readonly string _name;
        private readonly string _issuer;

        public TOTP(string secretKey, int digits = 6, int interval = 30, string name = null, string issuer = null)
        {
            _secretKey = Base32Decode(secretKey);
            _digits = digits;
            _interval = interval;
            _name = name;
            _issuer = issuer;
        }

        public string Now()
        {
            return GenerateOTP(GetTimeCode(DateTime.UtcNow));
        }

        public bool Verify(string otp, DateTime? forTime = null, int validWindow = 0)
        {
            DateTime time = forTime ?? DateTime.UtcNow;

            if (validWindow > 0)
            {
                for (int i = -validWindow; i <= validWindow; i++)
                {
                    if (SecureCompare(otp, GenerateOTP(GetTimeCode(time) + i)))
                    {
                        return true;
                    }
                }
                return false;
            }

            return SecureCompare(otp, GenerateOTP(GetTimeCode(time)));
        }

        private long GetTimeCode(DateTime time)
        {
            long unixTime = new DateTimeOffset(time).ToUnixTimeSeconds();
            return unixTime / _interval;
        }

        private string GenerateOTP(long counter)
        {
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            using (HMACSHA1 hmac = new HMACSHA1(_secretKey))
            {
                byte[] hash = hmac.ComputeHash(counterBytes);

                int offset = hash[hash.Length - 1] & 0x0F;
                int binary = ((hash[offset] & 0x7F) << 24)
                             | ((hash[offset + 1] & 0xFF) << 16)
                             | ((hash[offset + 2] & 0xFF) << 8)
                             | (hash[offset + 3] & 0xFF);

                int otp = binary % (int)Math.Pow(10, _digits);
                return otp.ToString().PadLeft(_digits, '0');
            }
        }

        private static byte[] Base32Decode(string input)
        {
            const string base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            input = input.TrimEnd('=').ToUpper();
            int byteCount = input.Length * 5 / 8;
            byte[] result = new byte[byteCount];

            int buffer = 0;
            int bitsLeft = 0;
            int index = 0;

            foreach (char c in input)
            {
                buffer = (buffer << 5) | base32Chars.IndexOf(c);
                bitsLeft += 5;
                if (bitsLeft >= 8)
                {
                    result[index++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return result;
        }

        private static bool SecureCompare(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}
