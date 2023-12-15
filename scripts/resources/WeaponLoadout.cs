using Godot;
using Godot.Collections;
using System;

public partial class WeaponLoadout : Resource {
	[Export]
	public Dictionary<string, Weapon> WeaponAssociation { get; set; }
	
}
