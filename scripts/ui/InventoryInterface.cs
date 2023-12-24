using Godot;
using System;

public partial class InventoryInterface : Control {

	public PackedScene SlotScene = ResourceLoader.Load<PackedScene>("res://scenes/ui/slot.tscn");
	public SlotData GrabbedSlotData { get; set; }
	public Slot GrabbedSlot { get; set; }
	
	public override void _Ready() => Visible = false;

	public override void _PhysicsProcess(double delta) {
		if (GrabbedSlotData is null) return;

		GrabbedSlot.Position = GetGlobalMousePosition() + new Vector2(10, 10);
	}

	public void ToggleInventory() {
		Visible = !Visible;

		if (Visible) Input.MouseMode = Input.MouseModeEnum.Visible;
		else Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	/* --- */

	public void SetPlayerInventoryData(InventoryData inventoryData) {
		inventoryData.Connect("InventoryInteracted", new Callable(this, "OnInventoryInteracted"));
		GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
	}

	public void OnInventoryInteracted(InventoryData inventoryData, int index, int button) {

		switch ((GrabbedSlotData, button)) {
			case (null, (int) MouseButton.Left):
				if (inventoryData.SlotDatas[index] == null) return;

				GrabbedSlotData = inventoryData.SlotDatas[index];
				inventoryData.SlotDatas[index] = null;
				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);

				GrabbedSlot = (Slot) SlotScene.Instantiate();
				GrabbedSlot.SelfModulate = new Color(1, 1, 1, 0);
				GrabbedSlot.GetNode<TextureRect>("Background").SelfModulate = new Color(1, 1, 1, 0);

				AddChild(GrabbedSlot);
				GrabbedSlot.SetSlotData(GrabbedSlotData);
				break;
			case (SlotData, (int) MouseButton.Left):
				if (inventoryData.SlotDatas[index] == null) {
					inventoryData.SlotDatas[index] = GrabbedSlotData;
					GrabbedSlotData = null;
					GrabbedSlot.QueueFree();
				} else {
					if (inventoryData.SlotDatas[index].ItemData == GrabbedSlotData.ItemData) {
						var deposited = inventoryData.SlotDatas[index].Quantity + GrabbedSlotData.Quantity;
						if (deposited > 99) {
							inventoryData.SlotDatas[index].Quantity = 99;
							GrabbedSlotData.Quantity = deposited - 99;
							GrabbedSlot.SetSlotData(GrabbedSlotData);
						} else {
							inventoryData.SlotDatas[index].Quantity = deposited;
							GrabbedSlotData = null;
							GrabbedSlot.QueueFree();
						}
					} else {
						(GrabbedSlotData, inventoryData.SlotDatas[index]) = (inventoryData.SlotDatas[index], GrabbedSlotData);
						GrabbedSlot.SetSlotData(GrabbedSlotData);
					}
				}
				
				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
				break;
			case (null, (int) MouseButton.Right):
				if (inventoryData.SlotDatas[index] == null) return;

				var smallQt = inventoryData.SlotDatas[index].Quantity / 2;
				var bigQt = inventoryData.SlotDatas[index].Quantity / 2 + inventoryData.SlotDatas[index].Quantity % 2;

				inventoryData.SlotDatas[index].Quantity = smallQt;

				GrabbedSlotData = (SlotData) inventoryData.SlotDatas[index].Duplicate();
				GrabbedSlotData.Quantity = bigQt;
				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);

				GrabbedSlot = (Slot) SlotScene.Instantiate();
				GrabbedSlot.SelfModulate = new Color(1, 1, 1, 0);
				GrabbedSlot.GetNode<TextureRect>("Background").SelfModulate = new Color(1, 1, 1, 0);

				AddChild(GrabbedSlot);
				GrabbedSlot.SetSlotData(GrabbedSlotData);

				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
				break;
			case (SlotData, (int) MouseButton.Right):
				break;
			default:
				break;
		}
	}

	// override public void _GuiInput(InputEvent @event) {
	// 	if (@event is not InputEventMouseButton mb) return;

	// 	GD.Print(mb.ButtonIndex);

	// 	if ((mb.ButtonIndex == MouseButton.Left || mb.ButtonIndex == MouseButton.Right) && mb.Pressed) {
			
	// 	}
	// }
}



