 using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class InventoryData : Resource {
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
		var grabTargetSlotData = SlotDatas[index];
		
		if (grabTargetSlotData != null) {
			SlotDatas[index] = null;
			EmitSignal(SignalName.InventoryUpdated, this);
		}

		return grabTargetSlotData;
	}

	public virtual SlotData DropSlotData(SlotData grabbedSlotData, int index) {
		var targetSlotData = SlotDatas[index];

		SlotData returnSlotData = null;
		if (targetSlotData is not null && targetSlotData.CanFullyMergeWith(grabbedSlotData)) {
			targetSlotData.FullyMergeWith(grabbedSlotData);
		} else if (targetSlotData is not null && targetSlotData.CanMergeWith(grabbedSlotData)) {
			returnSlotData = targetSlotData.PartialMergeWith(grabbedSlotData);
		} else {
			SlotDatas[index] = grabbedSlotData;
			returnSlotData = targetSlotData;
		}

		EmitSignal(SignalName.InventoryUpdated, this);
		
		return returnSlotData; // either null or SlotData
	}

	public virtual SlotData DropSingleSlotData(SlotData grabbedSlotData, int index) {
		var targetSlotData = SlotDatas[index];

		if (targetSlotData is null) SlotDatas[index] = grabbedSlotData.DetachSingleSlotData();
		else if (targetSlotData.CanMergeWith(grabbedSlotData)) targetSlotData.FullyMergeWith(grabbedSlotData.DetachSingleSlotData());

		EmitSignal(SignalName.InventoryUpdated, this);

		if (grabbedSlotData.Quantity > 0) return grabbedSlotData;
		else return null;
	}
}
