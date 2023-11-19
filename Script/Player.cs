using Godot;

public partial class Player : CharacterBody3D {
	private Node3D Main;
	private ulong StructureId = 0;
	private bool positioning = false;

	public override void _Ready() {
		Main = GetNode<Node3D>("..");
	}

	public override void _PhysicsProcess(double delta) {
		// var b = (Node)InstanceFromId(StructureId);
		// GD.Print(b.Name);
	}

	private void DisplayStructurePreview(PackedScene structure, bool selectionCycled) {
		if (!selectionCycled && StructureId != 0) {
			(InstanceFromId(StructureId) as Node).QueueFree();
			StructureId = 0;
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
		
	}
}
