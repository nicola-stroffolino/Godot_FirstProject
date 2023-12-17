using System;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody3D {
	[Signal]
	public delegate void SpreadDamageEventHandler(double amount);
	
	[Export(PropertyHint.ResourceType, "Attributes")]
	public Attributes PlayerAttributes { get; set; }

	public Array<Weapon> WeaponLoadout { get; set; } = new ();

	public override void _Ready() {
		
	}

	public override void _PhysicsProcess(double delta) {
		var spaceState = GetWorld3D().DirectSpaceState;

		// var query = PhysicsRayQueryParameters3D.Create();
		// var result = spaceState.IntersectRay();

	}

	public void PushWeapon(Weapon weapon) {
		WeaponLoadout.Add(weapon);	
	}
	
	public void Attack(double damage) {
		EmitSignal(SignalName.SpreadDamage, damage);
	}
}

