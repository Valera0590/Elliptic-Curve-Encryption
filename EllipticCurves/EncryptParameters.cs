
namespace EllipticCurves
{
    internal class EncryptParameters
    {
        public int N { get; set; }
        public int K { get; set; }
        public EllipticPoint BobOpennedKey { get; set; }
        public EllipticPoint BobG { get; set; }
        public Dictionary<char, EllipticPoint> Alphabet { get; set; }
        public string InputStr { get; set; }
    }
}
