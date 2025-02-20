using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Завдання 1
        Console.WriteLine("Task 1: Display Current Time and Date");
        Task task1 = new Task(DisplayDateTime);
        task1.Start();
        Task task2 = Task.Factory.StartNew(DisplayDateTime);
        Task task3 = Task.Run(DisplayDateTime);
        Task.WaitAll(task1, task2, task3);

        // Завдання 2
        Console.WriteLine("\nTask 2: Prime Numbers from 0 to 1000");
        Task primeTask = Task.Run(() => DisplayPrimes(0, 1000));
        primeTask.Wait();

        // Завдання 3
        Console.WriteLine("\nTask 3: Prime Numbers with Custom Range");
        int start = 10, end = 200;
        Task primeRangeTask = Task.Run(() => CountPrimes(start, end));
        primeRangeTask.Wait();

        // Завдання 4
        Console.WriteLine("\nTask 4: Array Statistics");
        int[] numbers = { 4, 7, 1, 9, 12, 5, 3, 8 };
        FindArrayStatistics(numbers);

        // Завдання 5
        Console.WriteLine("\nTask 5: Array Operations with Continuation Tasks");
        int[] array = { 5, 3, 8, 3, 1, 5, 7, 8, 2 };
        ProcessArray(array, 7);
    }

    static void DisplayDateTime()
    {
        Console.WriteLine($"Current Time: {DateTime.Now}");
    }

    static void DisplayPrimes(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            if (IsPrime(i)) Console.Write(i + " ");
        }
        Console.WriteLine();
    }

    static void CountPrimes(int start, int end)
    {
        int count = Enumerable.Range(start, end - start + 1).Count(IsPrime);
        Console.WriteLine($"Total prime numbers between {start} and {end}: {count}");
    }

    static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    static void FindArrayStatistics(int[] numbers)
    {
        Task<int> minTask = Task.Run(() => numbers.Min());
        Task<int> maxTask = Task.Run(() => numbers.Max());
        Task<double> avgTask = Task.Run(() => numbers.Average());
        Task<int> sumTask = Task.Run(() => numbers.Sum());

        Task.WaitAll(minTask, maxTask, avgTask, sumTask);

        Console.WriteLine($"Min: {minTask.Result}, Max: {maxTask.Result}, Avg: {avgTask.Result}, Sum: {sumTask.Result}");
    }

    static void ProcessArray(int[] array, int searchValue)
    {
        Task<int[]> removeDuplicatesTask = Task.Run(() => array.Distinct().ToArray());
        Task<int[]> sortTask = removeDuplicatesTask.ContinueWith(prevTask => prevTask.Result.OrderBy(x => x).ToArray());
        Task<int> searchTask = sortTask.ContinueWith(prevTask => Array.BinarySearch(prevTask.Result, searchValue));

        searchTask.Wait();
        Console.WriteLine("Sorted Array: " + string.Join(", ", sortTask.Result));
        Console.WriteLine(searchTask.Result >= 0 ? $"Value {searchValue} found at index {searchTask.Result}" : $"Value {searchValue} not found");
        Console.ReadLine();
    }
}
