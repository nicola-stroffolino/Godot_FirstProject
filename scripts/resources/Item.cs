using Godot;
using System;

[GlobalClass]
public partial class Item : Resource { 
	[Export]
	public string Name { get; set; }
	[Export]    
	public Texture Texture { get; set; }
	[Export]
	public PackedScene Model { get; set; }
}
