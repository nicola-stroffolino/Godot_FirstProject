using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class WeaponLoadout : Resource {
	[Export]
  	public Dictionary WeaponAssociation { get; set; }	
	
}
