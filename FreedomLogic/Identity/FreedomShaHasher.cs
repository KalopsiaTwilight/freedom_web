using FreedomLogic.Managers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class FreedomShaHasher : IPasswordHasher
    {
        public static string Sha1HashHexdecimal(string unhashed, bool reversed = false)
        {
            using (SHA1 sha = new SHA1CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(unhashed);
                byte[] hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);

                if (reversed)
                {
                    foreach (byte b in hash.Reverse())
                    {
                        // can be "x2" if you want lowercase
                        sb.Append(b.ToString("X2"));
                    }
                }
                else
                {
                    foreach (byte b in hash)
                    {
                        // can be "x2" if you want lowercase
                        sb.Append(b.ToString("X2"));
                    }
                }

                return sb.ToString();
            }
        }

        public static string Sha256HashHexdecimal(string unhashed, bool reversed = false)
        {
            using (SHA256 sha = new SHA256CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(unhashed);
                byte[] hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);

                if (reversed)
                {
                    foreach (byte b in hash.Reverse())
                    {
                        // can be "x2" if you want lowercase
                        sb.Append(b.ToString("X2"));
                    }
                }
                else
                {
                    foreach (byte b in hash)
                    {
                        // can be "x2" if you want lowercase
                        sb.Append(b.ToString("X2"));
                    }
                }

                return sb.ToString();
            }
        }

        public string HashPassword(string password)
        {
            // we are handling hashing elsewhere
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {            
            if (providedPassword.ToUpper() != hashedPassword.ToUpper())
            {
                return PasswordVerificationResult.Failed;
            }
            else
            {
                return PasswordVerificationResult.Success;
            }       
        }
    }
}
