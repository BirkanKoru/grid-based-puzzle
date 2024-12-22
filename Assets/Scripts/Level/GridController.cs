using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridPoint pointPrefab; // Prefab for grid points
    [SerializeField] private Item itemPrefab;       // Prefab for items

    public Vector2 GridSize { get; private set; }   // Dimensions of the grid
    public GridPoint[,] GridPoints { get; private set; } // 2D array holding all grid points

    private FileManager fileManager;  // Reference to FileManager for grid setup
    private int[,] levelGridInfo;     // Grid data defining the initial state
    private GridOperations gridOperations; // Handles grid-based operations
    
    /// <summary>
    /// Initializes the grid.
    /// </summary>
    public void CreateGrid()
    {
        SetupGridSize();
        InitializeGridPoints();
        gridOperations = new GridOperations(this); // Instantiate GridOperations
    }

    /// <summary>
    /// Configures the grid size based on the FileManager data.
    /// </summary>
    private void SetupGridSize()
    {
        fileManager = FileManager.Instance;

        if (fileManager == null)
        {
            Debug.LogError("FileManager instance is null. Cannot set up grid.");
            return;
        }

        GridSize = new Vector2(fileManager.CurrColumnCount, fileManager.CurrRowCount);
        levelGridInfo = fileManager.CurrGridInfo;
    }

    /// <summary>
    /// Instantiates the grid points and positions them in the scene.
    /// </summary>
    private void InitializeGridPoints()
    {
        GridPoints = new GridPoint[(int)GridSize.x, (int)GridSize.y];

        for(int y=0; y < (int)GridSize.y; y++)
        {
            for(int x=0; x < (int)GridSize.x; x++)
            {
                GridPoint newPoint = Instantiate(pointPrefab, this.transform);
                newPoint.transform.localPosition = new Vector3(x * GameConstants.GRID_POINT_DISTANCE_OFFSET, y * GameConstants.GRID_POINT_DISTANCE_OFFSET, 0f);
                newPoint.transform.name = "Point_" + x + "_" + y;
                newPoint.SetPoint(new Vector2(x, y), levelGridInfo[x, y], itemPrefab);

                GridPoints[x, y] = newPoint;
            }
        }

        // Center the grid in the scene
        this.transform.localPosition = new Vector3(((GridSize.x - 1f) / 2f) * -1f, ((GridSize.y - 1f) / 2f) * -1f );
    }

    /// <summary>
    /// Retrieves a grid point at a specific (x, y) position, ensuring it's within bounds.
    /// </summary>
    public GridPoint GetPoint(int x, int y)
    {
        if(x >= 0 && x < (int)GridSize.x && y >= 0 && y < (int)GridSize.y)
        {
            return GridPoints[x, y];
        }

        return null;
    }

    /// <summary>
    /// Subscribes to relevant events when enabled.
    /// </summary>
    private void OnEnable()
    {
        ActionManager.Instance?.StartListening(GameConstants.GRID_POINT_SELECTED, OnGridPointSelected);
    }

    /// <summary>
    /// Unsubscribes from events when disabled.
    /// </summary>
    private void OnDisable()
    {
        ActionManager.Instance?.StopListening(GameConstants.GRID_POINT_SELECTED, OnGridPointSelected);
    }

    /// <summary>
    /// Handles the selection of a grid point.
    /// </summary>
    private void OnGridPointSelected(ActionParam param)
    {
        if (param?.paramObject is not GridPoint selectedPoint)
        {
            Debug.LogWarning("Invalid parameter received for grid point selection.");
            return;
        }

        HandleGridPointSelection(selectedPoint);
    }

    /// <summary>
    /// Finds and processes matches for the selected grid point.
    /// </summary>
    private void HandleGridPointSelection(GridPoint selectedPoint)
    {
        var matches = gridOperations?.GetMatches(selectedPoint);
        if (matches == null || matches.Count < 2) return;

        GridPoint[] matchPoints = matches[0];
        GridPoint[] breakablePoints = matches[1];

        if(matchPoints.Length >= 3)
        {
            ProcessMatchedPoints(matchPoints, breakablePoints);
        }
    }

    /// <summary>
    /// Processes matched points and handles item destruction and falling logic.
    /// </summary>
    private void ProcessMatchedPoints(GridPoint[] matchPoints, GridPoint[] breakablePoints)
    {
        GridPoint[] allPoints = matchPoints.Union(breakablePoints).ToArray();

        foreach(GridPoint point in allPoints)
        {
            if(point?.CurrItem == null) continue;

            bool destroyed = point.CurrItem.Damaged();
            if(destroyed)
            {
                Item destroyItem = point.CurrItem;
                Destroy(destroyItem, 1f);

                point.RemoveItem();
            }
        }

        gridOperations.FillAndFall(allPoints);
    }
}
