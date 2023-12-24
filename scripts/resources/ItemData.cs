using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource { 
	[Export]
	public string Name { get; set; }
	[Export]    
	public AtlasTexture Texture { get; set; }
	[Export]
	public PackedScene Model { get; set; }
	[Export]
	public bool Stackable { get; set; } = false;
}
