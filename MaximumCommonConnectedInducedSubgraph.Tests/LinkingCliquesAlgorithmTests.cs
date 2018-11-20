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
            for (int i = 0; i < cliquesList.Count; i++)
            {
                cliquesList[i].Sort();
            }
            #endregion

            #region Then
            Assert.Equal(expectedResult.Count, cliquesList.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                for (int j = 0; j < expectedResult[i].Count; j++)
                {
                    Assert.Equal(expectedResult[i][j], cliquesList[i][j]);
                }
            }
            #endregion
        }

        [Theory]
        [InlineData("b2_unfortunate_numerating")]
        [InlineData("b1")]
        public void TwoSameGraphsAsConnectedCliques(string name)
        {
            #region Give
            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + name + ".csv";

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

        [Fact]
        public void TwoDifferentGraphsAsConnectedCliquesUnfortunateVertices()
        {
            #region Give
            var g1Name = "b1";
            var g1ExpectedResult = new int[] { 4, 5, 6 };
            var g2Name = "b2_unfortunate_numerating";
            var g2ExpectedResult = new int[] { 2, 5, 4 };

            var g1Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g1Name + ".csv";
            var g2Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g2Name + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);
            #endregion

            #region Then
            Assert.Equal(x1.Length, x2.Length);
            for (int i = 0; i < x1.Length; i++)
            {
                Assert.Equal(g1ExpectedResult[i], x1[i]);
                Assert.Equal(g2ExpectedResult[i], x2[i]);
            }
            #endregion
        }

        [Fact]
        public void TwoDifferentGraphsAsConnectedCliquesFortunateVertices()
        {
            #region Give
            var g1Name = "b1";
            var g1ExpectedResult = new int[] { 4, 5, 6, 7, 8 };
            var g2Name = "b3_fortunate_numerating";
            var g2ExpectedResult = new int[] { 2, 5, 4, 3, 1 };

            var g1Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g1Name + ".csv";
            var g2Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g2Name + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);
            #endregion

            #region Then
            Assert.Equal(x1.Length, x2.Length);
            for (int i = 0; i < x1.Length; i++)
            {
                Assert.Equal(g1ExpectedResult[i], x1[i]);
                Assert.Equal(g2ExpectedResult[i], x2[i]);
            }
            #endregion
        }

        [Fact]
        public void TwoDifferentGraphsAsConnectedCliquesWithoutCommonCliques()
        {
            #region Give
            var g1Name = "b4_k3_with_k1";
            var g2Name = "b5_k4_with_k2";

            var g1Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g1Name + ".csv";
            var g2Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g2Name + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);
            #endregion

            #region Then
            Assert.Empty(x1);
            Assert.Empty(x2);
            #endregion
        }

        [Fact]
        public void SameCliquesConnectedWithBridge()
        {
            #region Give
            var gName = "b6_k3_bridge_k3";
            var gExpectedResult = new int[] { 1, 0, 2, 3, 4, 5, 6 };

            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + gName + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(gPath, gPath);
            #endregion

            #region Then
            Assert.Equal(gExpectedResult.Length, x1.Length);
            Assert.Equal(gExpectedResult.Length, x2.Length);

            for(int i=0; i< gExpectedResult.Length; i++)
            {
                Assert.Equal(gExpectedResult[i], x1[i]);
                Assert.Equal(gExpectedResult[i], x2[i]);
            }

            #endregion
        }

        [Fact]
        public void k3_bridge_k3_and_k3_bridge_k4()
        {
            #region Give
            var g1Name = "b6_k3_bridge_k3";
            var g2Name = "b7_k3_bridge_k4";

            var gExpectedResult = new int[] { 1, 0, 2, 3 };

            var g1Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g1Name + ".csv";
            var g2Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g2Name + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);
            #endregion

            #region Then
            Assert.Equal(gExpectedResult.Length, x1.Length);
            Assert.Equal(gExpectedResult.Length, x2.Length);

            for (int i = 0; i < gExpectedResult.Length; i++)
            {
                Assert.Equal(gExpectedResult[i], x1[i]);
                Assert.Equal(gExpectedResult[i], x2[i]);
            }

            #endregion
        }

        [Fact]
        public void A1A2()
        {
            #region Give
            var g1Name = "a1";
            var g2Name = "a2";

            var gExpectedResult = new int[] { 0 };

            var g1Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g1Name + ".csv";
            var g2Path = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\" + g2Name + ".csv";

            var alg = new LinkingCliquesAlgorithm();
            #endregion

            #region When
            int[] x1;
            int[] x2;
            (x1, x2) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);
            #endregion

            #region Then
            Assert.Equal(gExpectedResult.Length, x1.Length);
            Assert.Equal(gExpectedResult.Length, x2.Length);

            for (int i = 0; i < gExpectedResult.Length; i++)
            {
                Assert.Equal(gExpectedResult[i], x1[i]);
                Assert.Equal(gExpectedResult[i], x2[i]);
            }

            #endregion
        }


    }
}
