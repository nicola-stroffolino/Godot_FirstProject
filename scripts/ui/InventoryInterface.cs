using Godot;
using System;

public partial class InventoryInterface : Control {
	public Inventory Inventory { get; set; }
	
	public override void _Ready() {
		Inventory = GetNode<Inventory>("Layout/Inventory");
		Visible = false;
	}
	
	public void SetPlayerInventoryData(InventoryData inventoryData) {
		Inventory.SetInventoryData(inventoryData);
	}

	public void ToggleInventory() {
		Visible = !Visible;

		if (Visible) Input.MouseMode = Input.MouseModeEnum.Visible;
		else Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
