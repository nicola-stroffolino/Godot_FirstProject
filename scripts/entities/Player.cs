using System;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody3D {
	[Signal]
	public delegate void SpreadDamageEventHandler(double amount);
	
	[Export(PropertyHint.ResourceType, "Attributes")]
	public Attributes PlayerAttributes { get; set; }
	[Export]
	public RayCast3D Pointer { get; set; }
	[Export]
	public Array<Marker3D> AnchorsArray { get; set; } = new() { null, null, null, null };


	public Dictionary<string, Marker3D> AnchorDictionary { get; set; } = new();

	public override void _Ready() {
		GD.Print(AnchorsArray.Count);
		foreach (var anchor in AnchorsArray) {
			AnchorDictionary.Add(anchor.Name, anchor);
			GD.Print(anchor.Name + ", " + AnchorDictionary[anchor.Name]);
		}
	}

	public override void _PhysicsProcess(double delta) {
		GD.Print(AnchorsArray.Count);
		if (Pointer.IsColliding() && Input.IsActionJustPressed("pickup_item")) {
			if (Pointer.GetCollider() is StaticBody3D item) {
				GD.Print(item.GetParent().Name + " - " + (item.GetParent() is Weapon));
				PickupItem(item.GetParent());
			}
		};
	}

	public void PushWeapon(Weapon weapon) {
		//WeaponLoadout.Add(weapon);	
	}
	
	public void Attack(double damage) {
		EmitSignal(SignalName.SpreadDamage, damage);
	}

	public void PickupItem(Node item) {
		if (item is not Weapon w) return;
		
		switch (w.Name) {
			case "excalibur_morgan":
				AnchorDictionary["em_position"].AddChild(w);
				break;
			default:
				break;
		}
	}
}

