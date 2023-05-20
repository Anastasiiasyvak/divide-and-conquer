
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
            value = value.Substring(1); // Remove the negative sign
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
        string[] parts = input.Split(' ');
        if (parts.Length != 3)
        {
            return null;
        }

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
        else
        {
            Console.WriteLine("Invalid operation.");
            return null;
        }

        return result;
    }

    public BigInteger Add(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        // Check if both numbers have the same sign
        if (_isNegative == another._isNegative)
        {
            result = AddPositive(another);
            result._isNegative = _isNegative; // Preserve the sign of the numbers
        }
        else
        {
            // Subtract the absolute value of the negative number from the positive number
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
            result._isNegative = result.CompareTo(BigInteger.Zero) < 0; // Set the sign based on the result
        }

        return result;
    }


    private BigInteger AddPositive(BigInteger another)
    {
        BigInteger result = new BigInteger("0");

        BigInteger longer = this._numbers.Length >= another._numbers.Length ? this : another;
        BigInteger shorter = this._numbers.Length < another._numbers.Length ? this : another;

        int maxLength = longer._numbers.Length;
        int minLength = shorter._numbers.Length;

        int[] sum = new int[maxLength + 1]; // Add one extra element for possible carry
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

public BigInteger Sub(BigInteger another)
{
    BigInteger result = new BigInteger("0");

    // Check if the current number is negative
    if (_isNegative)
    {
        // If the other number is also negative, subtract them as positive numbers
        if (another._isNegative)
        {
            _isNegative = false;
            another._isNegative = false;
            result = SubtractPositive(another);
            result._isNegative = !result.IsZero(); // Set negative if the result is not zero
        }
        else
        {
            // Subtracting a negative number is equivalent to addition
            another._isNegative = true;
            result = Add(another);
        }
    }
    else
    {
        // If the other number is negative, perform addition
        if (another._isNegative)
        {
            another._isNegative = false;
            result = Add(another);
        }
        else
        {
            // Subtract two positive numbers
            result = SubtractPositive(another);
            result._isNegative = !result.IsZero(); // Set negative if the result is not zero
        }
    }

    return result;
}

public static BigInteger Zero => new BigInteger("0");


public bool IsZero()
{
    return _numbers.Length == 1 && _numbers[0] == 0;
}


private BigInteger SubtractPositive(BigInteger another)
{
    BigInteger result = new BigInteger("0");

    BigInteger larger = this;
    BigInteger smaller = another;

    // Determine the larger and smaller numbers
    if (this.CompareTo(another) < 0)
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


    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);

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
        string input = Console.ReadLine();

        BigInteger result = BigInteger.PerformOperation(input);

        if (result != null)
        {
            Console.WriteLine($"Result: {result}");
        }
    }
}
