using System;

namespace all
{
    public class algorithm
    {
        private int[] _knumbers;
        private bool _isNegative;

        public algorithm(string value)
        {
            _isNegative = false;
            if (value.StartsWith("-"))
            {
                _isNegative = true;
                value = value.Substring(1);
            }

            foreach (int i in _knumbers)
            {
                Console.WriteLine(i);
            }

        }

        public static void Main()
        {
            var x = new algorithm("13145");

            // Rest of your program logic
        }
    }
}
