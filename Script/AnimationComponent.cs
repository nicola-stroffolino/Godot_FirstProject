using Godot;
using System;

public partial class AnimationComponent : Node {
	public override void _Ready(){
		//var ciao = GetNode<MovementComponent>("").Connect(SignalName.MovementStatusChanged, );
		//(Owner as Node3D).Connect("MovementStatusChanged", Callable.From(() => Owner as Node3D));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_movement_component_movement_status_changed(double value)
	{
		GD.Print("Movement Status has changed: " + value);
	}
}


