using Microsoft.Extensions.Options;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _options;

        public PasswordService(IOptions<PasswordOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// (19.1)
        /// Creates the hash for the password. This hash is the value stored in the password column in the database.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Hash(string password)
        {
            // Using goes here because once this method gets executed, the requirement is disposed (memory management).
            // PBKDF2 implementation.
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                _options.SaltSize,
                _options.Iterations
            )) {
                var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
                // The hash generation uses the iterations the salt and the key size.
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{_options.Iterations}.{salt}.{key}";
            }
        }

        /// <summary>
        /// (19.1)
        /// Compares the hash with the actual password.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format");
            }
            var iterations = Convert.ToInt32(parts[0]);
            //var iterations = Int32.Parse(parts[0]); // Alternatively
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations
            ))
            {
                var keyToCheck = algorithm.GetBytes(_options.KeySize);
                return keyToCheck.SequenceEqual(key);
            }

        }
    }
}
