using Godot;
using System;

[GlobalClass]
public partial class WeaponStats : Resource {
	[Export]
	public double Damage { get; set; }
	[Export]
	public WeaponRange Range { get; set; }
	
	public enum WeaponRange {
		Short = 0,
		Medium = 1,
		Long = 2,
		Distance = 3,
	}

	public enum Held {
		OneHanded = 0,
		TwoHanded = 1,
		DualWielded = 2,
		Floating = 3
	}
}
