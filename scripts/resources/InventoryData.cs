using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class InventoryData : Resource {
	[Export]
	public Array<SlotData> SlotDatas { get; set; }

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
