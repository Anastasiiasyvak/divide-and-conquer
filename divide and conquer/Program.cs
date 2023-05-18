public class BigInteger
{
     private int[] _numbers;
     private bool _isNegative;
     
     public BigInteger(string value)
     {
         _numbers = new int[value.Length];
         for (int i = 0; i < value.Length; i++)
         {
             _numbers[i] = int.Parse(value[value.Length - 1 - i].ToString());
         }
         _isNegative = false;
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
             Console.WriteLine("Invalid input format. Please provide two numbers and an operation separated by spaces.");
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
     
         // Determine the longer and shorter numbers
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
     
         // Determine the larger and smaller numbers
         BigInteger larger = this;
         BigInteger smaller = another;
     
         // Check if the larger number is negative
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
     
         // Remove leading zeros
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
        // Compare the sign
        if (_isNegative && !another._isNegative)
        {
            return -1;
        }
        if (!_isNegative && another._isNegative)
        {
            return 1;
        }

        // Compare the number of digits
        if (_numbers.Length < another._numbers.Length)
        {
            return -1;
        }
        if (_numbers.Length > another._numbers.Length)
        {
            return 1;
        }

        // Compare the digits from left to right
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

        // The numbers are equal
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
            Console.WriteLine("Result:");
            Console.WriteLine(result);
        }
    }
}
