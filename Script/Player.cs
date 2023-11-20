using Godot;

public partial class Player : CharacterBody3D {
	private Node3D Main;
	private ulong StructureId = 0;
	private bool InBuildingMode = false;

	public override void _Ready() {
		Main = GetNode<Node3D>("..");
	}

	public override void _Process(double delta) {
		if (InBuildingMode) ProjectStructureDisplay();
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

	private void ProjectStructureDisplay() {
		var s = (MeshInstance3D) InstanceFromId(StructureId);
		s.Position = new Vector3(Position.X + 1, Position.Y + 1, Position.Z + 1);
		GD.Print(Position);
		//s.Transform = s.Transform.Translated(Transform.Basis.);
	}
}
