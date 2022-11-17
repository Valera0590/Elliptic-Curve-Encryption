
namespace EllipticCurves
{
    internal class EllipticCurveEcnrypter
    {
        public string Crypt(EncryptParameters param)
        {
            var cryptedCharacters = new List<(EllipticPoint, EllipticPoint)>();
            string cryptedStr = "";
            Random rnd = new Random();
            foreach (var symbol in param.InputStr)
            {
                var fromAlphabet = param.Alphabet[symbol];
                param.K = rnd.Next(2, param.N);
                var x = EllipticPointsOperations.Multiply(param.BobG, param.K);
                var y = EllipticPointsOperations.Sum(fromAlphabet, EllipticPointsOperations.Multiply(param.BobOpennedKey, param.K));
                cryptedCharacters.Add((x, y));
                cryptedStr += param.Alphabet.First(item => item.Value.Equals(x)).Key;
                cryptedStr += param.Alphabet.First(item => item.Value.Equals(y)).Key;
            }
            return cryptedStr;
        }
    }
}
