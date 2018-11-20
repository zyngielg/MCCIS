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
            
            g1.FillEdgesFromCsv("Graphs\\Circles of 5 and 6 vertices-1.csv");
            g2.FillEdgesFromCsv("Graphs\\Circles of 5 and 6 vertices-2.csv");

            (int[], int[]) mappingV;
            (int[], int[]) mappingVE;

            McGregorAlgorithm mcGregorAlgorithm = new McGregorAlgorithm(g1, g2);
            var no = mcGregorAlgorithm.PerformMcGregorForVertices(out mappingV);
            Console.WriteLine("Number of vertices in MCCIS is: " + no);

            var no2 = mcGregorAlgorithm.PerformMcGregorForVerticesAndEdges(out mappingVE);
            Console.WriteLine("Number of vertices in MCCIS is: " + mappingVE.Item1.Length);

            Console.WriteLine("Press any key ...");
            Console.ReadKey();

        }

    }
}
