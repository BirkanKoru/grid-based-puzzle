using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GridOperations
{
    private GridController gridController;

    public GridOperations(GridController gridController)
    {
        this.gridController = gridController;
    }

    // Enum for directional indexing
    public enum Direction
    {
        Left = 1,
        Up = 3,
        Right = 5,
        Down = 7,
        LeftUp = 2,
        RightUp = 4,
        RightDown = 6,
        LeftDown = 8
    }

    /// <summary>
    /// Finds all matching points of the same color and breakables starting from a given grid point.
    /// </summary>
    public List<GridPoint[]> GetMatches(GridPoint gridPoint)
    {
        var matchedPoints = new HashSet<GridPoint>();
        var matchedBreakables = new HashSet<GridPoint>();

        var currentQueue = new Queue<GridPoint>();
        currentQueue.Enqueue(gridPoint);
        matchedPoints.Add(gridPoint);

        while (currentQueue.Count > 0)
        {
            var currentPoint = currentQueue.Dequeue();

            var neighborGroups = GetSameColorNeighbourPoints(currentPoint, new[] { Direction.Left, Direction.Up, Direction.Right, Direction.Down });

            foreach (var match in neighborGroups[0])
            {
                if (matchedPoints.Add(match)) // Adds only if not already in the set
                {
                    currentQueue.Enqueue(match);
                }
            }

            foreach (var breakable in neighborGroups[1])
            {
                matchedBreakables.Add(breakable);
            }
        }

        return new List<GridPoint[]> { matchedPoints.ToArray(), matchedBreakables.ToArray() };
    }

    /// <summary>
    /// Returns same-color neighbors and breakable neighbors of a grid point.
    /// </summary>
    public List<GridPoint[]> GetSameColorNeighbourPoints(GridPoint gridPoint, Direction[] directions)
    {
        var neighbors = GetPointNeighbours(gridPoint, directions);

        var sameColorPoints = new List<GridPoint>();
        var breakablePoints = new List<GridPoint>();

        foreach (var neighbor in neighbors)
        {
            if (neighbor.CurrState == GameConstants.GridPointState.Full)
            {
                if (neighbor.CurrItem.CurrItemModel.itemName == gridPoint.CurrItem.CurrItemModel.itemName)
                {
                    sameColorPoints.Add(neighbor);
                }
                else if (neighbor.CurrItem.CurrItemModel.itemType == LEItemType.Breakable)
                {
                    breakablePoints.Add(neighbor);
                }
            }
        }

        return new List<GridPoint[]> { sameColorPoints.ToArray(), breakablePoints.ToArray() };
    }

    /// <summary>
    /// Gets the neighbors of a grid point based on the provided directions.
    /// </summary>
    public GridPoint[] GetPointNeighbours(GridPoint gridPoint, Direction[] directions)
    {
        var neighbors = new List<GridPoint>();
        int x = (int)gridPoint.GridPosition.x;
        int y = (int)gridPoint.GridPosition.y;

        foreach (var direction in directions)
        {
            GridPoint neighbor = direction switch
            {
                Direction.Left => gridController.GetPoint(x - 1, y),
                Direction.Up => gridController.GetPoint(x, y + 1),
                Direction.Right => gridController.GetPoint(x + 1, y),
                Direction.Down => gridController.GetPoint(x, y - 1),
                Direction.LeftUp => gridController.GetPoint(x - 1, y + 1),
                Direction.RightUp => gridController.GetPoint(x + 1, y + 1),
                Direction.RightDown => gridController.GetPoint(x + 1, y - 1),
                Direction.LeftDown => gridController.GetPoint(x - 1, y - 1),
                _ => null
            };

            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors.ToArray();
    }

    /// <summary>
    /// Handles falling and filling mechanics after points are cleared.
    /// </summary>
    public void FillAndFall(GridPoint[] clearedPoints)
    {
        var columns = clearedPoints.Select(p => (int)p.GridPosition.x).Distinct();

        foreach (var column in columns)
        {
            FallColumn(column);
            FillColumn(column, GameConstants.ITEM_FALL_DURATION);
        }
    }

    /// <summary>
    /// Makes items fall to fill empty spaces in a column.
    /// </summary>
    public void FallColumn(int column)
    {
        for (int y = 0; y < gridController.GridSize.y; y++)
        {
            var currPoint = gridController.GridPoints[column, y];

            if (currPoint != null && currPoint.CurrState == GameConstants.GridPointState.Empty)
            {
                var fullPoint = FindFirstFullPoint(currPoint);

                if (fullPoint != null)
                {
                    currPoint.AddNewItem(fullPoint.CurrItem);
                    fullPoint.RemoveItem();
                }
            }
        }
    }

    /// <summary>
    /// Fills empty points in a column with new items.
    /// </summary>
    public void FillColumn(int column, float delay = 0f)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            for (int y = 0; y < gridController.GridSize.y; y++)
            {
                var currPoint = gridController.GridPoints[column, y];

                if (currPoint != null && currPoint.CurrState == GameConstants.GridPointState.Empty)
                {
                    var randomItem = FileManager.Instance.LEBase.GetRandomItem(GetPointNeighbours(currPoint, new[] { Direction.Left, Direction.Up, Direction.Right, Direction.Down }));
                    currPoint.CreateItem(randomItem, true);
                }
            }
        });
    }

    /// <summary>
    /// Finds the first full grid point above the current point.
    /// </summary>
    public GridPoint FindFirstFullPoint(GridPoint point)
    {
        for (int y = (int)point.GridPosition.y + 1; y < gridController.GridSize.y; y++)
        {
            var candidatePoint = gridController.GridPoints[(int)point.GridPosition.x, y];
            if (candidatePoint?.CurrState == GameConstants.GridPointState.Full)
            {
                return candidatePoint;
            }
        }

        return null;
    }
}