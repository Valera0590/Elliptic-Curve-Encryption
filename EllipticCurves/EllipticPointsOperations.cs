
namespace EllipticCurves
{
    internal static class EllipticPointsOperations
    {
        const int _OValueEquivalent = -999;

        public static int Modulo { get; set; }

        public static int A { get; set; }

        public static int GetPointOrder(EllipticPoint point)
        {
            for (int i = 1; i < Modulo; ++i)
            {
                if (Multiply(point, i).IsO) return i;
                //if (i > 13) return 39;
            }
            return -1;
        }
        public static int GetLambdaForSum(EllipticPoint point1, EllipticPoint point2)
        {
            int reversed;
            if (!point1.Equals(point2))
            {
                if (point1.X == point2.X)
                {
                    return _OValueEquivalent;
                }
                TryModInverse(GetNumberByModulo(point2.X - point1.X), out reversed);
                return GetNumberByModulo((point2.Y - point1.Y) * reversed);
            }

            TryModInverse(GetNumberByModulo(2 * point1.Y), out reversed);
            return GetNumberByModulo((3 * (int)Math.Pow(point1.X, 2) + A) * reversed);
        }
        public static bool TryModInverse(int number, out int result)
        {
            int n = number;
            int m = Modulo, v = 0, d = 1;
            while (n > 0)
            {
                int t = m / n, x = n;
                n = m % x;
                m = x;
                x = d;
                d = checked(v - t * x); // Just in case
                v = x;
            }
            result = v % Modulo;
            if (result < 0) result += Modulo;
            if ((long)number * result % Modulo == 1L) return true;
            result = default;
            return false;
        }
        public static EllipticPoint Sum(EllipticPoint point1, EllipticPoint point2)
        {
            if (point1.IsO)
            {
                return point2;
            }

            if (point2.IsO)
            {
                return point1;
            }

            if ((point1.X == point2.X) && (point1.Y == -point2.Y))
            {
                return new EllipticPoint(0, 0)
                {
                    IsO = true
                };
            }

            int lambda = GetLambdaForSum(point1, point2);
            if (lambda == _OValueEquivalent)
            {
                return new EllipticPoint(0, 0) { IsO = true };
            }
            int newX = GetNumberByModulo((int)Math.Pow(lambda, 2) - point1.X - point2.X);
            int newY = GetNumberByModulo(lambda * (point1.X - newX) - point1.Y);

            return new EllipticPoint(newX, newY);
        }
        public static EllipticPoint Sub(EllipticPoint point1, EllipticPoint point2)
        {
            point2.Y = GetNumberByModulo(-point2.Y);
            return Sum(point1, point2);
        }
        public static int GetNumberByModulo(int num) => num % Modulo > 0 ? num % Modulo : num % Modulo + Modulo;

        public static EllipticPoint Multiply(EllipticPoint point, int factor)
        {
            EllipticPoint result = point;
            for (int i = 0; i < factor - 1; ++i)
            {
                result = Sum(result, point);
            }
            return result;
        }
    }
}
