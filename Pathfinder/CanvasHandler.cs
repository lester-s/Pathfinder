using Pathfinder.AStarPathfinder;
using Pathfinder.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pathfinder
{
    public class CanvasHandler
    {
        public List<ITool> Tools { get; set; }
        private ITool CurrentTool { get; set; }
        public Canvas MyCanvas { get; set; }

        public int TileAmount { get; set; }
        public double SquareSize { get; set; }
        public Square[,] TilesArray { get; set; }
        public IPathfinder Finder { get; set; }

        public CanvasHandler(Canvas canvas)
        {
            MyCanvas = canvas;

            InitTiles();

            Finder = new MyAstarPathfinder(this);
            Finder.OnReadyToProcess += Finder_OnReadyToProcess;

            Tools = new List<ITool>();
            Tools.Add(new Drawer(this));
            CurrentTool = Tools.First(t => t.GetType() == typeof(Drawer));
        }

        private void InitTiles()
        {
            //you can change this variable for a more or less accurate grid on the canvas
            TileAmount = 50;
            TilesArray = new Square[TileAmount, TileAmount];
            SquareSize = MyCanvas.Height / TileAmount;

            for (int i = 0; i < TileAmount; i++)
            {
                for (int j = 0; j < TileAmount; j++)
                {
                    var square = new Square { X = j * SquareSize, Y = i * SquareSize, XIndex = j, YIndex = i };
                    TilesArray[j, i] = square;
                }
            }
        }

        internal void Clear()
        {
            Finder.Reset();
            MyCanvas.Children.Clear();
        }

        public void ExecuteToolAction(Point point)
        {
            CurrentTool.ExecuteMainAction(point);
        }

        public void ExecuteToolAction(Square square)
        {
            CurrentTool.ExecuteMainAction(square);
        }

        internal void ChangeTool<T>() where T : ITool
        {
            CurrentTool = Tools.First(t => t.GetType() == typeof(T));
        }

        private void Finder_OnReadyToProcess()
        {
            var path = Finder.ProcessPath();
            foreach (var tile in path)
            {
                ExecuteToolAction(tile);
            }
        }
    }
}