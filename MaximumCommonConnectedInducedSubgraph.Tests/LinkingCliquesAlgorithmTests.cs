﻿using System;
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

        [Theory]
        [InlineData("b2")]
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
        public void TwoDifferentGraphsAsConnectedCliques()
        {
            #region Give
            var g1Name = "b1";
            var g2Name = "b2";

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
                Assert.Equal(x1[i], x2[i]);
            }
            #endregion
        }
        //[Fact] 
        //public void TwoDifferentGraphsAsConnectedCliquesWithoutCommonCliques()
        //{
        //    #region Give
        //    var gPath = Environment.CurrentDirectory + "\\..\\..\\..\\Graphs\\b2.csv";

        //    var alg = new LinkingCliquesAlgorithm();
        //    var g = new Graph();
        //    g.FillEdgesFromCsv(gPath);
        //    #endregion

        //    #region When
        //    int[] x1;
        //    int[] x2;
        //    (x1, x2) = alg.GetMaximalCommonSubgraphMapping(gPath, gPath);
        //    #endregion

        //    #region Then

        //    #endregion
        //}



    }
}
