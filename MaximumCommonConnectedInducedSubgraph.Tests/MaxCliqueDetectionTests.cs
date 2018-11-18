using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MaximumCommonConnectedInducedSubgraph.Tests
{

    public class MaxCliqueDetectionTests
    {
        [Theory]
        [InlineData("\\..\\..\\..\\Graphs\\a2.csv")]
        public void MaxCliqueDetectionSingleGraph(string gName)
        {
            var expectedResult = new List<int> { 1, 2, 4 };

            var gPath = Environment.CurrentDirectory + gName;


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
    }
}
