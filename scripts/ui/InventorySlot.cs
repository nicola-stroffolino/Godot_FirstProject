using Godot;
using System;

public partial class InventorySlot : CenterContainer {
	[Export(PropertyHint.Enum, "Item,Weapon")]
	public int ItemType { get; set; }

	public TextureRect Display { get; set; }

	public override void _Ready() {
		Display = GetNode<TextureRect>("Background/Display");
	}

	public void DisplayItem(Item item) {
		if (item != null) Display.Texture = item.Texture;
	}
}
