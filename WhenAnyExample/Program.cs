using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhenAnyExample
{
    class Program
    {
        private static Random _random = new Random();

        static async Task Main(string[] args)
        {
            var tasks = new List<Task>
            {
                Task1(),
                Task2(),
                Task3()
            };

            var completedTask = await Task.WhenAny(tasks);
            tasks.Remove(completedTask);
            HandleTaskCompletion(completedTask);

            var restOfTheTasks = new List<Task>(tasks.Capacity);
            foreach (var task in tasks)
            {
                restOfTheTasks.Add(task.ContinueWith(HandleTaskCompletion));
            }

            await Task.WhenAll(restOfTheTasks);

            //foreach (var task in tasks)
            //{
            //    await task.ContinueWith(HandleTaskCompletion);
            //}
        }

        private static void HandleTaskCompletion(Task task)
        {
            if (task.IsFaulted)
                Console.WriteLine(task.Exception?.Flatten()?.Message ?? "null exception in faulted task");
            else
                Console.WriteLine($"task result: {(task as Task<int>)?.Result}");
        }

        static async Task<int> Task1()
        {
            await Task.Delay(GetNextRandomNumber());
            return 5;
        }

        static async Task<int> Task2()
        {
            await Task.Delay(GetNextRandomNumber());
            return 99;
        }

        static async Task<int> Task3()
        {
            await Task.Delay(GetNextRandomNumber());
            throw new Exception("I am crazy");
        }

        private static int GetNextRandomNumber() => _random.Next(10, 100);
    }
}
