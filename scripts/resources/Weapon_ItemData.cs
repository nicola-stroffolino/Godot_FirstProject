using Godot;
using System;

[GlobalClass]
public partial class Weapon_ItemData : ItemData {
	[Export]
	public WeaponStats WeaponStats { get; set; }
}
