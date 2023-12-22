using Godot;
using Godot.Collections;
using System;

public partial class InventoryContainer : GridContainer
{
	// [Export(PropertyHint.ResourceType, "ItemSlot")]
	// public PackedScene Slot { get; set; }
	
	public Inventory PlayerInventory { get; set; }
	public Array<InventorySlot> Slots { get; set; } = new();
	public Item FloatingItem { get; set; }
	
	public override void _Ready() {
		Visible = false;
		
		PlayerInventory = GetTree().Root.GetChild(0).GetNode<Player>("Player").PlayerInventory;
		foreach (var item in GetNode<GridContainer>("InventorySlots").GetChildren()) {
			Slots.Add((InventorySlot) item);
		}
		
		UpdateInventoryDisplay(PlayerInventory);
	}

	public override void _Process(double delta) {
		if (FloatingItem != null) GD.Print(FloatingItem.Name);
	}

	private int inv = 0;
	public void OpenInventory() {
		inv = 1 ^ inv;
		if (inv == 1) {
			Visible = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else {
			Visible = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
	}
	
	public void DisplayInventorySlots() {
		foreach (var item in Slots) {
			if (item is InventorySlot i) {
				if (PlayerInventory.Items[Slots.IndexOf(item)] != null) {
					i.DisplayItem(PlayerInventory.Items[Slots.IndexOf(item)]);
				}
			}
		}
	}

	// Specified paramether so that the function can be called
	// with other inventory segments
	public void UpdateInventoryDisplay(Inventory inventory) {
		for (int i = 0; i < inventory.Items.Count; i++) {
			UpdateInventorySlotDisplay(i);
		}
	}

	public void UpdateInventorySlotDisplay(int itemIndex) {
		var slot = Slots[itemIndex];
		var item = PlayerInventory.Items[itemIndex];
		slot.DisplayItem(item);
	}

	public void OnItemsChanged(int[] indexes) {
		for (int i = 0; i < indexes.Length; i++) {
			UpdateInventorySlotDisplay(indexes[i]);
		}
	}
}
