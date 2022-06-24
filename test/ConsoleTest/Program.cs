
Number n = new Number(2, 2.0);
Number n2 = new Number(3, 3.0);
 var num= n * n2;
Console.WriteLine(num);
Console.ReadLine();


// Define other methods and classes here
public struct Number
{
    public int intV;
    public double doubleV;
    public Number(int iVal, double dVal)
    {
        intV = iVal;
        doubleV = dVal;
    }

    public Number(int val) : this(val, 0)
    { }

    public Number(double val) : this(0, val)
    { }

    public static Number operator *(Number num, Number num2)
    {
        var t = num.intV*num2.intV;
        var t2 = num.doubleV* num2.doubleV;
        var newt = new Number(t, t2);
        return newt;
    }

    public override string ToString()
    {
        return $"intV={intV},doubleV={doubleV}";

    }
}