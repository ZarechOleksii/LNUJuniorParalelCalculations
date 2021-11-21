using System;
using System.Collections.Generic;
using System.Threading;

namespace MatrixAdding
{
    class Program
    {
        static void Main()
        {
            const int height = 19231;
            const int width = 17131;
            int[] threadNums = new int[] { 2, 4, 8, 16 };

            int[,] matrix1 = new int[height, width];
            int[,] matrix2 = new int[height, width];
            Random random = new();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    matrix1[i, j] = random.Next(0, 10000);
                    matrix2[i, j] = random.Next(0, 10000);
                }
            }

            //Nonparalel
            int[,] result = new int[height, width];

            var start = DateTime.Now;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            var finish = DateTime.Now;
            Console.WriteLine($"Time for nonparalel: {finish - start}");

            //Paralel
            foreach (var threadNum in threadNums)
            {
                AddMatricesAdaptive(matrix1, matrix2, threadNum, height, width);
            }
            Console.ReadLine();
        }
        public static int[,] AddMatricesAdaptive(int[,] matrix1, int[,] matrix2, int threadNum, int height, int width)
        {
            int[,] result = new int[height, width];

            var start = DateTime.Now;
            List<Thread> threads = new();

            if (width > height)
            {
                int remainder = width % threadNum;
                int remainderTrack = 0;

                for (int i = 0; i < threadNum; i++)
                {
                    int from = i * (width / threadNum) + remainderTrack;
                    int to = (i + 1) * (width / threadNum) + remainderTrack;


                    if (remainder > 0)
                    {
                        to++;
                        remainder--;
                        remainderTrack++;
                    }
                    Thread thread = new(() => AddMatricesInArea(matrix1, matrix2, result, 0, height, from, to));
                    threads.Add(thread);
                }

            }
            else
            {
                int remainder = height % threadNum;
                int remainderTrack = 0;

                for (int i = 0; i < threadNum; i++)
                {
                    int from = i * (height / threadNum) + remainderTrack;
                    int to = (i + 1) * (height / threadNum) + remainderTrack;
                    if (remainder > 0)
                    {
                        to++;
                        remainder--;
                        remainderTrack++;
                    }
                    Thread thread = new(() => AddMatricesInArea(matrix1, matrix2, result, from, to, 0, width));
                    threads.Add(thread);
                }
            }


            foreach (var x in threads)
                x.Start();
            foreach (var x in threads)
                x.Join();

            var finish = DateTime.Now;
            Console.WriteLine($"Time for paralel({threadNum} threads): {finish - start}");
            return result;
        }
        public static void AddMatricesInArea(int[,] matrix1, int[,] matrix2, int[,] changed, int fromRow, int toRow, int fromColumn, int toColumn)
        {
            for (int i = fromRow; i < toRow; i++)
            {
                for (int j = fromColumn; j < toColumn; j++)
                {
                    changed[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
        }
    }
}
