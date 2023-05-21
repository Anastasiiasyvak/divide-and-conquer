// class quickSort
// {
//     static void Main()
//     {
//         string input = Console.ReadLine()!;
//
//         string[] inputArray = input.Split(',');
//         int[] array = new int[inputArray.Length];
//
//         for (int i = 0; i < inputArray.Length; i++)
//         {
//             array[i] = int.Parse(inputArray[i].Trim());
//         }
//
//         QuickSort(array, 0, array.Length - 1);
//
//         for (int i = 0; i < array.Length - 1; i++)
//         {
//             Console.Write(array[i] + ", ");
//         }
//         
//         Console.Write(array[^1]);
//     }
//
//     static void QuickSort(int[] array, int left, int right)
//     {
//         if (left < right)
//         {
//             int separator = Position(array, left, right);
//             QuickSort(array, left, separator - 1);
//             QuickSort(array, separator + 1, right);
//         }
//     }
//
//     static int Position(int[] array, int left, int right)
//     {
//         int separator = array[right];
//         int i = -1;
//
//         for (int j = left; j < right; j++)
//         {
//             if (array[j] < separator)
//             {
//                 i++;
//                 (array[i], array[j]) = (array[j], array[i]);
//             }
//         }
//         (array[i+1], array[right]) = (array[right], array[i+1]);
//         return i + 1;
//     }
// }