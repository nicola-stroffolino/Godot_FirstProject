using Godot;
using System;

public partial class Slot : PanelContainer {
	[Signal]
	public delegate void SlotClickedEventHandler(int index, int button);

	public TextureRect Display { get; set; }
	public Label Count { get; set; }

	public override void _Ready() {
		Display = GetNode<TextureRect>("Background/Display");
		Count = GetNode<Label>("Count");
	}

	public void SetSlotData(SlotData slotData) {
		var itemData = slotData.ItemData;
		Display.Texture = itemData.Texture;
		TooltipText = $"{itemData.Name} x{slotData.Quantity}";

		if (slotData.Quantity > 1) {
			Count.Text = "x" + slotData.Quantity;
		}
	}
	
	private void OnGuiInput(InputEvent @event) {
		if (@event is not InputEventMouseButton mb) return; 
		
		if ((mb.ButtonIndex == MouseButton.Left || mb.ButtonIndex == MouseButton.Right) && mb.Pressed) {
			EmitSignal(SignalName.SlotClicked, GetIndex(), (int) mb.ButtonIndex);
		}
	}

	// [Export(PropertyHint.Enum, "Item,Weapon")]
	// public int ItemType { get; set; }

	// public InventoryData PlayerInventory { get; set; }
	// public TextureRect Display { get; set; }

	// public override void _Ready() {
	// 	PlayerInventory = GetTree().Root.GetChild(0).GetNode<Player>("Player").PlayerInventory;
	// 	Display = GetNode<TextureRect>("Background/Display");
	// 	AttachCallback(this);
	// }

	// public void DisplayItem(ItemData item) {
	// 	if (item != null) Display.Texture = item.Texture;
	// }

	// // Item Drag Functions
	// public override Variant _GetDragData(Vector2 atPosition)
	// {
	// 	GD.Print("Clicked");
	// 	return base._GetDragData(atPosition);
	// }

	// public override bool _CanDropData(Vector2 atPosition, Variant data)
	// {	 
	// 	return base._CanDropData(atPosition, data);
	// }

	// public override void _DropData(Vector2 atPosition, Variant data)
	// {
	// 	base._DropData(atPosition, data);
	// }

	// // Mouse Click Handler "stroffo gay";
	// public void AttachCallback(Node node) {
	// 	if (node is Control) {
	// 		var cb = new Callable(node, "Handler");
			
	// 	}
	// }

	// public void Handler(InputEvent what, Node who) {
	// 	GD.Print(what, who);
	// }

	// public override void _GuiInput(InputEvent @event)
	// {
	// 	if (@event is InputEventMouseButton mb)
	// 	{
	// 		if (mb.ButtonIndex == MouseButton.Left && mb.Pressed)
	// 		{
	// 			var i = GetNode<GridContainer>("..").GetChildren().IndexOf(this);
	// 			var item = PlayerInventory.Items[i];
				
	// 			var container = (InventoryContainer) GetNode<GridContainer>("../..");
	// 			container.FloatingItem = item;
	// 		}
	// 	}
	// }
}


