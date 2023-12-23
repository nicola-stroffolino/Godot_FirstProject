using Godot;
using System;

public partial class InventoryInterface : Control {
	public PanelContainer Inventory { get; set; }
	
	public override void _Ready() {
		Inventory = GetNode<PanelContainer>("Inventory");
	}
	
	public void ToggleInventory() {
		Visible = !Visible;
	}
}
