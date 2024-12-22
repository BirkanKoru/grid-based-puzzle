using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item on the grid, including its health and appearance.
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer skin;

    /// <summary>
    /// The model representing the item's properties and state.
    /// </summary>
    public LEItemModel CurrItemModel { get; private set; }

    private int MaxHealth = 0;
    public int CurrHealth { get; private set; }

    /// <summary>
    /// Initializes the item with a specified model.
    /// </summary>
    public void SetItem(LEItemModel itemModel)
    {
        this.CurrItemModel = itemModel;
        this.MaxHealth = itemModel.itemHealth;
        this.CurrHealth = MaxHealth;

        UpdateAppearance();
    }

    /// <summary>
    /// Updates the item's sprite based on its current health.
    /// </summary>
    private void UpdateAppearance()
    {
        int diff = MaxHealth - CurrHealth;
        skin.sprite = CurrItemModel.itemIcons[diff];
    }

    /// <summary>
    /// Reduces the item's health and updates its appearance.
    /// </summary>
    /// <returns>True if the item's health reaches zero; otherwise, false.</returns>
    public bool Damaged()
    {
        CurrHealth--;

        if(CurrHealth <= 0)
        {
            DeactivateItem();
            return true;

        } else {
            
            UpdateAppearance();
            return false;
        }
    }

    /// <summary>
    /// Deactivates the item when its health reaches zero.
    /// </summary>
    private void DeactivateItem()
    {
        gameObject.SetActive(false);
    }
}
