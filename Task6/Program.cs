﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Task6
{
    class Program
    {
        static void Main()
        {
            const int size = 10000;

            Console.WriteLine("Generating graph...");
            Graph test = new(size);
            Console.WriteLine("Graph generated!");

            NonParalelDejkstra(test, 0, out int[] resultNonPar, out TimeSpan resultTimeNonPar);

            int[] threadNums = new int[] { 2, 4, 6 };

            foreach (int x in threadNums)
            {
                ParalelDejkstra(test, 0, x, out int[] resultPar, out TimeSpan resultTimePar);
                if(!EqualResults(resultNonPar, resultPar))
                {
                    Console.WriteLine("A Mistake!");
                }
                else
                {
                    Console.WriteLine($"Speedup: { resultTimeNonPar / resultTimePar }");
                    Console.WriteLine($"Efficiency: { resultTimeNonPar / resultTimePar / x }");
                    Console.WriteLine($"Cost: { x * resultTimePar }");
                }
            }

            Console.ReadLine();
        }

        public static void ParalelDejkstra(Graph graph, int start, int threadNum, out int[] result, out TimeSpan resultTime)
        {
            var flag = true;
            ConcurrentQueue<Job> jobs = new();

            foreach (var x in graph.Points)
            {
                x.Passed = false;
                x.Weight = int.MaxValue;
            }

            Console.WriteLine($"Starting paralel Dejkstra for point {start}, with {threadNum} threads...");
            var startTime = DateTime.Now;

            Thread[] threads = new Thread[threadNum];
            CountdownEvent countdownEvent = new(threadNum);

            graph.Points[start].Weight = 0;
            GraphPoint current = graph.Points[start];

            for (int i = 0; i < threadNum; i++)
            {
                threads[i] = new Thread(() =>
                {
                    var threadNumber = i;
                    while (flag)
                    {
                        if (jobs.TryDequeue(out Job result))
                        {
                            result.Data
                            .GetRange(result.From, result.Take)
                            .ForEach(con =>
                            {
                                if (!con.SecondPoint.Passed)
                                {
                                    if (con.SecondPoint.Weight > current.Weight + con.Weight)
                                    {
                                        con.SecondPoint.Weight = current.Weight + con.Weight;
                                    }
                                }
                            });
                            countdownEvent.Signal();
                        }
                    }
                });
            }

            foreach (var x in threads)
                x.Start();

            while (current is not null)
            {
                int toEach = current.Connections.Count / threadNum;
                int remainder = current.Connections.Count % threadNum;

                for (int i = 0; i < threadNum; i++)
                {
                    int startCon = i * toEach;
                    int take = i + 1 != threadNum ? toEach : toEach + remainder;
                    Job job = new() { Data = current.Connections, From = startCon, Take = take };
                    jobs.Enqueue(job);
                }
                while(countdownEvent.CurrentCount != 0) { }
                countdownEvent.Reset();
                current.Passed = true;

                current = graph.Points
                    .Where(p => !p.Passed)
                    .OrderBy(p => p.Weight)
                    .FirstOrDefault();
            }

            flag = false;
            foreach (var x in threads)
                x.Join();

            var finishTime = DateTime.Now;
            resultTime = finishTime - startTime;
            Console.WriteLine($"Time for paralel Dejkstra with {threadNum} threads: {resultTime}");

            result = new int[graph.Points.Count];

            for (int i = 0; i < graph.Points.Count; i++)
            {
                result[i] = graph.Points[i].Weight;
            }
        }

        public class Job
        {
            public List<GraphConnection> Data { get; set; }
            public int From { get; set; }
            public int Take { get; set; }
        }

        public static void NonParalelDejkstra(Graph graph, int start, out int[] result, out TimeSpan resultTime)
        {
            foreach (var x in graph.Points)
            {
                x.Passed = false;
                x.Weight = int.MaxValue;
            }

            Console.WriteLine($"Starting nonparalel Dejkstra for point {start}...");
            var startTime = DateTime.Now;

            graph.Points[start].Weight = 0;
            GraphPoint current = graph.Points[start];

            while (current is not null)
            {
                current.Connections.ForEach(con =>
                {
                    if (!con.SecondPoint.Passed)
                    {
                        if (con.SecondPoint.Weight > current.Weight + con.Weight)
                        {
                            con.SecondPoint.Weight = current.Weight + con.Weight;
                        }
                    }
                });
                current.Passed = true;
                current = graph.Points
                    .Where(p=> !p.Passed)
                    .OrderBy(p => p.Weight)
                    .FirstOrDefault();
            }

            var finishTime = DateTime.Now;
            resultTime = finishTime - startTime;
            Console.WriteLine($"Time for nonparalel Dejkstra: {resultTime}");
            
            result = new int[graph.Points.Count];

            for (int i = 0; i < graph.Points.Count; i++)
            {
                result[i] = graph.Points[i].Weight;
            }
        }
        public static bool EqualResults(int[] res1, int[] res2)
        {
            if (res1.Length != res2.Length)
            {
                Console.WriteLine("Wrong result length");
                return false;
            }
            else
            {
                for(int i = 0; i < res1.Length; i++)
                {
                    if (res1[i] != res2[i])
                        return false;
                }
                return true;
            }
        }
    }

    public class GraphPoint
    {
        public string Name { get; set; }
        public bool Passed { get; set; }
        public List<GraphConnection> Connections { get; set; }
        public int Weight { get; set; }


        public GraphPoint(string name)
        {
            Passed = false;
            Weight = int.MaxValue;
            Name = name;
            Connections = new();
        }
        public override string ToString()
        {
            return $"{Name}\n\t{String.Join("\n\t", Connections)}";
        }
    }
    public class GraphConnection
    {
        public GraphPoint FirstPoint { get; set; }
        public GraphPoint SecondPoint { get; set; }
        public int Weight { get; set; }
        public override string ToString()
        {
            return $"{FirstPoint.Name} - {SecondPoint.Name}: {Weight}";
        }
    }
    public class Graph
    {
        public List<GraphPoint> Points { get; set; }
        public List<GraphConnection> Connections { get; set; }

        public Graph(int pointNum)
        {
            Points = new();
            Random random = new();
            for (int i = 0; i < pointNum; i++)
            {
                GraphPoint newPoint = new("Point" + i);
                int connection = 0;

                if (Points.Count != 0)
                {
                    connection = random.Next(0, Points.Count);
                    int weight = random.Next(1, 1000);
                    GraphConnection connectionFrom = new() { FirstPoint = Points[connection], SecondPoint = newPoint, Weight = weight };
                    GraphConnection connectionTo = new() { FirstPoint = newPoint, SecondPoint = Points[connection], Weight = weight };
                    newPoint.Connections.Add(connectionTo);
                    Points[connection].Connections.Add(connectionFrom);
                }

                foreach (var x in Points)
                {
                    if (random.NextDouble() < 0.5 && Points[connection].Name != x.Name)
                    {
                        int weight = random.Next(1, 1000);
                        GraphConnection connectionFrom = new() { FirstPoint = x, SecondPoint = newPoint, Weight = weight };
                        GraphConnection connectionTo = new() { FirstPoint = newPoint, SecondPoint = x, Weight = weight };
                        newPoint.Connections.Add(connectionTo);
                        x.Connections.Add(connectionFrom);
                    }
                }

                Points.Add(newPoint);
            }
        }
        public override string ToString()
        {
            return String.Join("\n\n", Points);
        }
        public void AdjacencyMatrix()
        {
            foreach(var x in Points)
            {
                foreach(var y in Points)
                {
                    if (x.Connections.Select(con => con.SecondPoint).Contains(y))
                    {
                        Console.Write($"{ x.Connections.First(con => con.SecondPoint.Name == y.Name).Weight }, ");
                    }
                    else
                    {
                        Console.Write("0, ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
