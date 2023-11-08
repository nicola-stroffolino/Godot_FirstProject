using Godot;
using System;

public class movement_component : Node {
	[Export]
	public KinematicBody Actor = new KinematicBody();

	public override void _Ready() {
		
	}
}
