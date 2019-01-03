using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AddArrayElementsUsingThreadLocal
{
    class Program
    {
        static void Main()
        {
            const int arrayLength = 10;//1000000
            int[] nums = Enumerable.Range(0, arrayLength).ToArray();
            long total = 0;

            // Use type parameter to make subtotal a long, not an int
            Parallel.For<long>(0, nums.Length, () => 0,
                (j, loop, subtotal) =>
                {
                    Debug.WriteLine($"    j:{j}, loop:{loop}, subtotal:{subtotal}, tid:{Thread.CurrentThread.ManagedThreadId}");
                    subtotal += nums[j];
                    return subtotal;
                },
                (x) =>
                {
                    Debug.WriteLine($"  total:{total}, x:{x}, tid:{Thread.CurrentThread.ManagedThreadId}");
                    Interlocked.Add(ref total, x);
                    Debug.WriteLine($"   total:{total}, x:{x}, tid:{Thread.CurrentThread.ManagedThreadId}");
                }
            );

            Console.WriteLine("The total is {0:N0}", total);

            var partition = Partitioner.Create(0, arrayLength, 3);
            Parallel.ForEach(partition, (range, loopState) =>
            {
                Debug.WriteLine($" range:{range}, loopState:{loopState}");
                for(var item=range.Item1; item<range.Item2; item++)
                {
                    Debug.WriteLine($"     item:{item}");
                }
            });

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}