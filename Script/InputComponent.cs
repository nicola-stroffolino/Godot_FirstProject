using Godot;
using System;

public partial class InputComponent : Node {
	[Signal]
	public delegate void DirectionStateEventHandler(Vector3 direction);
	[Signal]
	public delegate void SprintStateEventHandler(bool sprinting);
	[Signal]
	public delegate void VelocityEventHandler(bool moving, double delta);
	[Signal]
	public delegate void CameraRotationEventHandler(InputEventMouseMotion motion);
	
	public override void _Ready() {
	}
	
	public override void _PhysicsProcess(double delta) {
		if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_back") || Input.IsActionPressed("move_forward")) {
			var Direction = new Vector3 {
				X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
				Y = 0,
				Z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
			};
			EmitSignal(SignalName.DirectionState, Direction);
			EmitSignal(SignalName.SprintState, Input.IsActionPressed("sprint"));
			EmitSignal(SignalName.Velocity, true, delta);
		} else {
			EmitSignal(SignalName.Velocity, false, delta);
		}
	}
	
	public override void _Input(InputEvent ev) {
		if (ev is InputEventMouseMotion m) EmitSignal(SignalName.CameraRotation, m);
	}
}
