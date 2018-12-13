using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WhenAnyWithCancellationToken
{
    class Program
    {
        private static Timer _timer;
        static async Task Main()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var initialTimerFireDueTime = GetNextRandomNumber();
            Console.WriteLine($"initial timer fire due time: {initialTimerFireDueTime} ms");
            _timer = new Timer(TimerCallback, null, initialTimerFireDueTime, Timeout.Infinite);

            foreach (var i in Enumerable.Range(1, 1000))
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.ffffff}: loop counter: {i}");
                var tasks = new List<Task>
                {
                    //Task11(i, cancellationTokenSource.Token.WithTimout(TimeSpan.FromMilliseconds(GetNextRandomNumber()))),
                    Task1(cancellationTokenSource.Token.WithTimout(TimeSpan.FromMilliseconds(GetNextRandomNumber()))),
                    Task2(cancellationTokenSource.Token.WithTimout(TimeSpan.FromMilliseconds(GetNextRandomNumber()))),
                    Task3(cancellationTokenSource.Token.WithTimout(TimeSpan.FromMilliseconds(GetNextRandomNumber()))),
                    Task4()
                };

                Console.WriteLine($"{DateTime.Now:HH:mm:ss.ffffff}: waiting on any task to complete");
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);
                HandleTaskCompletion(completedTask);
                AttachTaskCompletionHandler(tasks);

                Console.WriteLine($"{DateTime.Now:HH:mm:ss.ffffff}: end of loop: {i}{Environment.NewLine}");
            }

            void TimerCallback(object _)
            {
                // stop the timer from being fired
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.ffffff}: cancel the token");
                // cancel the token
                cancellationTokenSource.Cancel();
                var nextTimerDueTime = GetNextRandomNumber();
                _timer.Change(nextTimerDueTime, Timeout.Infinite);
            }
        }

        private static void AttachTaskCompletionHandler(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                task.ContinueWith(HandleTaskCompletion);
            }
        }

        private static void HandleTaskCompletion(Task task)
        {
            if (task.IsFaulted)
                Console.WriteLine($"*** {DateTime.Now:HH:mm:ss.ffffff}: id:{task.Id} {task.Exception?.Flatten()?.Message ?? "null exception in faulted task"}");
            else if (task.IsCanceled)
                Console.WriteLine($"*** {DateTime.Now:HH:mm:ss.ffffff}: task id:{task.Id} was cancelled");
            else
                Console.WriteLine($"*** {DateTime.Now:HH:mm:ss.ffffff}: task id:{task.Id} result: {(task as Task<int>)?.Result}");
        }

        //static async Task<int> Task11(int i, CancellationToken token)
        //{
        //    var taskCompletionSource = new TaskCompletionSource<int>();
        //    while (true)
        //    {
        //        token.ThrowIfCancellationRequested();
        //        var randomWait = GetNextRandomNumber();
        //        Console.WriteLine($" Task11 id:{i} with taskId: {taskCompletionSource.Task.Id} - Delay: {randomWait} ms");
        //        await Task.Delay(randomWait, token);

        //        if (randomWait % 33 == 0)
        //        {
        //            Console.WriteLine($" Task1 done for randomWait: {randomWait} ms");
        //            break;
        //        }
        //    }

        //    taskCompletionSource.SetResult(33);
        //    return await taskCompletionSource.Task;
        //}

        static async Task<int> Task1(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var randomWait = GetNextRandomNumber();
                var task = Task.Delay(randomWait, cancellationToken);
                Console.WriteLine($"  {DateTime.Now:HH:mm:ss.ffffff}: Task1 id:{task.Id} - Delay: {randomWait}");
                await task;

                if (randomWait % 33 == 0)
                {
                    Console.WriteLine($"  {DateTime.Now:HH:mm:ss.ffffff}: Task1 done for randomWait: {randomWait} ms");
                    return 33;
                }
            }
        }

        static async Task<int> Task2(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var randomWait = GetNextRandomNumber();
                var task = Task.Delay(randomWait, cancellationToken);
                Console.WriteLine($"   {DateTime.Now:HH:mm:ss.ffffff}: Task2 id:{task.Id} - Delay: {randomWait}");
                await task;

                if (randomWait % 33 == 0)
                {
                    Console.WriteLine($"   {DateTime.Now:HH:mm:ss.ffffff}: Task2 done for randomWait: {randomWait} ms");
                    return 33;
                }
            }
        }

        static async Task<int> Task3(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var randomWait = GetNextRandomNumber();
                var task = Task.Delay(randomWait, cancellationToken);
                Console.WriteLine($"    {DateTime.Now:HH:mm:ss.ffffff}: Task3 id:{task.Id} - Delay: {randomWait}");
                await task;

                if (randomWait % 33 == 0)
                {
                    Console.WriteLine($"    {DateTime.Now:HH:mm:ss.ffffff}: Task3 done for randomWait: {randomWait} ms - throwing exception");
                    throw new Exception("Task3 is crazy");
                }
            }
        }

        static async Task<int> Task4()
        {
            while (true)
            {
                var randomWait = GetNextRandomNumber();
                var task = Task.Delay(randomWait);
                Console.WriteLine($"     {DateTime.Now:HH:mm:ss.ffffff}: Task4 id:{task.Id} - Delay: {randomWait}");
                await task;

                if (randomWait % 33 == 0)
                {
                    Console.WriteLine($"     {DateTime.Now:HH:mm:ss.ffffff}: Task4 done for randomWait: {randomWait} ms - throwing exception");
                    throw new Exception("Task4 is crazy");
                }
            }
        }

        private static readonly Random Random = new Random();
        private static int GetNextRandomNumber() => Random.Next(10, 100);
    }
}
