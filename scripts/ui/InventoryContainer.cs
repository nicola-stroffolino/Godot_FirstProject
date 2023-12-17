using Godot;
using System;

public partial class InventoryContainer : CenterContainer {
	[Export]
	public PackedScene Slot { get; set; }
	
	public override void _Ready() {
		var grid = GetNode<GridContainer>("");
		// if (GetParent().has)
		// var parent = (Player) GetParent();
		// foreach (var item in ) {
			
		// }
	}
}
