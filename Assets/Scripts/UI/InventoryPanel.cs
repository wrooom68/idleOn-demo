using System.Collections.Generic;
using IdleGuildDemo.Core;
using IdleGuildDemo.Data;
using IdleGuildDemo.Runtime;
using IdleGuildDemo.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGuildDemo.UI
{
  public class InventoryPanel : UIPanel
  {
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotView slotPrefab;
    [SerializeField] private Text detailText;

    public Transform SlotContainer => slotContainer;
    public InventorySlotView SlotPrefab => slotPrefab;

    private readonly List<InventorySlotView> _slots = new List<InventorySlotView>();

    public void RefreshFromServices(ServiceRegistry services, IReadOnlyList<ItemDefinition> itemDefinitions = null)
    {
      if (services == null || !services.IsInitialized)
      {
        ClearSlots();
        SetDetail("Inventory unavailable.");
        return;
      }

      RefreshInventory(services.InventorySystem, services.PlayerProfile, itemDefinitions);
    }

    public void RefreshInventory(
      InventorySystem inventorySystem,
      PlayerProfile profile,
      IReadOnlyList<ItemDefinition> itemDefinitions = null)
    {
      if (inventorySystem == null)
      {
        ClearSlots();
        SetDetail("Inventory unavailable.");
        return;
      }

      IReadOnlyList<InventoryStack> stacks = inventorySystem.GetStacks();
      EnsureSlotCount(stacks.Count);

      for (int i = 0; i < _slots.Count; i++)
      {
        InventorySlotView slot = _slots[i];
        if (slot == null)
        {
          continue;
        }

        if (i >= stacks.Count)
        {
          slot.SetEmpty();
          continue;
        }

        InventoryStack stack = stacks[i];
        ItemDefinition itemDefinition = FindItem(itemDefinitions, stack.itemId);
        string displayName = itemDefinition != null && !string.IsNullOrEmpty(itemDefinition.DisplayName)
          ? itemDefinition.DisplayName
          : FormatItemName(stack.itemId);
        Sprite icon = itemDefinition != null ? itemDefinition.Icon : null;
        slot.SetItem(icon, stack.quantity, displayName);
      }

      int coins = profile != null ? profile.coins : 0;
      SetDetail($"Coins: {coins} | Stacks: {stacks.Count}");
    }

    public void SetDetail(string detail)
    {
      if (detailText != null)
      {
        detailText.text = detail;
      }
    }

    public void ClearSlots()
    {
      for (int i = 0; i < _slots.Count; i++)
      {
        if (_slots[i] != null)
        {
          _slots[i].SetEmpty();
        }
      }
    }

    private void EnsureSlotCount(int count)
    {
      if (slotContainer == null || slotPrefab == null)
      {
        return;
      }

      while (_slots.Count < count)
      {
        _slots.Add(Instantiate(slotPrefab, slotContainer));
      }
    }

    private static ItemDefinition FindItem(IReadOnlyList<ItemDefinition> items, string itemId)
    {
      if (items == null || string.IsNullOrEmpty(itemId))
      {
        return null;
      }

      foreach (ItemDefinition item in items)
      {
        if (item != null && item.Id == itemId)
        {
          return item;
        }
      }

      return null;
    }

    private static string FormatItemName(string itemId)
    {
      switch (itemId)
      {
        case GameConstants.ItemCopperOreId:
          return "Copper Ore";
        case GameConstants.ItemCopperBarId:
          return "Copper Bar";
        case GameConstants.ItemSlimeGooId:
          return "Slime Goo";
        case GameConstants.ItemCopperSwordId:
          return "Copper Sword";
        case GameConstants.ItemCopperPickaxeId:
          return "Copper Pickaxe";
        case GameConstants.ItemCoinsId:
          return "Coins";
        default:
          return string.IsNullOrEmpty(itemId) ? "Item" : itemId;
      }
    }
  }
}
