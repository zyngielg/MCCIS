namespace MaximumCommonConnectedInducedSubgraph
{
    public interface IAlgorithm
    {
        (int[], int[]) GetMaximalCommonSubgraphMapping(string g1Path, string g2Path);
    }
}
