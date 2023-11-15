using Godot;
using System;

public partial class InputComponent : Node {
	[Signal]
	public delegate void DirectionStatusEventHandler(Vector3 direction);
	[Signal]
	public delegate void SprintStatusEventHandler(bool sprinting);
	[Signal]
	public delegate void CameraStatusEventHandler(InputEventMouseMotion motion);
	[Signal]
	public delegate void JumpActionEventHandler();
	
	public override void _PhysicsProcess(double delta) {
		var Direction = new Vector3 {
			X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			Y = 0,
			Z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
		};
		EmitSignal(SignalName.DirectionStatus, Direction);
		EmitSignal(SignalName.SprintStatus, Input.IsActionPressed("sprint"));
		if (Input.IsActionPressed("jump")) EmitSignal(SignalName.JumpAction);
	}
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion m) EmitSignal(SignalName.CameraStatus, m);
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
				GetTree().Quit();
	}
}
