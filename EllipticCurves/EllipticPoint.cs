
namespace EllipticCurves
{
    internal class EllipticPoint
    {
        public EllipticPoint(int x, int y)
        {
            X = x;
            Y = y;
            IsO = x == 0 && y == 0;
        }
        public int X { get; set; }

        public int Y { get; set; }

        public int Order { get; set; } = 1;
        public bool IsO { get; set; }

        public bool Equals(EllipticPoint point) => (point.X == X) && (point.Y == Y);
        
    }
}
