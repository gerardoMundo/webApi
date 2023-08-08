using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApi.DTOs;

namespace WebApi.Services
{
    public class HashService
    {
        public HashResult HashTo(string flatText) 
        {
            var sal = new Byte[16];
            using(var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }

            return Hash(flatText, sal);
        }

        public HashResult Hash(string flatText, byte[] sal)
        {
            var derivedKey = KeyDerivation.Pbkdf2(password: flatText, salt: sal, prf: KeyDerivationPrf.HMACSHA1, 
                                iterationCount: 10000, numBytesRequested: 32);

            var hash = Convert.ToBase64String(derivedKey);

            return new HashResult() { Hash = hash, Sal = sal };
        }
    }
}
