using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace asyncVoid
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TaskScheduler.UnobservedTaskException += (o, e) =>
            {
                Console.WriteLine($"TaskScheduler.UnobservedTaskException: {e.Exception}");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine($"AppDomain.CurrentDomain.UnhandledException: {e.ExceptionObject}");
            };

            try
            {
                SomeMethodThatThrowsException();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{exception}");
            }

            SomeMethodThatThrowsException2();

            Thread.Sleep(3000);
        }

        private static void SomeMethodThatThrowsException2()
        {
            throw new Exception("moo");
        }

        private static async void SomeMethodThatThrowsException()
        {
            await Task.Delay(100);
            throw new Exception("blah blah blah");
        }
    }
}
