using System;
using Godot;
using Godot.Collections;

public partial class Player : CharacterBody3D {
	[Signal]
	public delegate void SpreadDamageEventHandler(double amount);
	[Signal]
	public delegate void InventoryDataReadyEventHandler(InventoryData inventoryData);
	[Signal]
	public delegate void WeaponInventoryDataReadyEventHandler(Weapon_InventoryData inventoryData);
	
	[Export]
	public Attributes _Attributes { get; set; }
	[Export]
	public InventoryData _InventoryData { get; set; }
	[Export]
	public Weapon_InventoryData _WeaponInventoryData { get; set; }
	
	[Export]
	public RayCast3D Pointer { get; set; }
	[Export]
	public Array<NodePath> AnchorsPathArray { get; set; }

	public Array<Marker3D> AnchorsArray { get; set; } = new();
	public Dictionary<string, Marker3D> AnchorDictionary { get; set; } = new();

	public override void _Ready() {
		foreach (var anchor in AnchorsPathArray) {
			AnchorsArray.Add(GetNode<Marker3D>(anchor));
		}

		foreach (var anchor in AnchorsArray) {
			AnchorDictionary.Add(anchor.Name, anchor);
		}

		EmitSignal(SignalName.InventoryDataReady, _InventoryData);
		EmitSignal(SignalName.WeaponInventoryDataReady, _WeaponInventoryData);
	}

	public override void _PhysicsProcess(double delta) {
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
		//if (item is not Weapon w) return;
		
		var new_w = (Node3D) item.Duplicate();
		new_w.Position = Vector3.Zero;
		new_w.Rotation = Vector3.Zero;
		new_w.Scale = new Vector3(1, 1, 1);

		switch (item.Name) {
			case "excalibur_morgan":
				AnchorDictionary["em_position"].AddChild(new_w);
				break;
			default:
				break;
		}
	}
}

