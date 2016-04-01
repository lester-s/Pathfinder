using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pathfinder.Tools
{
    /// <summary>
    /// drawing tool
    /// http://www.c-sharpcorner.com/uploadfile/mahesh/polyline-in-wpf/
    /// I used this tutorial for the basics of canvas drawing
    /// </summary>
    public class Drawer : ITool
    {
        public CanvasHandler ToolCanvas { get; set; }

        public Drawer(CanvasHandler canvasHandler)
        {
            ToolCanvas = canvasHandler;
        }

        public void ExecuteMainAction(Point point)
        {
            ToolCanvas.Finder.SetPoint(point);
            DrawRectangle(point);
        }

        public void ExecuteMainAction(Square square)
        {
            DrawRectangle(square);
        }

        private void DrawRectangle(Point currentPoint)
        {
            var pos = currentPoint;
            var selectedSquare = ToolCanvas.Finder.GetSelectedSquare(currentPoint);

            if (selectedSquare == null)
            {
                return;
            }

            var myRect = new Rectangle
            {
                Stroke = Brushes.Black,
                Fill = Brushes.SkyBlue,
                Margin = new Thickness(selectedSquare.X, selectedSquare.Y, 0, 0),
                Height = ToolCanvas.SquareSize,
                Width = ToolCanvas.SquareSize
            };
            ToolCanvas.MyCanvas.Children.Add(myRect);
        }

        private void DrawRectangle(Square currentSquare)
        {
            var selectedSquare = currentSquare;

            if (selectedSquare == null)
            {
                return;
            }

            var myRect = new Rectangle
            {
                Stroke = Brushes.Red,
                Fill = Brushes.WhiteSmoke,
                Margin = new Thickness(selectedSquare.X, selectedSquare.Y, 0, 0),
                Height = ToolCanvas.SquareSize,
                Width = ToolCanvas.SquareSize
            };
            ToolCanvas.MyCanvas.Children.Add(myRect);
        }
    }
}