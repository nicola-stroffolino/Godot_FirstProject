using Godot;
using Godot.Collections;
using System;

public partial class Inventory : PanelContainer {
	public PackedScene SlotScene = ResourceLoader.Load<PackedScene>("res://scenes/ui/slot.tscn");
	public GridContainer ItemGrid { get; set; }

	public override void _Ready() {
		ItemGrid = GetNode<GridContainer>("MarginContainer/ItemGrid");
		var InventoryData = ResourceLoader.Load<InventoryData>("res://resources/player_inventory.tres");
	}
	
	public void SetInventoryData(InventoryData inventoryData) {
		PopulateItemGrid(inventoryData);
	}
	
	public void PopulateItemGrid(InventoryData inventoryData) {
		foreach (var child in ItemGrid.GetChildren()) {
			child.QueueFree();
		}

		foreach (var slotData in inventoryData.SlotDatas) {
			var slot = (Slot) SlotScene.Instantiate();
			ItemGrid.AddChild(slot);

			slot.Connect("SlotClicked", new Callable(inventoryData, "OnSlotClicked"));

			if (slotData != null) {
				slot.SetSlotData(slotData);
			}
		}
	}
}
