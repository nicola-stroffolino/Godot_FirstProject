using Godot;
using System;

[GlobalClass]
public partial class SlotData : Resource {
	public const int MaxStackSize = 99;
	[Export]
	public ItemData ItemData { get; set; }

	[Export(PropertyHint.Range, "1,99")]
	public int Quantity { get; set; } = 1;
}
