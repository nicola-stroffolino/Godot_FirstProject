 using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class InventoryData : Resource {
	[Signal]
	public delegate void InventorySlotUpdatedEventHandler(int index);
	[Signal]
	public delegate void InventoryUpdatedEventHandler(InventoryData inventoryData);
	[Signal]
	public delegate void InventoryInteractedEventHandler(InventoryData inventoryData, int index, int button);
	
	[Export]
	public Array<SlotData> SlotDatas { get; set; }

	public void OnSlotClicked(int index, int button) {
		EmitSignal(SignalName.InventoryInteracted, this, index, button);
	}

	public SlotData GrabSlotData(int index) {
		var slotData = SlotDatas[index];
		
		if (slotData != null) {
			SlotDatas[index] = null;
			EmitSignal(SignalName.InventoryUpdated, this);
		}

		return slotData;
	}

	public SlotData DropSlotData(SlotData grabbedSlotData, int index) {
		var slotData = SlotDatas[index];
		SlotDatas[index] = grabbedSlotData;
		
		return slotData;
	}

	//public ItemData SetItem(int itemIndex, ItemData item) {
		//var previousItem = Items[itemIndex];
		//Items[itemIndex] = item;
		//return previousItem;
	//}
//
	//public ItemData RemoveItem(int itemIndex) {
		//var previousItem = Items[itemIndex];
		//Items[itemIndex] = null;
		//return previousItem;
	//}
//
	//public void SwapItems(int fromItemIndex, int toItemIndex) {
		//(Items[toItemIndex], Items[fromItemIndex]) = (Items[fromItemIndex], Items[toItemIndex]);
	//}


}
