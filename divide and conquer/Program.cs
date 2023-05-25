public class BigInteger
{
    private int[] _numbers;
    private bool _isNegative;

    public BigInteger(string value)
    {
        _isNegative = false;

        if (value.StartsWith("-"))
        {
            _isNegative = true;
            value = value.Substring(1);
        }

        _numbers = new int[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            _numbers[i] = int.Parse(value[value.Length - 1 - i].ToString());
        }
    }

    public override string ToString()
    {
        string result = "";
        for (int i = _numbers.Length - 1; i >= 0; i--)
        {
            result += _numbers[i].ToString();
        }
        if (_isNegative)
        {
            result = "-" + result;
        }
        return result;
    }


    public static BigInteger PerformOperation(string input)
    {
        string[] parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        string num1 = parts[0];
        string operation = parts[1];
        string num2 = parts[2];

        BigInteger x = new BigInteger(num1);
        BigInteger y = new BigInteger(num2);

        BigInteger result;
        if (operation == "+")
        {
            result = x + y;
        }
        else if (operation == "-")
        {
            result = x - y;
        }
        else if (operation == "*")
        {
            result = x * y;
        }
        else
        {
            return null;
        }

        return result;
    }

    public BigInteger Add(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        if (_isNegative == another._isNegative)
        {
            result = AddPositive(another);
            result._isNegative = _isNegative;
        }
        else
        {
            if (_isNegative)
            {
                _isNegative = false;
                result = another.SubtractPositive(this);
            }
            else
            {
                another._isNegative = false;
                result = SubtractPositive(another);
            }
            result._isNegative = result.CompareTo(Zero) < 0;
        }

        return result;
    }
    public BigInteger Sub(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        if (_isNegative)
        {
            if (another._isNegative)
            {
                _isNegative = false;
                another._isNegative = false;
                result = SubtractPositive(another);
                result._isNegative = !result.IsZero();
            }
            else
            {
                another._isNegative = true;
                result = Add(another);
            }
        }
        else
        {
            if (another._isNegative)
            {
                another._isNegative = false;
                result = Add(another);
            }
            else
            {
                result = SubtractPositive(another);
                result._isNegative = result.CompareTo(Zero) < 0;
            }
        }

        return result;
    }
    private BigInteger AddPositive(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        BigInteger longer = _numbers.Length >= another._numbers.Length ? this : another;
        BigInteger shorter = _numbers.Length < another._numbers.Length ? this : another;

        int maxLength = longer._numbers.Length;
        int minLength = shorter._numbers.Length;

        int[] sum = new int[maxLength + 1];
        int carry = 0;

        for (int i = 0; i < maxLength; i++)
        {
            int currentSum = longer._numbers[i] + (i < minLength ? shorter._numbers[i] : 0) + carry;
            carry = currentSum / 10;
            sum[i] = currentSum % 10;
        }

        if (carry > 0)
        {
            sum[maxLength] = carry;
            result._numbers = sum;
        }
        else
        {
            result._numbers = new int[maxLength];
            Array.Copy(sum, result._numbers, maxLength);
        }

        return result;
    }
    private BigInteger SubtractPositive(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        BigInteger larger = this;
        BigInteger smaller = another;

        if (CompareTo(another) < 0)
        {
            larger = another;
            smaller = this;
            result._isNegative = true;
        }
        
        int maxLength = larger._numbers.Length;
        int minLength = smaller._numbers.Length;

        int[] difference = new int[maxLength];
        int carry = 0;

        for (int i = 0; i < maxLength; i++)
        {
            int currentDifference = larger._numbers[i] - (i < minLength ? smaller._numbers[i] : 0) - carry;
            if (currentDifference < 0)
            {
                currentDifference += 10;
                carry = 1;
            }
            else
            {
                carry = 0;
            }

            difference[i] = currentDifference;
        }
        
        int leadingZeros = 0;
        for (int i = difference.Length - 1; i >= 0; i--)
        {
            if (difference[i] == 0)
            {
                leadingZeros++;
            }
            else
            {
                break;
            }
        }

        result._numbers = new int[maxLength - leadingZeros];
        Array.Copy(difference, result._numbers, maxLength - leadingZeros);

        return result;
    }
    public BigInteger Multiply(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        if (IsZero() || another.IsZero())
        {
            return result;
        }

        BigInteger larger = this;
        BigInteger smaller = another;

        if (CompareTo(another) < 0)
        {
            larger = another;
            smaller = this;
        }

        int largerLength = larger._numbers.Length;
        int smallerLength = smaller._numbers.Length;

        if (smallerLength == 1)
        {
            return Mul(larger, smaller);
        }

        int max = Math.Max(largerLength, smallerLength);
        int m = max / 2;
        
        if (smallerLength < m)
        {
            m = smallerLength / 2;
        }

        BigInteger x0 = new BigInteger(string.Join("", larger._numbers[..m].Reverse()));
        BigInteger x1 = new BigInteger(string.Join("", larger._numbers[m..].Reverse()));
        BigInteger y0 = new BigInteger(string.Join("", smaller._numbers[..m].Reverse()));
        BigInteger y1 = new BigInteger(string.Join("", smaller._numbers[m..].Reverse()));

        BigInteger z0 = x0.Multiply(y0);
        BigInteger z2 = x1.Multiply(y1);
        BigInteger z1 = (x0 + x1).Multiply(y0 + y1) - z0 - z2;

        result = Power(z2, m * 2) + Power(z1, m * 1) + z0;
        result._isNegative = _isNegative != another._isNegative;

        return result;
    }
    private static BigInteger Power(BigInteger number, int count)
    {
        BigInteger result = new BigInteger("0");
        result._numbers = new int[count + number._numbers.Length];
        Array.Copy(number._numbers, 0, result._numbers, count, number._numbers.Length);

        for (int i = 0; i < count; i++)
        {
            result._numbers[i] = 0;
        }

        result._isNegative = number._isNegative;

        return result;
    }
    
    private BigInteger Mul(BigInteger num1, BigInteger num2)
    {
        BigInteger result = new BigInteger("0");

        if (num1.IsZero() || num2.IsZero())
        {
            return result;
        }

        BigInteger larger = num1;
        BigInteger smaller = num2;

        int largerLength = larger._numbers.Length;
        int smallerLength = smaller._numbers.Length;

        int[] product = new int[largerLength + smallerLength];

        for (int i = 0; i < num1._numbers.Length; i++)
        {
            int carry = 0;

            for (int j = 0; j < num2._numbers.Length; j++)
            {
                int currentProduct = num1._numbers[i] * num2._numbers[j] + product[i + j] + carry;
                carry = currentProduct / 10;
                product[i + j] = currentProduct % 10;
            }

            if (carry > 0)
            {
                product[i + num2._numbers.Length] = carry;
            }
        }

        int leadingZeros = 0;
        for (int i = product.Length - 1; i >= 0; i--)
        {
            if (product[i] == 0)
            {
                leadingZeros++;
            }
            else
            {
                break;
            }
        }

        result._numbers = new int[product.Length - leadingZeros];
        Array.Copy(product, 0, result._numbers, 0, product.Length - leadingZeros);

        result._isNegative = num1._isNegative != num2._isNegative;

        return result;
    }

    public static BigInteger Zero => new ("0");
    
    public bool IsZero()
    {
        for (int i = 0; i < _numbers.Length; i++)
        {
            if (_numbers[i] != 0)
            {
                return false;
            }
        }
        return true;
    }

    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);
    public static BigInteger operator *(BigInteger a, BigInteger b) => a.Multiply(b);

    public int CompareTo(BigInteger another)
    {
        if (_isNegative && !another._isNegative)
        {
            return -1;
        }
        if (!_isNegative && another._isNegative)
        {
            return 1;
        }

        if (_numbers.Length < another._numbers.Length)
        {
            return -1;
        }
        if (_numbers.Length > another._numbers.Length)
        {
            return 1;
        }

        for (int i = _numbers.Length - 1; i >= 0; i--)
        {
            if (_numbers[i] < another._numbers[i])
            {
                return -1;
            }
            if (_numbers[i] > another._numbers[i])
            {
                return 1;
            }
        }

        return 0;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the expression:");
        string input = Console.ReadLine()!;
        BigInteger result = BigInteger.PerformOperation(input);
        Console.WriteLine($"Result: {result}");
    }
}