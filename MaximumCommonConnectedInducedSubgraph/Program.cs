using System;
using System.Collections.Generic;

namespace MaximumCommonConnectedInducedSubgraph
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g1 = new Graph();
            Graph g2 = new Graph();
            g1.FillEdgesFromCsv("Graphs\\g1_1.csv");
            g2.FillEdgesFromCsv("Graphs\\g1_2.csv");

            McGregorAlgorithm mcGregorAlgorithm = new McGregorAlgorithm(g1, g2);
            var no = mcGregorAlgorithm.PerformMcGregorForVertices();
            Console.WriteLine("Number of vertices in MCCIS is: " + no);

            Console.WriteLine("Press any key ...");
            Console.ReadKey();
            //Save();

        }

        //static void Save()
        //{
        //    List<(int, int)> tempList = new List<(int, int)>();
        //    foreach (var el in Function())
        //    {
        //        tempList.Add(el);
        //    }
        //    int aa = 0;
        //}

        //static IEnumerable<(int,int)> Function()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        for (int j = 0; j < 6; j++)
        //        {
        //            yield return (i, j);
        //        }
        //    }
        //}
    }
}
