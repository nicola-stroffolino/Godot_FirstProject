using Godot;
using System;

public partial class ItemSlot : CenterContainer {
	[Export(PropertyHint.Enum, "Item,Weapon")]
	public int ItemType { get; set; }
	
	public TextureRect Display { get; set; }

	public override void _Ready() {
		Display = GetNode<TextureRect>("TextureRect/TextureRect");
	}

	public void DisplayItem(Texture2D texture) {
		if (texture != null) Display.Texture = texture;
		else Display.Texture = null;
	}
}
