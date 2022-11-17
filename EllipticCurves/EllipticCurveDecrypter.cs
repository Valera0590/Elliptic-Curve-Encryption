
namespace EllipticCurves
{
    internal class EllipticCurveDecrypter
    {
        public string Decrypt(DecryptParameters param)
        {
            string decryptedStr = "";
            for (int i = 0; i < param.EncryptedString.Length; i += 2)
            {
                var pointEncryptedCharPartA = param.Alphabet[param.EncryptedString[i]];
                var pointEncryptedCharPartB = param.Alphabet[param.EncryptedString[i + 1]];
                var result = EllipticPointsOperations.Sub(pointEncryptedCharPartB, EllipticPointsOperations.Multiply(pointEncryptedCharPartA, param.BobHiddenKey));
                var decryptedChar = param.Alphabet.First(item => item.Value.Equals(result)).Key;
                decryptedStr += decryptedChar;
            }
            return decryptedStr;
        }
    }
}
