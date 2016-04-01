using System;
using System.Collections.Generic;
using System.Windows;

namespace Pathfinder.AStarPathfinder
{
    /// <summary>
    /// This interface implements the minimum method for a new pathfinder to be used
    /// </summary>
    public interface IPathfinder
    {
        void SetPoint(Point point);

        List<Square> ProcessPath();

        void Reset();

        event Action OnReadyToProcess;

        Square GetSelectedSquare(Point currentPoint);
    }
}