using System;
using System.Collections.Generic;
using Xunit;


namespace MaximumCommonConnectedInducedSubgraph.Tests.ModularGraphMaxCliqueAlgorithmTests
{
    public class ModularGraphGenerationTests
    {
        [Theory]
        [InlineData("\\..\\..\\..\\Graphs\\a1.csv", "\\..\\..\\..\\Graphs\\a2.csv")]
        public void ProperModularGraphConstruction(string g1Name, string g2Name)
        {
            #region Give
            var g1Path = System.Environment.CurrentDirectory + g1Name;
            var g2Path = System.Environment.CurrentDirectory + g2Name;

            var g1 = new Graph();
            var g2 = new Graph();
            g1.FillEdgesFromCsv(g1Path);
            g2.FillEdgesFromCsv(g2Path);

            var expectedGraphPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\half_of_a1_x_a2.csv";
            var expectedResult = new Graph();
            expectedResult.FillEdgesFromCsv(expectedGraphPath, true);

            var alg = new ModularGraphMaxCliqueAlgorithm(g1, g2);
            #endregion

            #region When
            alg.CreateModularGraph();
            var result = alg._modularGraph;
            #endregion

            expectedResult.PrintGraph();

            #region Then
            Assert.Equal(expectedResult.Size, result.Size);

            List<KeyValuePair<int, int>> x = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < expectedResult.Size; i++)
            {
                for (int j = 0; j < expectedResult.Size; j++)
                {
                    Assert.Equal(expectedResult.GraphData[i, j], result.GraphData[i, j]);
                }
            }

            #endregion
        }
    }
}
