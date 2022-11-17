
namespace EllipticCurves
{
    internal class EllipticCurve
    {
        public EllipticCurve(int modulo)
        {
            Modulo = modulo;
            Points = new List<EllipticPoint>();
            FindPoints();
        }
        public List<EllipticPoint> Points { get; set; }

        public int Modulo { get; set; }

        private void FindPoints()
        {
            Points.Add(new EllipticPoint(0, 0) { IsO = true });

            for (int x = 0; x < Modulo; ++x)
            {
                for (int y = 0; y < Modulo; ++y)
                {
                    if (((int)Math.Pow(y, 2) % Modulo) == (((int)Math.Pow(x, 3) + x + 3)) % Modulo)
                    {
                        Points.Add(new EllipticPoint(x, y));
                    }
                }
            }
        }
    }
}
