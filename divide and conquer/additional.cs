//
// using System.Numerics;  // для можливості використовувати структуру BigInteger 
//
// internal class Program 
// {
//     public static void Main(string[] args) 
//     {
//         Console.WriteLine("Enter an expression: ");
//         string expression = Console.ReadLine()!;
//         List<string> opers1 = new List<string> { "+", "-", "*", "/", "^", "(", ")" };
//         List<string> opers2 = new List<string> { "+", "-", "*", "/", "^" }; 
//         var stack = new MyStack();
//         var queue = new MyQueue();
//         string[] tokens = new string[50];
//         string buffer = "";
//         int i = 0;
//
//         foreach (char e in expression)
//         {
//             if (Char.IsDigit(e))
//             {
//                 buffer += e;
//             }
//             else if (Char.IsWhiteSpace(e))
//             {
//                 if (buffer != "")
//                 {
//                     tokens[i] = buffer;
//                     buffer = "";
//                     i++;
//                 }
//             }
//             else if (e == '+' || e == '-' || e == '*' || e == '/' || e == '^' || e == '(' || e == ')')
//             {
//                 if (buffer != "")
//                 {
//                     tokens[i] = buffer;
//                     buffer = "";
//                     i++;
//                 }
//                 tokens[i] = e.ToString();
//                 i++;
//             }
//         }
//         if (buffer != "")
//         {
//             tokens[i] = buffer;
//         }
//         foreach (string s in tokens)
//         {
//             if (!opers1.Contains(s))
//             {
//                 queue.Enqueue(s);
//             }
//             else if (opers2.Contains(s))
//             {
//                 Dictionary<string, int> priorities = new Dictionary<string, int>
//                 {
//                     { "+", 1 },
//                     { "-", 1 },
//                     { "*", 2 },
//                     { "/", 2 },
//                     { "^", 3 }
//                 };
//                 int curPriority = priorities[s];
//                 while (stack.Count() > 0 && (stack.Peek() == "*" || stack.Peek() == "/" || stack.Peek() == "^"))
//                 {
//                     queue.Enqueue(stack.Pop());
//                 }
//                 if (stack.Count() > 0 && (stack.Peek() == "+" || stack.Peek() == "-"))
//                 {
//                     int lastPriority = 1;
//                     string lastOper = stack.Peek();
//                     if (lastOper == "+" || lastOper == "-")
//                     {
//                         lastPriority = 1;
//                     }
//                     else if (lastOper == "*" || lastOper == "/")
//                     {
//                         lastPriority = 2;
//                     }
//                     else if (lastOper == "^")
//                     {
//                         lastPriority = 3;
//                     }
//
//                     if (curPriority <= lastPriority)
//                     {
//                         lastOper = stack.Pop();
//                         queue.Enqueue(lastOper);
//                     }
//                 }
//
//                 stack.Push(s);
//             }
//             else if (s == "(")
//             {
//                 stack.Push(s);
//             }
//             else if (s == ")")
//             {
//                 while (stack.Count() > 0 && stack.Peek() != "(")
//                 {
//                     queue.Enqueue(stack.Pop());
//                 }
//                 if (stack.Count() > 0 && stack.Peek() == "(")
//                 {
//                     stack.Pop();
//                 }
//             }
//         }
//         while (stack.Count() > 0)
//         {
//             string lastOperator = stack.Pop();
//             queue.Enqueue(lastOperator);
//         }
//         foreach (string q in queue.GetElements())
//         {
//             if (q is null)
//             {
//                 continue;
//             }
//             if (opers2.Contains(q))
//             {
//                 BigInteger operand2 = BigInteger.Parse(stack.Pop());
//                 BigInteger operand1 = BigInteger.Parse(stack.Pop());
//                 BigInteger result = 0;
//                 if (q == "+")
//                 {
//                     result = operand1 + operand2;
//                 }
//                 else if (q == "-")
//                 {
//                     result = operand1 - operand2;
//                 }
//                 else if (q == "*")
//                     result = operand1 * operand2;
//                 else if (q == "/")
//                     result = operand1 / operand2;
//                 else if (q == "^")
//                 {
//                     result = BigInteger.Pow(operand1, (int)operand2);
//                 }
//                 stack.Push(result.ToString());
//             }
//             else if (!opers2.Contains(q))
//             {
//                 BigInteger newQ = BigInteger.Parse(q);
//                 stack.Push(newQ.ToString());
//             }
//         }
//         BigInteger final = BigInteger.Parse(stack.Pop());
//         Console.WriteLine($"Result: {final}");
//     }
// }
//
//
//
// public class MyStack
// {
//     private const int Capacity = 50;
//
//     private string[] _array = new string[Capacity];
//
//     private int _pointer = 0;
//
//     public void Push(string value)
//     {
//         _array[_pointer] = value;
//         _pointer++;
//     }
//
//     public string Pop()
//     {
//         string result = _array[_pointer - 1];
//         _pointer--;
//         return result;
//     }
//
//     public int Count()
//     {
//         return _pointer;
//     }
//
//     public string Peek()
//     {
//         if (_pointer == -1)
//         {
//             throw new InvalidOperationException();
//         }
//         return _array[_pointer - 1];
//     }
// }
//
//
//
// public class MyQueue
// {
//     private const int Capacity = 50;
//     private string[] _array = new string[Capacity];
//
//     private int _pointer = 0;
//     private int _headPoint;
//     private int _tailPoint;
//
//     public MyQueue()
//     {
//         _headPoint = 0;
//         _tailPoint = -1;
//     }
//
//     public void Enqueue(string value)
//     {
//         _tailPoint++;
//         _array[_tailPoint] = value;
//     }
//
//     public string[] GetElements()
//     {
//         return _array;
//     }
// }
//
