using Godot;
using System;

[GlobalClass]
public partial class SlotData : Resource {
	public const int MaxStackSize = 99;
	[Export]
	public ItemData ItemData { get; set; }

	[Export(PropertyHint.Range, "1,99")]
	public int Quantity { get; set; } = 1;

	public bool CanMergeWith(SlotData otherSlotData) 
		=> ItemData == otherSlotData.ItemData
		&& ItemData.Stackable
		&& Quantity < MaxStackSize;

	public bool CanFullyMergeWith(SlotData otherSlotData) 
		=> ItemData == otherSlotData.ItemData
		&& ItemData.Stackable
		&& Quantity + otherSlotData.Quantity <= MaxStackSize;

	public void FullyMergeWith(SlotData otherSlotData) => Quantity += otherSlotData.Quantity;

	public SlotData PartialMergeWith(SlotData otherSlotData) {
		otherSlotData.Quantity = Quantity + otherSlotData.Quantity - MaxStackSize;
		Quantity = MaxStackSize;
		return otherSlotData;
	}

	public SlotData DetachSingleSlotData() {
		var newSlotData = (SlotData) Duplicate();
		newSlotData.Quantity = 1;
		Quantity--;
		return newSlotData; 
	}
}
