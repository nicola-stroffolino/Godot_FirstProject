using Godot;
using System;

[GlobalClass]
public partial class WeaponItem : Item {
	[Export]
	public WeaponStats WeaponStats { get; set; }
}
