using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MaximumCommonConnectedInducedSubgraph.Tests
{

    public class MaxCliqueDetectionTests
    {
        [Fact]
        public void MaxCliqueDetectionTwiceSameGraph()
        {
            var expectedResult = new List<int> { 1, 2, 4 };

            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a2.csv";


            var g = new Graph();

            g.FillEdgesFromCsv(gPath);

            var alg = new ModularGraphMaxCliqueAlgorithm();

            alg.MaxCliquePolynomial(g);

            #region Then
            Assert.Equal(expectedResult.Count, alg._maxCP.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i], alg._maxCP[i]);
            }
            #endregion
        }

        [Fact]
        public void MaxCliqueDetectionSingleVertexSingleVertex()
        {
            var expectedResult = new List<int> { 0 };

            var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a3_single_vertex.csv";

            var g = new Graph();

            g.FillEdgesFromCsv(gPath);

            var alg = new ModularGraphMaxCliqueAlgorithm();

            alg.MaxCliquePolynomial(g);

            #region Then
            Assert.Equal(expectedResult.Count, alg._maxCP.Count);
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i], alg._maxCP[i]);
            }
            #endregion
        }

        [Fact]
        public void MaxCliqueDetectionSingleVertexNormalGraph()
        {
            var expectedResult = new List<int> { 0 };

            var gPath1 = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a3_single_vertex.csv";
            var gPath2 = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a2.csv";

            var g1 = new Graph();
            var g2 = new Graph();

            g1.FillEdgesFromCsv(gPath1);
            g2.FillEdgesFromCsv(gPath2);

            var alg = new ModularGraphMaxCliqueAlgorithm();
            int[] g1Mapping;
            int[] g2Mapping;
            (g1Mapping, g2Mapping) = alg.GetMaximalCommonSubgraphMapping(gPath1, gPath2);

            #region Then
            Assert.Equal(expectedResult.Count, g1Mapping.Length);
            Assert.Equal(expectedResult.Count, g2Mapping.Length);
            Assert.Equal(g1Mapping[0], g2Mapping[0]);
            #endregion
        }

        [Fact]
        public void MaxCliqueDetectionG1G3()
        {
            var expectedResult = new List<int> { 0, 2, 3 };
            var expectedResult2 = new List<int> { 2, 1, 4 };

            var gPath1 = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a1.csv";
            var gPath2 = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\a2.csv";
         
            var alg = new ModularGraphMaxCliqueAlgorithm();
            int[] g1Mapping;
            int[] g2Mapping;
            (g1Mapping, g2Mapping) = alg.GetMaximalCommonSubgraphMapping(gPath1, gPath2);

            #region Then
            Assert.Equal(expectedResult.Count, g1Mapping.Length);
            Assert.Equal(expectedResult2.Count, g2Mapping.Length);

            for(int i=0; i< expectedResult.Count; i++)
            {
                Assert.Equal(expectedResult[i], g1Mapping[i]);
                Assert.Equal(expectedResult2[i], g2Mapping[i]);
            }
            #endregion
        }
    }
}
