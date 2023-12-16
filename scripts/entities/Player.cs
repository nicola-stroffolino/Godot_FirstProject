using System;
using Godot;

public partial class Player : CharacterBody3D {
	[Signal]
	public delegate void SpreadDamageEventHandler(double amount);

	[Export]
	public WeaponLoadout WeaponLoadout { get; set; }
	[Export]
	public PackedScene Weapon { get; set; }

	public bool IsInBuildingMode { get; set; } = false;
	private Node3D Scene;

	public override void _Ready() {
		Scene = GetNode<Node3D>("..");
	}

	public void DisposeStructure(MeshInstance3D structure) {
		structure.QueueFree();
	}

	public void InstantiateStructure(MeshInstance3D structure) {
		Scene.AddChild(structure);
	}
	
	public void Attack(double damage) {
		EmitSignal(SignalName.SpreadDamage, damage);
	}

	private void EquipWeapon() {
		
	}
}

