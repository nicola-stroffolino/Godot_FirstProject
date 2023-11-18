using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Player : CharacterBody3D {
	private Node3D Main;
	private Node Structures;
	
	public override void _Ready() {
		Main = GetNode<Node3D>("..");
		Structures = GD.Load<PackedScene>("res://Scenes/structures.tscn").Instantiate();
	}
	
	private void Build(long selection) {
		var s = Structures.GetChild((int)selection + 1);
		Structures.RemoveChild(s);
		Main.AddChild(s);
		s.QueueFree();
	}
}



