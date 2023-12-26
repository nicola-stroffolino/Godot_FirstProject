using Godot;
using System;

[GlobalClass]
public partial class Weapon_InventoryData : InventoryData {
	public override SlotData DropSlotData(SlotData grabbedSlotData, int index) {
		if (grabbedSlotData.ItemData is not Weapon_ItemData) return grabbedSlotData;
		
		return base.DropSlotData(grabbedSlotData, index);
	}

	public override SlotData DropSingleSlotData(SlotData grabbedSlotData, int index) {
		if (grabbedSlotData.ItemData is not Weapon_ItemData) return grabbedSlotData;

		return base.DropSingleSlotData(grabbedSlotData, index);
	}
}
