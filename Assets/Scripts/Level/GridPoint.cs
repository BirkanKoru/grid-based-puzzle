using UnityEngine;
using DG.Tweening;

public class GridPoint : MonoBehaviour
{
    /// <summary>
    /// The grid position of this point.
    /// </summary>
    public Vector2 GridPosition { get; private set; }

    /// <summary>
    /// The current state of this grid point.
    /// </summary>
    public GameConstants.GridPointState CurrState;

    /// <summary>
    /// The current item at this grid point.
    /// </summary>
    public Item CurrItem { get; private set; }

    private Item itemPrefab;

    /// <summary>
    /// Initializes the grid point with its position and initial item information.
    /// </summary>
    public void SetPoint(Vector2 gridPosition, int gridInfo, Item itemPrefab)
    {
        this.GridPosition = gridPosition;
        this.itemPrefab = itemPrefab;

        var itemModel = FileManager.Instance.GetModel(gridInfo);
        CreateItem(itemModel, canFall: false);
    }

    /// <summary>
    /// Creates an item at this grid point.
    /// </summary>
    public void CreateItem(LEItemModel itemModel, bool canFall)
    {
        CurrItem = Instantiate(itemPrefab, this.transform);

        if(canFall) {

            CurrItem.transform.localPosition = new Vector3(0f, this.transform.localPosition.y + 10f, 0f);
            CurrItem.transform.DOLocalMove(Vector3.zero, GameConstants.ITEM_FALL_DURATION * 2f);

        } else {

            CurrItem.transform.localPosition = Vector3.zero;
        }

        CurrItem.SetItem(itemModel);

        if(itemModel.itemType == LEItemType.Color || itemModel.itemType == LEItemType.Breakable)
            CurrState = GameConstants.GridPointState.Full;
        else if(itemModel.itemType == LEItemType.Obstacle)
            CurrState = GameConstants.GridPointState.Obstacle;
    }

    /// <summary>
    /// Removes the current item from this grid point and updates its state.
    /// </summary>
    public void RemoveItem()
    {
        CurrItem = null;
        CurrState = GameConstants.GridPointState.Empty;
    }

    /// <summary>
    /// Adds a new item to this grid point.
    /// </summary>
    public void AddNewItem(Item newItem)
    {
        CurrItem = newItem;
        CurrItem.transform.SetParent(this.transform);
        CurrItem.transform.DOLocalMove(Vector3.zero, GameConstants.ITEM_FALL_DURATION).SetEase(Ease.OutBack);

        CurrState = GameConstants.GridPointState.Full;
    }
}
