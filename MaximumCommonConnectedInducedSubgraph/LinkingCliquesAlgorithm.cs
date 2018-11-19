using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class LinkingCliquesAlgorithm : IAlgorithm
    {
        private Graph _g1;
        private Graph _g2;

        private List<List<int>> _g1Cliques;
        private List<List<int>> _g2Cliques;
        public List<KeyValuePair<int, int>> cliquesMapping;
        public List<KeyValuePair<int, int>> verticesMappings;

        public ModularGraphMaxCliqueAlgorithm mgAlg;
        public LinkingCliquesAlgorithm()
        {
            _g1 = new Graph();
            _g2 = new Graph();

            _g1Cliques = new List<List<int>>();
            _g2Cliques = new List<List<int>>();

            cliquesMapping = new List<KeyValuePair<int, int>>();
            verticesMappings = new List<KeyValuePair<int, int>>();
        }

        public (int[], int[]) GetMaximalCommonSubgraphMapping(string g1Path, string g2Path)
        {
            _g1.FillEdgesFromCsv(g1Path);
            _g2.FillEdgesFromCsv(g2Path);

            GetGraphCliques(new Graph((int[,])_g1.GraphData.Clone()), _g1Cliques);
            GetGraphCliques(new Graph((int[,])_g1.GraphData.Clone()), _g2Cliques);

            bool[] g1UsedCliques = new bool[_g1Cliques.Count];
            bool[] g2UsedCliques = new bool[_g2Cliques.Count];

            // TODO: check if swap works
            if (_g2Cliques[0].Count < _g1Cliques[0].Count)
            {
                var x = new List<List<int>>(_g2Cliques);
                _g2Cliques = new List<List<int>>(_g1Cliques);
                _g1Cliques = new List<List<int>>(x);
            }

            //_g1Clique[0] < _g2Clique[0]
            var alreadyMapped = false;
            // TODO: optimalization: remove from g2cliques those which are bigger than current g1clique[i]
            int currentG2 = 0;

            #region Sorting vertices in cliques by degree descending
            for (int i = 0; i < _g1Cliques.Count; i++)
            {
                _g1Cliques[i].Sort((a, b) => _g1.verticesDegrees[a].CompareTo(_g2.verticesDegrees[b]) * -1);
            }
            for (int i = 0; i < _g2Cliques.Count; i++)
            {
                _g2Cliques[currentG2].Sort((a, b) => _g1.verticesDegrees[a].CompareTo(_g2.verticesDegrees[b]) * -1);
            }
            #endregion

            for (int i = 0; i < _g1Cliques.Count; i++)
            {
                for (int j = 0; j < _g2Cliques.Count; j++)
                {
                    if (_g1Cliques[i].Count < _g2Cliques[j].Count)
                    {
                        g2UsedCliques[j] = true;
                    }
                    else if (_g1Cliques[i].Count == _g2Cliques[j].Count)
                    {
                        cliquesMapping.Add(new KeyValuePair<int, int>(i, j));
                        currentG2 = j;
                        g2UsedCliques[currentG2] = true;
                        alreadyMapped = true;
                        break;
                    }
                }
                if (!alreadyMapped)
                {
                    g1UsedCliques[i] = true;
                }
                else
                {
                    var cliqueG1 = _g1Cliques[i];


                    Dictionary<int, KeyValuePair<int, int>> degreePerVertices = new Dictionary<int, KeyValuePair<int, int>>();

                    //// mapping vertices
                    //for (int j = 0; j < _g1Cliques[i].Count; j++)
                    //{
                    //    var minMaxDegree = _g1Cliques[i][j] < _g2Cliques[currentG2][j] ? _g1Cliques[i][j] : _g1Cliques[i][j];
                    //    degreePerVertices.Add(minMaxDegree, new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentG2][j]));
                    //    verticesMappings.Add(new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentG2][j]));
                    //}


                    for (int k = 0; k < cliqueG1.Count; k++) // for every vertex in g1 maxclique
                    {
                        var vertexG1 = cliqueG1[k];
                        var freeDegreesOfV = _g1.verticesDegrees[cliqueG1[k]] - cliqueG1.Count + 1;
                        //if (freeDegreesOfV > 0)

                        for (int l = 0; l < freeDegreesOfV; l++) // for every free degree of the vertex
                        {
                            var goForNextDegree = false;

                            for (int m = i + 1; m < _g1Cliques.Count; m++) // iterating through cliques smaller than current clique
                            {
                                var cliqueG1toMatch = _g1Cliques[m];
                                foreach (var vertexG1toMatch in cliqueG1toMatch) // for each vertex of smaller clique
                                {
                                    // if there is a match in G1, we look for the same in g2
                                    if (_g1.GraphData[vertexG1, vertexG1toMatch] > 0)
                                    {
                                        var cliqueG2 = _g2Cliques[currentG2]; // current max clique from G2

                                        foreach (var vertexG2 in cliqueG2) // for each vertex in max clique from g2
                                        {
                                            for (int n = currentG2 + 1; n < _g2Cliques.Count; n++) // iterating through cliques smaller than current clique
                                            {
                                                if (g2UsedCliques[n]) continue;

                                                var cliqueG2toMatch = _g2Cliques[n];

                                                foreach (var vertexG2toMatch in cliqueG2toMatch) // for every vertex in clique G2
                                                {
                                                    if (_g2.GraphData[vertexG2, vertexG2toMatch] > 0)
                                                    {
                                                        verticesMappings.Add(new KeyValuePair<int, int>(k, currentG2));
                                                        verticesMappings.Add(new KeyValuePair<int, int>(vertexG1toMatch, vertexG2toMatch));
                                                        goForNextDegree = true;
                                                    }
                                                }
                                                if (goForNextDegree) break;
                                            }
                                        }


                                    }
                                    if (goForNextDegree) break;
                                }
                                if (goForNextDegree) break;
                            }
                        }
                    }
                }
            }

            //Dictionary<int, KeyValuePair<int, int>> degreePerVertices = new Dictionary<int, KeyValuePair<int, int>>();

            //// mapping vertices
            //for (int j = 0; j < _g1Cliques[i].Count; j++)
            //{
            //    var minMaxDegree = _g1Cliques[i][j] < _g2Cliques[currentg2][j] ? _g1Cliques[i][j] : _g1Cliques[i][j];
            //    degreePerVertices.Add(minMaxDegree, new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentg2][j]));
            //    verticesMappings.Add(new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentg2][j]));
            //}
            throw new NotImplementedException();
        }

        public void GetGraphCliques(Graph g, List<List<int>> cliques)
        {
            int[,] graphData = (int[,])g.GraphData.Clone();
            Graph gCp = new Graph(g.GraphData);

            List<int> clique;

            int sizeToDelete = 0;

            List<int> vertices = new List<int>();

            while (gCp.AnyEdgesLeft())
            {
                mgAlg = new ModularGraphMaxCliqueAlgorithm();

                mgAlg.MaxCliquePolynomial(gCp);
                clique = new List<int>();

                for (int i = 0; i < mgAlg._maxCP.Count; i++)
                {
                    clique.Add(mgAlg._maxCP[i]);
                    vertices.Add(mgAlg._maxCP[i]);
                }
                sizeToDelete += clique.Count;

                cliques.Add(clique);

                for (int i = 0; i < mgAlg._maxCP.Count; i++)
                {
                    for (int j = 0; j < g.GraphData.GetLength(0); j++)
                    {
                        graphData[mgAlg._maxCP[i], j] = 0;
                    }
                }

                for (int i = 0; i < g.GraphData.GetLength(0); i++)
                {
                    for (int j = 0; j < mgAlg._maxCP.Count; j++)
                    {
                        graphData[i, mgAlg._maxCP[j]] = 0;
                    }
                }
                gCp = new Graph((int[,])graphData.Clone());
                gCp.Size -= sizeToDelete;
            }

            vertices.Sort((a, b) => a.CompareTo(b));
            // for single vertices
            if (vertices.Count != g.GraphData.GetLength(0))
            {
                for (int i = 0; i < g.GraphData.GetLength(0); i++)
                {
                    if (!vertices.Contains(i))
                    {
                        cliques.Add(new List<int> { i });
                    }
                }
            }
        }
    }
}
