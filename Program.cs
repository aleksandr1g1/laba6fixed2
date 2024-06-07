public class HeronTriangleAreaStrategy : ITriangleAreaStrategy
{
    public double CalculateArea(double a, double b, double c)
    {
        double s = (a + b + c) / 2;
        return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }

}
public interface ITriangleAreaStrategy
{
    double CalculateArea(double a, double b, double c);
}
public class RightTriangleAreaStrategy : ITriangleAreaStrategy
{
    public double CalculateArea(double a, double b, double c)
    {
        if (a * a + b * b == c * c)
        {
            return 0.5 * a * b;
        }
        if (a * a + c * c == b * b)
        {
            return 0.5 * a * c;
        }
        if (b * b + c * c == a * a)
        {
            return 0.5 * b * c;
        }

        return 0;
    }
}
public class TriangleAreaContext
{

    private ITriangleAreaStrategy? _strategy;

    public TriangleAreaContext(ITriangleAreaStrategy? strategy)
    {
        _strategy = strategy;
    }

    public double CalculateArea(double a, double b, double c)
    {
        if (a <= 0 || b <= 0 || c <= 0)
        {
            throw new ArgumentException("Ошибка. Стороны треугольника должны быть > 0.");
        }
        if ((a + b <= c) || (a + c <= b) || (b + c <= a))
        {
            throw new ArgumentException("Из данных значений невозможно составить треугольник.");
        }

        if (_strategy == null)
        {
            if (a * a + b * b == c * c || a * a + c * c == b * b || b * b + c * c == a * a)
            {
                ITriangleAreaStrategy strategy = new RightTriangleAreaStrategy();
                return strategy.CalculateArea(a, b, c);
            }
            else
            {
                ITriangleAreaStrategy strategy = new HeronTriangleAreaStrategy();
                return strategy.CalculateArea(a, b, c);
            }
        }
        else
        {
            return _strategy.CalculateArea(a, b, c);
        }
    }
}
internal class Program
{
    static void Main(string[] args)
    {
        List<(double a, double b, double c)> triangles = new List<(double a, double b, double c)>()
            {
                (3, 4, 5),
                (2, 3, 4),
                (0, 0, 0),
                (6, 7, 17)
            };

        foreach (var triangle in triangles)
        {
            try
            {
                double area = new TriangleAreaContext(null).CalculateArea(triangle.a, triangle.b, triangle.c);
                Console.WriteLine($"Площадь треугольника со сторонами {triangle.a}, {triangle.b}, {triangle.c} равна {area}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}