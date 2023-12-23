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
	
	public void SetInventoryData(InventoryData InventoryData) {
		PopulateItemGrid(InventoryData.SlotDatas);
	}
	
	public void PopulateItemGrid(Array<SlotData> slotDatas) {
		foreach (var child in ItemGrid.GetChildren()) {
			child.QueueFree();
		}

		foreach (var slotData in slotDatas) {
			var slot = SlotScene.Instantiate();
			ItemGrid.AddChild(slot);

			if (slotData != null) {
				(slot as Slot).SetSlotData(slotData);
			}
		}
	}
}
