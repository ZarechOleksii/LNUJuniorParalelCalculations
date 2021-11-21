using System;
using System.Collections.Generic;
using System.Threading;

namespace EquationSystem2
{
    class Program
    {
        static void Main()
        {
            const int size = 13000;
            const double epsilon = 0.001;
            var generated = GenerateRandomLinearEquation(size);
            int[] threadNum = new int[] { 2, 4, 8, 16 };

            double[] XNonParalel = new double[size];

            Console.WriteLine("Starting nonparalel:");
            var start = DateTime.Now;
            JacobiNonparalel(size, generated["Matrix"], generated["Free elements"], ref XNonParalel, epsilon);
            var finish = DateTime.Now;
            Console.WriteLine($"Time for nonparalel:{finish - start}\n");

            foreach (var x in threadNum)
            {
                double[] XParalel = new double[size];

                Console.WriteLine($"Starting paralel ({x} threads):");
                var start2 = DateTime.Now;
                JacobiParalel(size, generated["Matrix"], generated["Free elements"], ref XParalel, epsilon, x);
                var finish2 = DateTime.Now;
                Console.WriteLine($"Time for paralel ({x} threads):{finish2 - start2}");
                var speedup = (finish - start) / (finish2 - start2);
                Console.WriteLine($"Speedup: {speedup}");
                Console.WriteLine($"Efficiency: {speedup / x}");
                Console.WriteLine($"Cost: {x * (finish2- start2)}\n");
            }

            Console.ReadLine();
        }

        public static void JacobiNonparalel(int size, double[][] coefficients, double[] values, ref double[] X, double eps)
        {
            double[] previousX = new double[size];
            double err;

            do
            {
                err = 0;
                double[] newValues = new double[size];

                for (int i = 0; i < size; i++)
                {
                    newValues[i] = values[i];

                    for (int j = 0; j < size; j++)
                    {

                        if (i != j)
                        {
                            newValues[i] -= coefficients[i][j] * previousX[j];
                        }

                    }

                    newValues[i] = newValues[i] / coefficients[i][i];

                    if (Math.Abs(previousX[i] - newValues[i]) > err)
                    {
                        err = Math.Abs(previousX[i] - newValues[i]);
                    }
                }
                previousX = newValues;
            } while (err > eps);

            X = previousX;
        }

        public static void JacobiParalel(int size, double[][] coefficients, double[] values, ref double[] X, double eps, int threadNum)
        {
            double[] previousX = new double[size];
            double err;

            Thread[] threads = new Thread[threadNum];

            int[,] parameters = new int[threadNum, 2];


            int remainder = size % threadNum;
            int remainderPassed = 0;

            for (int q = 0; q < threadNum; q++)
            {
                int from = size / threadNum * q + remainderPassed;
                int to = size / threadNum * (q + 1) + remainderPassed;

                if (remainder > 0)
                {
                    remainder--;
                    remainderPassed++;
                    to++;
                }
                parameters[q, 0] = from;
                parameters[q, 1] = to;
            }

            do
            {
                err = 0;
                double[] newValues = new double[size];

                for (int q = 0; q < threadNum; q++)
                {
                    int toPass = q;
                    threads[q] = new Thread(v =>
                    {
                        for (int i = parameters[toPass, 0]; i < parameters[toPass, 1]; i++)
                        {
                            newValues[i] = values[i];

                            for (int j = 0; j < size; j++)
                            {

                                if (i != j)
                                {
                                    newValues[i] -= coefficients[i][j] * previousX[j];
                                }

                            }

                            newValues[i] = newValues[i] / coefficients[i][i];

                            if (Math.Abs(previousX[i] - newValues[i]) > err)
                            {
                                err = Math.Abs(previousX[i] - newValues[i]);
                            }
                        }
                    });
                }

                foreach (var item in threads)
                {
                    item.Start();
                }

                foreach (var item in threads)
                {
                    item.Join();
                }

                previousX = newValues;

            } while (err > eps);

            X = previousX;
        }

        public static Dictionary<string, dynamic> GenerateRandomLinearEquation(int vars)
        {
            Dictionary<string, dynamic> toReturn = new();

            var solutions = new double[vars];
            var freeElements = new double[vars];

            List<double[]> matrix = new();

            Random random = new();

            for (int i = 0; i < vars; i++)
                solutions[i] = random.Next(1, 100);

            for (int i = 0; i < vars; i++)
            {
                var toAdd = new double[vars];

                for (int j = 0; j < vars; j++)
                {
                    if (j == i)
                        toAdd[j] = random.Next(100 * vars, 200 * vars);
                    else
                        toAdd[j] = random.Next(1, 100);
                    freeElements[i] += toAdd[j] * solutions[j];
                }

                matrix.Add(toAdd);
            }

            toReturn["Matrix"] = matrix.ToArray();
            toReturn["Free elements"] = freeElements;
            toReturn["Solutions"] = solutions;
            return toReturn;
        }
        public static bool EqualResult(double[] first, double[] second)
        {
            if (first.Length != second.Length)
                return false;
            for (int i = 0; i < first.Length; i++)
            {
                if (Math.Round(first[i], 2) != Math.Round(second[i], 2))
                    return false;
            }
            return true;
        }
        public static void PrintMatrix(double[][] matrix)
        {
            if (matrix.Length != 0)
            {
                Console.Write("[");
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    Console.Write("[");
                    for (int j = 0; j < matrix[0].GetLength(0); j++)
                    {
                        Console.Write(matrix[i][j]);
                        if (matrix[0].GetLength(0) - 1 != j)
                            Console.Write(", ");
                    }
                    Console.Write("]");
                    if (matrix.GetLength(0) - 1 != i)
                        Console.WriteLine(", ");
                }
                Console.WriteLine("]");
            }
        }
        public static void PrintArray<T>(T[] arr)
        {
            Console.Write("[");
            Console.Write(String.Join(", ", arr));
            Console.WriteLine("]");
        }
    }
}
