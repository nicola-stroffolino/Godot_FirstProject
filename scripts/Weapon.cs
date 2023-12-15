using Godot;
using System;

public partial class Weapon : Node3D {
	[Export]
	public WeaponStats StatsResource { get; set; }
}
