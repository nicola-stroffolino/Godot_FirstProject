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

				var smallQt = 0;
				var bigQt = 1;
				if (inventoryData.SlotDatas[index].Quantity > 1) {
					smallQt = inventoryData.SlotDatas[index].Quantity / 2;
					bigQt = inventoryData.SlotDatas[index].Quantity / 2 + inventoryData.SlotDatas[index].Quantity % 2;
				}
				
				GrabbedSlotData = (SlotData) inventoryData.SlotDatas[index].Duplicate();
				GrabbedSlotData.Quantity = bigQt;

				inventoryData.SlotDatas[index].Quantity = smallQt;
				if (inventoryData.SlotDatas[index].Quantity == 0) inventoryData.SlotDatas[index] = null;

				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);

				GrabbedSlot = (Slot) SlotScene.Instantiate();
				GrabbedSlot.SelfModulate = new Color(1, 1, 1, 0);
				GrabbedSlot.GetNode<TextureRect>("Background").SelfModulate = new Color(1, 1, 1, 0);

				AddChild(GrabbedSlot);
				GrabbedSlot.SetSlotData(GrabbedSlotData);

				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
				break;
			case (SlotData, (int) MouseButton.Right):
				if (inventoryData.SlotDatas[index] == null) {
					var dropped =  (SlotData) GrabbedSlotData.Duplicate();
					dropped.Quantity = 1;
					GrabbedSlotData.Quantity--;

					GrabbedSlot.SetSlotData(GrabbedSlotData);

					if (GrabbedSlotData.Quantity <= 0) {
						GrabbedSlotData = null;
						GrabbedSlot.QueueFree();
					}

					inventoryData.SlotDatas[index] = dropped;
				} else if (inventoryData.SlotDatas[index].ItemData == GrabbedSlotData.ItemData && inventoryData.SlotDatas[index].Quantity < 99) {
					
					inventoryData.SlotDatas[index].Quantity++;
					GrabbedSlotData.Quantity--;
					
					GrabbedSlot.SetSlotData(GrabbedSlotData);

					if (GrabbedSlotData.Quantity <= 0) {
						GrabbedSlotData = null;
						GrabbedSlot.QueueFree();
					}
				}

				GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
				break;
			default:
				break;
		}
	}
}



