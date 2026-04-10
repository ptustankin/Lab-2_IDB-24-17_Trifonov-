
namespace OOP_Lab1;


public class ComplexNumber
{
    public double Real { get; }
    public double Imagine { get; }

    public ComplexNumber(double real, double imagine)
    {
        Real = real;
        Imagine = imagine;
    }

    public override string ToString()
    {
        bool has_real = Real != 0;
        bool has_imagine = Imagine != 0;

        if (!has_real && !has_imagine) 
            return "0";

        if (!has_imagine) 
            return FormatReal(Real);

        string imagPart = Imagine == 1 ? "i" : Imagine == -1 ? "-i" : $"{FormatReal(Imagine)}i";

        if (!has_real) 
            return imagPart;

        // оба ненулевые
        if (Imagine > 0)
            return $"{FormatReal(Real)} + {imagPart}";
        return $"{FormatReal(Real)} - {Math.Abs(Imagine):G}i";
    }

    private static string FormatReal(double v) => v.ToString("G");

    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        => new(a.Real + b.Real, a.Imagine + b.Imagine);

    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        => new(a.Real - b.Real, a.Imagine - b.Imagine);

    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        => new(a.Real * b.Real - a.Imagine * b.Imagine,
               a.Real * b.Imagine + a.Imagine * b.Real);

    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        double denom = b.Real * b.Real + b.Imagine * b.Imagine;
        if (denom == 0) throw new DivideByZeroException("Деление на нулевое комплексное число.");
        return new((a.Real * b.Real + a.Imagine * b.Imagine) / denom,
                   (a.Imagine * b.Real - a.Real * b.Imagine) / denom);
    }

    public static bool operator ==(ComplexNumber? a, ComplexNumber? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Real == b.Real && a.Imagine == b.Imagine;
    }

    public static bool operator !=(ComplexNumber a, ComplexNumber b)
        => !(a == b);

    public static double operator +(ComplexNumber a)
        => Math.Sqrt(a.Real * a.Real + a.Imagine * a.Imagine);

    public static ComplexNumber operator -(ComplexNumber a)
        => new(a.Real, -a.Imagine);
    
    public override bool Equals(object? obj)
    {
        if (obj is not ComplexNumber other) return false;
        return Real == other.Real && Imagine == other.Imagine;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Real, Imagine);
    }
}
