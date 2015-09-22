using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class CryptographicReferenceGenerator : IReferenceGenerator
    {
        public string CreateReference(int size)
        {
            const int byteSize = 0x100;
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var allowedCharSet = new HashSet<char>(chars).ToArray();

            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buffer = new byte[128];

                while (result.Length < size)
                {
                    cryptoProvider.GetBytes(buffer);

                    for (var i = 0; i < buffer.Length && result.Length < size; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize%allowedCharSet.Length);

                        if (outOfRangeStart <= buffer[i])
                        {
                            continue;
                        }

                        result.Append(allowedCharSet[buffer[i]%allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}
