using System.Numerics;
using System.Security.Cryptography;

namespace JabNetClient
{
    internal class Authorisation
    {
        






        public static BigInteger GenerateRandom1024BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[128];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 1024 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 1024 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный

        public static BigInteger GenerateRandom512BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[64];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 512 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 512 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный

        public static BigInteger GenerateRandom256BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[32];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 256 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 256 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный
    }
}