using System;
using Godot;

public partial class Player : CharacterBody3D {
	[Export]
	public LineEdit Output { get; set; }

	public bool IsInBuildingMode { get; set; } = false;
	private Node3D Main;

	public override void _Ready() {
		Main = GetNode<Node3D>("..");
	}

	public void DisplayStructurePreview(MeshInstance3D structure) {
		Main.AddChild(structure);
	}

	public void DisposeStructurePreview(ulong structure_id) {
		var s = (Node3D) InstanceFromId(structure_id);
		s.QueueFree();
	}

	public void Build(MeshInstance3D structure) {
		Main.AddChild(structure);
	}
}



