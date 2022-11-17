using EllipticCurves;

static class Program
{
    const string _alphabetSymbols = "qwertyuiopasdfghjklzxcvbnm1234567890!.,";
    const int _modulo = 41;
    const int _a = 1;
    const int _n = 9;
    static void Main(string[] args)
    {
        Random random = new Random();
        int _bobHiddenKey = random.Next(2, _n - 1);
        EllipticPoint _bobG = new EllipticPoint(-1, -1);
        EllipticPoint _bobOpennedKey = new EllipticPoint(-1, -1);
        EllipticPointsOperations.Modulo = _modulo;
        EllipticPointsOperations.A = _a;
        string encryptedStr = "";
        var alphabet = FillAlphabet();
        var curve = new EllipticCurve(_modulo);
        var curvePoints = FillOrderPoints(curve);
        var encryptParam = new EncryptParameters();
        encryptParam.Alphabet = alphabet;
        var decryptParam = new DecryptParameters();
        decryptParam.Alphabet = alphabet;
        char answer = '0';
        bool isExit = false;

        while (!isExit)
        {
            Console.WriteLine(
                " List of actions:\n" +
                "    1 : Enter Bob's openned key\n" +
                "    2 : Encrypt (message to Bob)\n" +
                "    3 : Decrypt (message from Alice)\n" +
                "    4 : Print openned keys\n" +
                "    5 : Exit");
            Console.Write(" Input: ");
            answer = char.Parse(Console.ReadLine()!);
            switch (answer)
            {
                case '1':
                {
                    Console.WriteLine(" Here's all curve points with order 13, choose one (Bob's G): ");
                    var newCurvePoints = curvePoints.FindAll(item => item.Order == 13 || item.IsO);
                    /*for (int i = 0; i < curvePoints.Count; ++i)
                    {
                        if (i % 5 == 0 && i != 0) Console.WriteLine();
                        Console.Write($" {i+1}. [{curvePoints[i].X}; {curvePoints[i].Y}]\t");
                    }*/
                    for (int i = 0; i < newCurvePoints.Count(); ++i)
                    {
                        if (i % 5 == 0 && i != 0) Console.WriteLine();
                        Console.Write($" {i + 1}. [{newCurvePoints[i].X}; {newCurvePoints[i].Y}]\t");
                    }

                    Console.Write("\n Index of point: ");
                    var choosenIndex = int.Parse(Console.ReadLine()!);
                    _bobG = curvePoints[choosenIndex-1];

                    //choose client
                    Console.WriteLine(" Are you Bob? (y/n)");
                    Console.Write(" Input: ");
                    if (Console.ReadLine() == "y")
                    {
                        _bobOpennedKey = EllipticPointsOperations.Multiply(_bobG, _bobHiddenKey);
                        Console.WriteLine($"Openned key is generated - [{_bobOpennedKey.X};{_bobOpennedKey.Y}]");
                    }
                    else
                    {
                        Console.WriteLine(" Coordinates of openned key (point): ");
                        Console.Write(" X: ");
                        var x = int.Parse(Console.ReadLine()!);
                        Console.Write(" Y: ");
                        var y = int.Parse(Console.ReadLine()!);
                        _bobOpennedKey = new EllipticPoint(x, y);
                    }

                    break;
                }
                case '2':
                {
                    if (_bobG.X == -1 || _bobG.Y == -1)
                    {
                        Console.WriteLine(" Do step '1' before encrypting");
                        break;
                    }

                    Console.WriteLine($" Allowing characters is {_alphabetSymbols}");
                    Console.Write(" Text to encrypt: ");
                    string strToEncrypt = Console.ReadLine();
                    encryptParam.BobG = _bobG;
                    encryptParam.InputStr = strToEncrypt;
                    encryptParam.N = _n;
                    encryptParam.BobOpennedKey = _bobOpennedKey;
                    encryptedStr = new EllipticCurveEcnrypter().Crypt(encryptParam);
                    Console.WriteLine($" Encrypted string: {encryptedStr}");
                    break;
                }
                case '3':
                {

                    if (_bobG.X == -1 || _bobG.Y == -1)
                    {
                        Console.WriteLine(" Do step '1' before decrypting");
                        break;
                    }

                    Console.Write(" Text to decrypt: ");
                    string strToDecrypt = Console.ReadLine();
                    decryptParam.BobHiddenKey = _bobHiddenKey;
                    decryptParam.EncryptedString = encryptedStr;
                    decryptParam.EncryptedString = strToDecrypt;
                    string decryptedStr = new EllipticCurveDecrypter().Decrypt(decryptParam);
                    Console.WriteLine($" Decrypted string: {decryptedStr}");
                    break;
                }
                case '4':
                {
                    Console.WriteLine(
                        $" Alice's 'n': {_n}\n" +
                        $" Bob's 'd': {_bobHiddenKey}\n" +
                        $" Bob G: [{_bobG.X}; {_bobG.Y}]\n" +
                        $" Bob openned key: [{_bobOpennedKey.X};{_bobOpennedKey.Y}]");

                    break;
                }
                case '5':
                {
                    Console.Write(" Exit from programm ........\n");
                    isExit = true;
                    continue;
                }
            }

            Console.WriteLine("\n" + new String('-', 25) + "\n");

        }
    }

    static Dictionary<char, EllipticPoint> FillAlphabet()
    {
        var alphabet = new Dictionary<char, EllipticPoint>();
        var curvePoints = new EllipticCurve(_modulo).Points;
        for (int i = 0; i < curvePoints.Count; ++i)
        {
            alphabet.Add(_alphabetSymbols[i], curvePoints[i]);
        }
        return alphabet;
    }

    static List<EllipticPoint> FillOrderPoints(EllipticCurve curve)
    {
        foreach (var point in curve.Points)
            point.Order =  point.X != 0 && point.Y != 0 ? EllipticPointsOperations.GetPointOrder(point) : -1;
        return curve.Points;
    }
}