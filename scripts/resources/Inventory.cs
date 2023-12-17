using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Inventory : Resource {
	[Export]
	public Array<Item> Items { get; set; }

	public Item SetItem(int itemIndex, Item item) {
		var previousItem = Items[itemIndex];
		Items[itemIndex] = item;
		return previousItem;
	}

	public Item RemoveItem(int itemIndex) {
		var previousItem = Items[itemIndex];
		Items[itemIndex] = null;
		return previousItem;
	}

	public void SwapItems(int fromItemIndex, int toItemIndex) {
		(Items[toItemIndex], Items[fromItemIndex]) = (Items[fromItemIndex], Items[toItemIndex]);
	}
}
