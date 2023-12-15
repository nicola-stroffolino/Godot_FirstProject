using Godot;
using System;

public abstract partial class Weapon : Node3D {
	public enum Held {
		OneHanded = 0,
		TwoHanded = 1,
		DualWielded = 2,
		Floating = 3
	}
}
