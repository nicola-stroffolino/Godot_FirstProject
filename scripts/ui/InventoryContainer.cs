using Godot;
using Godot.Collections;
using System;

public partial class InventoryContainer : GridContainer
{
	// [Export(PropertyHint.ResourceType, "ItemSlot")]
	// public PackedScene Slot { get; set; }
	
	public Inventory PlayerInventory { get; set; }
	public Array<CenterContainer> Slots { get; set; } = new();
	
	public override void _Ready() {
		PlayerInventory = GetTree().Root.GetChild(0).GetNode<Player>("Player").PlayerInventory;
		foreach (var item in GetNode<GridContainer>("InventorySlots").GetChildren()) {
			Slots.Add((CenterContainer) item);
		}
		DisplayInventorySlots();
	}
	
	public void DisplayInventorySlots() {
		foreach (var item in Slots) {
			if (item is ItemSlot i) {
				if (PlayerInventory.Items[Slots.IndexOf(item)] != null) {
					i.DisplayItem((Texture2D) PlayerInventory.Items[Slots.IndexOf(item)].Texture);
				}
			}
		}
	}
}
