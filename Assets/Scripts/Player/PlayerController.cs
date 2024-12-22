using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask selectMask; // Layer mask for raycast selection
    private RaycastHit2D hit; // Stores raycast results

    // Update is called once per frame
    void Update()
    {
        HandlePointerInput();
    }

    /// <summary>
    /// Handles pointer input and triggers grid point selection logic.
    /// </summary>
    private void HandlePointerInput()
    {
        if (!InputManager.Instance?.IsPointerUp ?? true) return; // Ensure InputManager exists and input is valid

        // Perform a 2D raycast based on the selectMask
        hit = InputManager.Instance.RaycastControl2D(selectMask);

        if (hit.collider != null && hit.transform.TryGetComponent(out GridPoint gridPoint))
        {
            ProcessGridPointSelection(gridPoint);
        }
    }

    /// <summary>
    /// Processes the selected grid point and invokes the corresponding action.
    /// </summary>
    /// <param name="gridPoint">The selected GridPoint object.</param>
    private void ProcessGridPointSelection(GridPoint gridPoint)
    {
        // Check if the grid point is valid and contains a selectable item
        if (gridPoint.CurrState == GameConstants.GridPointState.Full &&
            gridPoint.CurrItem.CurrItemModel.itemType == LEItemType.Color)
        {
            // Trigger the action with the selected grid point
            ActionManager.Instance?.InvokeAction(GameConstants.GRID_POINT_SELECTED, new ActionParam(paramObject: gridPoint));
        }
    }
}
