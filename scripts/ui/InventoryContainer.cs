using Godot;
using System;

public partial class InventoryContainer : GridContainer
{
	[Export(PropertyHint.ResourceType, "ItemSlot")]
	public PackedScene Slot { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public DisplayInventory() {
		
	}
}
