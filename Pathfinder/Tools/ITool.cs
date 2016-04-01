using System.Windows;

namespace Pathfinder.Tools
{
    /// <summary>
    /// This is the base interface for a tool. It must be implemented to create a new tool
    /// http://www.dofactory.com/net/state-design-pattern
    /// I used this link to well implement the state pattern.
    /// </summary>
    public interface ITool
    {
        void ExecuteMainAction(Point point);

        void ExecuteMainAction(Square square);

        CanvasHandler ToolCanvas { get; set; }
    }
}