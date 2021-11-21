using System;
using System.Collections.Generic;
using System.Threading;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main()
        {
            const int height = 1212;
            const int same = 1513;
            const int width = 1321;
            int[] threadNums = new int[] { 2, 4, 8, 16, 32 };

            int[,] matrix1 = GenerateRandomMatrix(height, same);
            int[,] matrix2 = GenerateRandomMatrix(same, width);

            //Nonparalel
            int[,] result = new int[height, width];

            var start = DateTime.Now;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int sum = 0;
                    for (int q = 0; q < same; q++)
                    {
                        sum += matrix1[i, q] * matrix2[q, j];
                    }
                    result[i, j] = sum;
                }
            }

            var finish = DateTime.Now;
            Console.WriteLine($"Time for nonparalel: {finish - start}");

            //Paralel interlocked
            foreach (var threadNum in threadNums)
            {
                var temp = MultiplyMatricesInterlocked(matrix1, matrix2, threadNum);
                if (!EqualMatrices(result, temp))
                    Console.WriteLine("ERROR HERE");
            }

            //Paralel
            foreach (var threadNum in threadNums)
            {
                var temp = MultiplyMatricesAdaptive(matrix1, matrix2, threadNum);
                if (!EqualMatrices(result, temp))
                    Console.WriteLine("ERROR HERE");
            }

            Console.ReadLine();
        }
        public static int[ , ] GenerateRandomMatrix(int height, int width)
        {
            Random random = new();
            int[,] result = new int[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    result[i, j] = random.Next(0, 10000);

            return result;
        }
        public static int[,] MultiplyMatricesInterlocked(int[,] matrix1, int[,] matrix2, int threadNum)
        {
            int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            int same = matrix1.GetLength(1);

            var start = DateTime.Now;
            List<Thread> threads = new();

            int remainder = same % threadNum;
            int remainderTrack = 0;

            for (int i = 0; i < threadNum; i++)
            {
                int from = i * (same / threadNum) + remainderTrack;
                int to = (i + 1) * (same / threadNum) + remainderTrack;

                if (remainder > 0)
                {
                    to++;
                    remainder--;
                    remainderTrack++;
                }
                Thread thread = new(() => MutliplyMatricesInterlocked(matrix1, matrix2, result, from, to));
                threads.Add(thread);
            }

            foreach (var x in threads)
                x.Start();
            foreach (var x in threads)
                x.Join();

            var finish = DateTime.Now;
            Console.WriteLine($"Time for interlocked paralel({threadNum} threads): {finish - start}");
            return result;
        }
        public static void MutliplyMatricesInterlocked(int[,] matrix1, int[,] matrix2, int[,] changed, int from, int to)
        {
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int q = from; q < to; q++)
                    {
                        int toAdd = matrix1[i, q] * matrix2[q, j];
                        Interlocked.Add(ref changed[i, j], toAdd);
                    }
                }
            }
        }
        public static int[,] MultiplyMatricesAdaptive(int[,] matrix1, int[,] matrix2, int threadNum)
        {
            int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            int same = matrix1.GetLength(1);

            var start = DateTime.Now;
            List<Thread> threads = new();

            if (matrix1.GetLength(0) > matrix2.GetLength(1))
            {
                int remainder = matrix1.GetLength(0) % threadNum;
                int remainderTrack = 0;

                for (int i = 0; i < threadNum; i++)
                {
                    int from = i * (matrix1.GetLength(0) / threadNum) + remainderTrack;
                    int to = (i + 1) * (matrix1.GetLength(0) / threadNum) + remainderTrack;

                    if (remainder > 0)
                    {
                        to++;
                        remainder--;
                        remainderTrack++;
                    }
                    Thread thread = new(() => MutliplyMatricesInArea(matrix1, matrix2, result, from, to, 0, matrix2.GetLength(1)));
                    threads.Add(thread);
                }
            }
            else
            {
                int remainder = matrix2.GetLength(1) % threadNum;
                int remainderTrack = 0;

                for (int i = 0; i < threadNum; i++)
                {
                    int from = i * (matrix2.GetLength(1) / threadNum) + remainderTrack;
                    int to = (i + 1) * (matrix2.GetLength(1) / threadNum) + remainderTrack;

                    if (remainder > 0)
                    {
                        to++;
                        remainder--;
                        remainderTrack++;
                    }
                    Thread thread = new(() => MutliplyMatricesInArea(matrix1, matrix2, result, 0, matrix1.GetLength(0), from, to));
                    threads.Add(thread);
                }
            }
            foreach (var x in threads)
                x.Start();
            foreach (var x in threads)
                x.Join();

            var finish = DateTime.Now;
            Console.WriteLine($"Time for adaptive paralel({threadNum} threads): {finish - start}");
            return result;
        }
        public static void MutliplyMatricesInArea(int[,] matrix1, int[,] matrix2, int[,] changed, int fromFirst, int toFirst, int fromSecond, int toSecond)
        {
            for (int i = fromFirst; i < toFirst; i++)
            {
                for (int j = fromSecond; j < toSecond; j++)
                {
                    int sum = 0;
                    for (int q = 0; q < matrix1.GetLength(1); q++)
                    {
                        sum += matrix1[i, q] * matrix2[q, j];
                    }
                    changed[i, j] = sum;
                }
            }
        }
        public static bool EqualMatrices(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
                return false;
            for (int i = 0; i < matrix1.GetLength(0); i++)
                for (int j = 0; j < matrix1.GetLength(1); j++)
                    if (matrix1[i, j] != matrix2[i, j])
                        return false;
            return true;
        }
    }
}
