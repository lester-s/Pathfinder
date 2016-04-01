namespace Pathfinder.AStarPathfinder
{
    /// <summary>
    /// the basic node for the Astar pathfinding algorithm
    /// </summary>
    public class AStarNode
    {
        public int Score
        {
            get
            {
                return CostFromStart + CostToEnd;
            }
        }

        public int CostFromStart { get; set; }
        public int CostToEnd { get; set; }
    }
}