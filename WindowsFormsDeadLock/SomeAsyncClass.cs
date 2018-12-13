using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsFormsDeadLock
{
    public class SomeAsyncClass
    {
        private static readonly Random Random = new Random();
        private static int GetNextRandomNumber() => Random.Next(1, 10);
        public static async Task<string> GetStringAsync1()
        {
            var someNumber = GetNextRandomNumber();
            Debug.WriteLine($"SomeAsyncClass.GetStringAsync random delay time: {someNumber}");
            await Task.Delay(TimeSpan.FromSeconds(someNumber));
            var result = string.Join(", ", Enumerable.Repeat("blah1", someNumber));
            Debug.WriteLine($"SomeAsyncClass.GetStringAsync result: {result}");
            return result;
        }
        public static async Task<string> GetStringAsync2()
        {
            var someNumber = GetNextRandomNumber();
            Debug.WriteLine($"SomeAsyncClass.GetStringAsync random delay time: {someNumber}");
            await Task.Delay(TimeSpan.FromSeconds(someNumber)).ConfigureAwait(false);
            var result = string.Join(", ", Enumerable.Repeat("blah2", someNumber));
            Debug.WriteLine($"SomeAsyncClass.GetStringAsync result: {result}");
            return result;
        }
    }
}