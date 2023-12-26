using Godot;
using System;

public partial class InventoryInterface : Control {

	public PackedScene SlotScene = ResourceLoader.Load<PackedScene>("res://scenes/ui/slot.tscn");
	public SlotData GrabbedSlotData { get; set; }
	public Slot GrabbedSlot { get; set; }
	
	public override void _Ready() => Visible = false;

	public override void _Process(double delta) {
		if (GrabbedSlotData is null) return;

		GrabbedSlot.Position = GetGlobalMousePosition() + new Vector2(10, 10);
	}

	public void ToggleInventory() {
		Visible = !Visible;

		if (Visible) Input.MouseMode = Input.MouseModeEnum.Visible;
		else Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void SetPlayerInventoryData(InventoryData inventoryData) {
		inventoryData.Connect("InventoryInteracted", new Callable(this, "OnInventoryInteracted"));
		GetNode<Inventory>("Layout/Inventory").SetInventoryData(inventoryData);
	}

	public void SetPlayerWeaponInventoryData(Weapon_InventoryData weaponInventoryData) {
		weaponInventoryData.Connect("InventoryInteracted", new Callable(this, "OnInventoryInteracted"));
		GetNode<Inventory>("Layout/Inventory").SetInventoryData(weaponInventoryData);
	}

	public void OnInventoryInteracted(InventoryData inventoryData, int index, int button) {
		switch ((GrabbedSlotData, button)) {
			case (null, (int) MouseButton.Left):
				GrabbedSlotData = inventoryData.GrabSlotData(index);
				break;
			case (SlotData, (int) MouseButton.Left):
				GrabbedSlotData = inventoryData.DropSlotData(GrabbedSlotData, index);
				break;
			case (null, (int) MouseButton.Right):

				break;
			case (SlotData, (int) MouseButton.Right):
				GrabbedSlotData = inventoryData.DropSingleSlotData(GrabbedSlotData, index);
				break;
			default:
				break;
		}

		UpdateGrabbedSlot();
	}

	public void UpdateGrabbedSlot() {
		if (GrabbedSlotData is not null) {
			if (!IsInstanceValid(GrabbedSlot)) {
				GrabbedSlot = (Slot) SlotScene.Instantiate();
				GrabbedSlot.SelfModulate = new Color(1, 1, 1, 0);
				GrabbedSlot.GetNode<TextureRect>("Background").SelfModulate = new Color(1, 1, 1, 0);
				AddChild(GrabbedSlot);
			}
			
			GrabbedSlot.SetSlotData(GrabbedSlotData);
		} else if (IsInstanceValid(GrabbedSlot)) {
			GrabbedSlot.QueueFree();
		}
	}
}

