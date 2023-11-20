using System;
using Godot;

public partial class Player : CharacterBody3D {
	[Export]
	private int StructureDistance = 4;
	
	private Node3D Main;
	private ulong StructureId = 0;
	private bool InBuildingMode = false;
	private Vector3 Direction;


	public override void _Ready() {
		Main = GetNode<Node3D>("..");
	}

	public override void _Process(double delta) {
		if (InBuildingMode) ProjectStructureDisplay(delta);
	}

	private void DisplayStructurePreview(PackedScene structure, bool selectionCycled) {
		InBuildingMode = true;
		if (!selectionCycled && StructureId != 0) {
			(InstanceFromId(StructureId) as Node).QueueFree();
			StructureId = 0;
			InBuildingMode = false;
			return;
		}

		if (selectionCycled) {
			(InstanceFromId(StructureId) as Node).QueueFree();
		}

		var s = structure.Instantiate();
		StructureId = s.GetInstanceId();
		Main.AddChild(s);
	}

	private void Build(PackedScene structure) {
		var old_s = (MeshInstance3D) InstanceFromId(StructureId);
		old_s.CreateConvexCollision();

		var new_s = (MeshInstance3D) structure.Instantiate();
		StructureId = new_s.GetInstanceId();

		DisplayStructurePreview(structure, true);
	}

	private void ProjectStructureDisplay(double delta) {
		var s = (MeshInstance3D) InstanceFromId(StructureId);
		
		var HCamRotation = GetNode<Node3D>("CameraComponent/Horizontal").GlobalTransform.Basis.GetEuler().Y;
		var a = GetNode<Node3D>("Armature");
		s.Rotation = new Vector3 {
			X = s.Rotation.X,
			Y = (float)Mathf.LerpAngle(a.Rotation.Y, HCamRotation + (float)Math.PI, delta * 7),
			Z = s.Rotation.Z
		};

		s.Position = new Vector3 {
			X = (float)(Math.Sin(a.Rotation.Y) * StructureDistance + Position.X),
			Y = Position.Y + 2,
			Z = (float)(Math.Cos(a.Rotation.Y) * StructureDistance + Position.Z)
		};
	}
	
	private void GetDirection(Vector3 direction){
		Direction = direction.Normalized();
	}
}



