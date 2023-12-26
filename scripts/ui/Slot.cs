using Godot;
using System;

public partial class Slot : PanelContainer {
	[Signal]
	public delegate void SlotClickedEventHandler(int index, int button, bool dragged);

	public TextureRect Display { get; set; }
	public Label Count { get; set; }

	public override void _Ready() {
		Display = GetNode<TextureRect>("Background/Display");
		Count = GetNode<Label>("Count");
	}

	private void OnGuiInput(InputEvent @event) {
		if (@event is not InputEventMouseButton mb) return; 
		
		if ((mb.ButtonIndex == MouseButton.Left || mb.ButtonIndex == MouseButton.Right) && mb.Pressed) {
			EmitSignal(SignalName.SlotClicked, GetIndex(), (int) mb.ButtonIndex);
		}
	}

	public void SetSlotData(SlotData slotData) {
		var itemData = slotData.ItemData;
		Display.Texture = itemData.Texture;
		TooltipText = $"{itemData.Name} x{slotData.Quantity}";

		if (slotData.Quantity > 1) {
			Count.Text = "x" + slotData.Quantity;
		} else {
			Count.Text = null;
		}
	}

	// To implement as updating all inventory slots surely can't be that efficient
	public void UpdateSingleInventorySlot(int index) {
		//var slot = Slots[itemIndex];
		//var item = PlayerInvewntory.Items[itemIndex];
		//slot.DisplayItem(item);
	}
}





