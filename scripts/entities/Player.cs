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

	[ExportGroup("PlayerData")]
	[Export]
	public StatsData _Attributes { get; set; }
	[Export]
	public InventoryData _InventoryData { get; set; }
	[Export]
	public Weapon_InventoryData _WeaponInventoryData { get; set; }

	[ExportGroup("Scene Nodes")]	
	[Export]
	public RayCast3D Pointer { get; set; }
	[Export]
	public Array<NodePath> AnchorsPathArray { get; set; }

	public Array<Marker3D> AnchorsArray { get; set; } = new();
	public Dictionary<string, Marker3D> AnchorDictionary { get; set; } = new();
	public AnimationTree _AnimationTree { get; set; }
	
	public override void _Ready() {
		foreach (var anchor in AnchorsPathArray) {
			AnchorsArray.Add(GetNode<Marker3D>(anchor));
		}

		foreach (var anchor in AnchorsArray) {
			AnchorDictionary.Add(anchor.Name, anchor);
		}

		EmitSignal(SignalName.InventoryDataReady, _InventoryData);
		EmitSignal(SignalName.WeaponInventoryDataReady, _WeaponInventoryData);
		_WeaponInventoryData.Connect("InventoryUpdated", new Callable(this, "EquipWeapon"));

		_AnimationTree = GetNode<AnimationTree>("AnimationTree");
	}

	public override void _PhysicsProcess(double delta) {
		if (Pointer.IsColliding() && Input.IsActionJustPressed("pickup_item")) {
			if (Pointer.GetCollider() is StaticBody3D item) {
				GD.Print(item.GetParent().Name + " - " + (item.GetParent() is Weapon));
				PickupItem(item.GetParent());
			}
		};
	}

	public void EquipWeapon(InventoryData inventoryData) {
		if (inventoryData is not Weapon_InventoryData wInv) return;
		
		if (wInv.SlotDatas[0] is not null) {
			if (wInv.SlotDatas[0].ItemData is Weapon_ItemData w) {
				// Animation
				_AnimationTree.Set("parameters/Sword Blend/blend_amount", 1);
				switch (w.Name) {
				case "Excalibur Morgan":
					// Animation
					ClearWeaponHold();
					_AnimationTree.Set("parameters/Transition/transition_request", "sword");
					AnchorDictionary["em_position"].AddChild(InstanceWeapon(w.Model));
					break;
				case "Gae Bolg":
					// Animation
					ClearWeaponHold();
					_AnimationTree.Set("parameters/Transition/transition_request", "lance");
					AnchorDictionary["gb_position"].AddChild(InstanceWeapon(w.Model));
					break;
				case "Sword of Rupture, Ea":
					AnchorDictionary["ea_position"].AddChild(InstanceWeapon(w.Model));
					break;
				default:
					break;
				}
			}
		} else {
			// Animation
			_AnimationTree.Set("parameters/Transition/transition_request", "idle");
			// _AnimationTree.Set("parameters/Sword Blend/blend_amount", 0);
			ClearWeaponHold();
		}
	}

	public void ClearWeaponHold(){
		foreach (var item in AnchorDictionary) {
			if (item.Value.GetChildren().Count == 0) continue;

			foreach (var child in item.Value.GetChildren()) item.Value.RemoveChild(child);
		}
	}

	public Node3D InstanceWeapon(PackedScene weapon) {
		var new_w = (Node3D) weapon.Instantiate().Duplicate();
		new_w.Position = Vector3.Zero;
		new_w.Rotation = Vector3.Zero;
		new_w.Scale = new Vector3(1, 1, 1);
		return new_w;
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

