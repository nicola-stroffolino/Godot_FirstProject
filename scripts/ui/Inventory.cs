using Godot;
using Godot.Collections;
using System;

public partial class Inventory : PanelContainer {
	public PackedScene SlotScene = ResourceLoader.Load<PackedScene>("res://scenes/ui/slot.tscn");
	//public GridContainer ItemGrid { get; set; }
	//public GridContainer WeaponGrid { get; set; }

	public override void _Ready() {
		//ItemGrid = GetNode<GridContainer>("MarginContainer/HBoxContainer/ItemGrid");
		//WeaponGrid = GetNode<GridContainer>("MarginContainer/HBoxContainer/WeaponGrid");
		var InventoryData = ResourceLoader.Load<InventoryData>("res://resources/player_inventory.tres");
	}
	
	public void SetInventoryData(InventoryData inventoryData) {
		inventoryData.Connect("InventoryUpdated", new Callable(this, "PopulateItemGrid"));
		PopulateItemGrid(inventoryData);
	}
	
	public void PopulateItemGrid(InventoryData inventoryData) {
		// This part is to update whenever we add a new item grid to the inventory
		// e.g an armor equip item grid
		GridContainer itemGrid;
		if (inventoryData is Weapon_InventoryData) itemGrid = GetNode<GridContainer>("MarginContainer/HBoxContainer/WeaponGrid");
		else itemGrid = GetNode<GridContainer>("MarginContainer/HBoxContainer/ItemGrid");

		foreach (var child in itemGrid.GetChildren()) {
			child.QueueFree();
		}

		foreach (var slotData in inventoryData.SlotDatas) {
			var slot = (Slot) SlotScene.Instantiate();
			itemGrid.AddChild(slot);

			slot.Connect("SlotClicked", new Callable(inventoryData, "OnSlotClicked"));

			if (slotData != null) {
				slot.SetSlotData(slotData);
			}
		}
	}
}
