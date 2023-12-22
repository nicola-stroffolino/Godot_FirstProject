using Godot;
using System;

public partial class InventorySlot : CenterContainer {
	[Export(PropertyHint.Enum, "Item,Weapon")]
	public int ItemType { get; set; }

	public Inventory PlayerInventory { get; set; }
	public TextureRect Display { get; set; }

	public override void _Ready() {
		PlayerInventory = GetTree().Root.GetChild(0).GetNode<Player>("Player").PlayerInventory;
		Display = GetNode<TextureRect>("Background/Display");
	}

	public void DisplayItem(Item item) {
		if (item != null) Display.Texture = item.Texture;
	}

	// Item Drag Functions
	public override Variant _GetDragData(Vector2 atPosition)
	{
		GD.Print("Clicked");
		return base._GetDragData(atPosition);
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{	 
		return base._CanDropData(atPosition, data);
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		base._DropData(atPosition, data);
	}

	// Mouse Click Handler
	public void AttachCallback(Node node) {
		if (node is Control) {
			var cb = new Callable(node, "Handler");
		
		}
	}

	public void Handler(InputEvent what, Node who) {
		GD.Print(what, who);
	}
}
