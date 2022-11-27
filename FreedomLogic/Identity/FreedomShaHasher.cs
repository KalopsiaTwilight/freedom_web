using FreedomLogic.Managers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Identity
{
    public class FreedomShaHasher : IPasswordHasher<User>
    {
        private static string Sha1HashHexdecimal(string unhashed, bool reversed = false)
        {
            using (SHA1 sha = SHA1.Create())
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

        private static string Sha256HashHexdecimal(string unhashed, bool reversed = false)
        {
            using (SHA256 sha = SHA256.Create())
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

        public static string CalculateBnetHash(string username, string password)
        {
            string userEmail = (username + AccountManager.FreedomBnetEmailConst).ToUpper();
            string usernameEmailHash = Sha256HashHexdecimal(userEmail);
            return Sha256HashHexdecimal(usernameEmailHash + ":" + password.ToUpper(), true);
        }

        public string HashPassword(User user, string password)
        {
            // we are handling hashing elsewhere
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {            
            if (CalculateBnetHash(user.UserName, providedPassword) != hashedPassword)
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
