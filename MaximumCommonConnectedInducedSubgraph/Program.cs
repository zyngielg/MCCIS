using System;
using System.Collections.Generic;

namespace MaximumCommonConnectedInducedSubgraph
{
    class Program
    {
        static void Main(string[] args)
        {
            var file1 = "Graphs\\g5_1.csv";
            var file2 = "Graphs\\g5_2.csv";

            (int[], int[]) mappingV;
            (int[], int[]) mappingVE;

            var vAlgo = new McGregorAlgorithm(true);
            var veAlgo = new McGregorAlgorithm(false);

            mappingV = vAlgo.GetMaximalCommonSubgraphMapping(file1, file2);
            Console.WriteLine("Vertices in MCCIS : " + mappingV.ToString());

            mappingVE = veAlgo.GetMaximalCommonSubgraphMapping(file1, file2);
            Console.WriteLine("Vertices+edges in MCCIS: " + mappingVE.ToString());

            Console.WriteLine("Press any key ...");
            Console.ReadKey();

        }

    }
}
