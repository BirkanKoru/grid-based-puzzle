using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const float GRID_POINT_DISTANCE_OFFSET = 1f;
    public const float ITEM_FALL_DURATION = 0.2f;

    public const string GRID_POINT_SELECTED = "GRID_POINT_SELECTED";

    public enum GridPointState
    {
        Empty,
        Full,
        Obstacle,
        Breakable
    }
}
