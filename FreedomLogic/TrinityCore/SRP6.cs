using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.TrinityCore
{
    internal class SRP6
    {
        private readonly static BigInteger _generator = 7;
        private readonly static BigInteger _modulo = ToSrpBigInt("894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7");

        public static byte[] GetVerifier(string username, string password, byte[] salt)
        {
            var v = GenerateVerifier(username, password, salt);
            return v.ToByteArray().Take(32).ToArray();
        }

        private static BigInteger GenerateVerifier(string I, string P, byte[] s)
        {
            // x = H(s | H(I | ":" | P))
            var x = GeneratePrivateKey(I, P, s);

            // v = g^x
            var v = BigInteger.ModPow(_generator, x, _modulo);

            return v;
        }

        private static BigInteger GeneratePrivateKey(string I, string P, byte[] s)
        {
            // x = H(s | H(I | ":" | P))
            var x = SHA1.HashData(s.Concat(SHA1.HashData(Encoding.UTF8.GetBytes(I + ":" + P))).ToArray());
            return ToSrpBigInt(x);
        }

        private static byte[] ToBytes(string hex)
        {
            var hexAsBytes = new byte[hex.Length / 2];

            for (var i = 0; i < hex.Length; i += 2)
            {
                hexAsBytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return hexAsBytes;
        }

        // both unsigned and big endian
        private static BigInteger ToSrpBigInt(byte[] bytes)
        {
            return new BigInteger(bytes, true, true);
        }

        // Add padding character back to hex before parsing
        private static BigInteger ToSrpBigInt(string hex)
        {
            return BigInteger.Parse("0" + hex, NumberStyles.HexNumber);
        }
    }
}
