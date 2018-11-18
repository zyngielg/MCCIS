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

            

            g1.FillEdgesFromCsv("Graphs\\g3_1.csv");
            g2.FillEdgesFromCsv("Graphs\\g3_2.csv");

            McGregorAlgorithm mcGregorAlgorithm = new McGregorAlgorithm(g1, g2);
            var no = mcGregorAlgorithm.PerformMcGregorForVertices();
            Console.WriteLine("Number of vertices in MCCIS is: " + no);

            var no2 = mcGregorAlgorithm.PerformMcGregorForVerticesAndEdges();
            Console.WriteLine("Number of vertices in MCCIS is: " + no2);

            Console.WriteLine("Press any key ...");
            Console.ReadKey();

        }

    }
}
