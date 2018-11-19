using System;
using System.Collections.Generic;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class McGregorAlgorithm : IAlgorithm
    {
        private bool _onlyVertices;

        public McGregorAlgorithm(bool onlyVertices)
        {
            _onlyVertices = onlyVertices;
        }
        public (int[], int[]) GetMaximalCommonSubgraphMapping(string g1Path, string g2Path)
        {
            throw new NotImplementedException();
        }
    }
}
