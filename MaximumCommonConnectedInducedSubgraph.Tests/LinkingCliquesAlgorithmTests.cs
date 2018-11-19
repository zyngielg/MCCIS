using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MaximumCommonConnectedInducedSubgraph.Tests
{
    public class LinkingCliquesAlgorithmTests
    {
        [Fact]
        public void GetGraphCliquesLinkedCliques()
        {
            #region Give
            var expectedResult = new List<List<int>>
            {
                new List<int> { 0, 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8 },
                new List<int> { 9 }
            };
            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\b1.csv";

            var alg = new LinkingCliquesAlgorithm();
            var g = new Graph();
            g.FillEdgesFromCsv(gPath);
            #endregion

            #region When
            var cliquesList = new List<List<int>>();
            alg.GetGraphCliques(g, cliquesList);
            for(int i=0; i< cliquesList.Count; i++)
            {
                cliquesList[i].Sort();
            }
            #endregion

            #region Then
            Assert.Equal(expectedResult.Count, cliquesList.Count);
            for(int i=0; i< expectedResult.Count; i++)
            {
                for(int j=0; j<expectedResult[i].Count; j++)
                {
                    Assert.Equal(expectedResult[i][j], cliquesList[i][j]);
                }
            }
            #endregion
        }

        [Fact]
        public void TestTemplate()
        {
            #region Give
            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\b2.csv";

            var alg = new LinkingCliquesAlgorithm();
            var g = new Graph();
            g.FillEdgesFromCsv(gPath);
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(gPath, gPath);
            #endregion

            #region Then

            #endregion
        }
    }
}
