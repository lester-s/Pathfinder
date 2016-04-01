using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Pathfinder.AStarPathfinder
{
    /// <summary>
    /// https://www.raywenderlich.com/4946/introduction-to-a-pathfinding
    /// http://heyes-jones.com/astar.php
    /// http://www.gamedev.net/page/resources/_/technical/artificial-intelligence/a-pathfinding-for-beginners-r2003
    ///
    /// I have been using those three links to have a good understanding of the A star pathfinding
    /// </summary>
    public class MyAstarPathfinder : IPathfinder
    {
        private List<Square> openNodes;
        private List<Square> closeNodes;
        private List<Square> closeNodesSave;
        private List<Square> currentPath;

        private readonly CanvasHandler canvasHandler;

        public event Action OnReadyToProcess;

        public MyAstarPathfinder(CanvasHandler canvas)
        {
            this.canvasHandler = canvas;
            Reset();
        }

        public void Reset()
        {
            closeNodes = new List<Square>();
            closeNodesSave = new List<Square>();
            openNodes = new List<Square>();
            currentPath = new List<Square>();
        }

        public void SetPoint(Point point)
        {
            if (currentPath.Count < 2)
            {
                currentPath.Add(GetSelectedSquare(point));
            }

            if (currentPath.Count == 2)
            {
                var handler = OnReadyToProcess;
                if (handler != null)
                    handler();
            }
        }

        /// <summary>
        /// The path is process with the AStar pathfinding algorithm
        /// </summary>
        /// <returns>list of the square in the path</returns>
        public List<Square> ProcessPath()
        {
            var endingTile = currentPath.ElementAt(1);
            //we reload in the close nodes the previous paths.
            closeNodes.AddRange(closeNodesSave);
            openNodes.Add(currentPath.ElementAt(0));
            var result = new List<Square>();

            //if there is no more open nodes, there is no path.
            while (openNodes.Count > 0)
            {
                //ending tile is reached, the path is complete we can stop
                if (closeNodes.Contains(endingTile))
                {
                    break;
                }

                var currentSquare = openNodes.OrderBy(s => s.Node.Score).ElementAt(0);
                openNodes.Remove(currentSquare);
                closeNodes.Add(currentSquare);

                var adjacentSquares = GetAdjacentSquares(currentSquare);
                //we process score of the adjacents squares to find the best one for the path
                foreach (var adjacent in adjacentSquares)
                {
                    //already in use in a path
                    if (closeNodes.Contains(adjacent))
                    {
                        continue;
                    }

                    if (!openNodes.Contains(adjacent))
                    {
                        //the parent is used to rebuild the path in the end.
                        adjacent.Parent = currentSquare;
                        CalculateTileScore(currentSquare, endingTile, adjacent);
                        openNodes.Add(adjacent);
                    }
                    else
                    {
                        if (adjacent.Node.CostFromStart < currentSquare.Node.CostFromStart + 1)
                        {
                            adjacent.Parent = currentSquare;
                            CalculateTileScore(currentSquare, endingTile, adjacent);
                        }
                    }
                }
            }

            //now we rebuild the path from the end thanks to the parent attribute.
            result.Add(endingTile);
            var current = endingTile;

            if (openNodes.Count > 0)
            {
                do
                {
                    result.Add(current);
                    //closeNodeSave is used to save the previously drawn path
                    closeNodesSave.Add(current);
                    current = current.Parent ?? current;
                } while (current != currentPath.ElementAt(0));
            }

            closeNodesSave.Add(currentPath.ElementAt(0));

            //we clear the finder to get ready for the next path
            ClearFinder();
            return result;
        }

        public Square GetSelectedSquare(Point currentPoint)
        {
            var xIndex = (int)Math.Ceiling(currentPoint.X / canvasHandler.SquareSize);
            var yIndex = (int)Math.Ceiling(currentPoint.Y / canvasHandler.SquareSize);

            return canvasHandler.TilesArray[xIndex <= 0 ? xIndex : xIndex - 1, yIndex <= 0 ? yIndex : yIndex - 1];
        }

        private void ClearFinder()
        {
            closeNodes.Clear();
            currentPath.Clear();
            openNodes.Clear();
        }

        private List<Square> GetAdjacentSquares(Square currentSquare)
        {
            var result = new List<Square>();

            //getting bottom square
            if (currentSquare.YIndex < canvasHandler.TileAmount - 1)
            {
                var tile = canvasHandler.TilesArray[currentSquare.XIndex, currentSquare.YIndex + 1];
                if (!closeNodes.Contains(tile))
                {
                    result.Add(tile);
                }
            }

            //getting right quare
            if (currentSquare.XIndex < canvasHandler.TileAmount - 1)
            {
                var tile = canvasHandler.TilesArray[currentSquare.XIndex + 1, currentSquare.YIndex];
                if (!closeNodes.Contains(tile))
                {
                    result.Add(tile);
                }
            }

            //getting up square
            if (currentSquare.YIndex > 0)
            {
                var tile = canvasHandler.TilesArray[currentSquare.XIndex, currentSquare.YIndex - 1];
                if (!closeNodes.Contains(tile))
                {
                    result.Add(tile);
                }
            }

            //getting left square
            if (currentSquare.XIndex > 0)
            {
                var tile = canvasHandler.TilesArray[currentSquare.XIndex - 1, currentSquare.YIndex];
                if (!closeNodes.Contains(tile))
                {
                    result.Add(tile);
                }
            }

            return result;
        }

        private void CalculateTileScore(Square origineTile, Square endingTile, params Square[] tilesToScore)
        {
            foreach (var tile in tilesToScore)
            {
                tile.Node.CostFromStart = origineTile.Node.CostFromStart + 1;
                tile.Node.CostToEnd = CalculateCostToEnd(tile, endingTile);
            }
        }

        private int CalculateCostToEnd(Square tile, Square endingTile)
        {
            var xScore = Math.Abs(endingTile.XIndex - tile.XIndex);
            var yScore = Math.Abs(endingTile.YIndex - tile.YIndex);
            var result = xScore + yScore;
            return result;
        }
    }
}