using System;
using Godot;

public partial class Player : CharacterBody3D {
	[Export]
	private int StructureDistance = 4;
	[Export]
	public LineEdit Output { get; set; }
	
	public bool IsInBuildingMode { get; set; } = false;
	
	private Node3D Main;
	private ulong StructureId = 0;

	public override void _Ready() {
		Main = GetNode<Node3D>("..");
	}
	
	public override void _Process(double delta) {
		//if (IsInBuildingMode) ProjectStructureDisplay(delta);
	}

	public void DisplayStructurePreview(MeshInstance3D structure) {
		Main.AddChild(structure);
	}

	public void DisposeStructurePreview(ulong structure_id) {
		var s = (Node3D) InstanceFromId(structure_id);
		s.QueueFree();
	}

	public void ProjectStructureDisplay(MeshInstance3D structure, double delta) {
		var a = GetNode<Node3D>("Armature");

		var rad = a.Rotation.Y;
		var deg = Mathf.RadToDeg(rad) % 360;
		int rounded_deg = (int)(Math.Round(deg / 90) * 90) + 180;

		structure.Rotation = new Vector3 {
			X = structure.Rotation.X,
			Y = Mathf.DegToRad(rounded_deg),
			Z = structure.Rotation.Z
		};

		structure.Position = new Vector3 {
			X = (float)(Math.Sin(a.Rotation.Y) * StructureDistance + Position.X),
			Y = Position.Y,
			Z = (float)(Math.Cos(a.Rotation.Y) * StructureDistance + Position.Z)
		};

		int x_tiles = (int) Math.Round(structure.Position.X / 4);
		int y_tiles = (int) Math.Round(structure.Position.Y / 4);
		int z_tiles = (int) Math.Round(structure.Position.Z / 4);

		structure.Position = new Vector3 {
			X = x_tiles * 4,
			Y = y_tiles * 4 + 1,
			Z = z_tiles * 4
		};
		
		Output.Text = structure.Position.ToString() + " - " + rounded_deg;
	}

	public void Build(MeshInstance3D structure) {
		var new_s = (MeshInstance3D) structure.Duplicate();
		new_s.CreateTrimeshCollision();

		Main.AddChild(new_s);
	}
}



