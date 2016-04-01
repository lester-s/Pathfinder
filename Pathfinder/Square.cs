using Pathfinder.AStarPathfinder;

namespace Pathfinder
{
    public class Square
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public AStarNode Node { get; set; }
        public Square Parent { get; set; }

        public Square()
        {
            Node = new AStarNode();
        }
    }
}