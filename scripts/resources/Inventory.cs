using Godot;
using Godot.Collections;
using System;

[GlobalClass, Tool]
public partial class Inventory : Resource {
	[Export(PropertyHint.Range, "0,90,")]
	public int NumberOfSlots { get; set; }
	[Export]
	public Array<Item> Items { get; set; }


	public void SetItem() {

	}

	public void RemoveItem() {

	}

	public void SwapItems() {

	}
}
