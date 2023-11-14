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
	[Signal]
	public delegate void JumpedEventHandler();
	
	public override void _Ready() {
	}
	
	public override void _PhysicsProcess(double delta) {
		var Direction = new Vector3 {
			X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			Y = 0,
			Z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
		};
		EmitSignal(SignalName.DirectionState, Direction);
		EmitSignal(SignalName.SprintState, Input.IsActionPressed("sprint"));
		if (Input.IsActionPressed("jump")) EmitSignal(SignalName.Jumped);
	}
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion m) EmitSignal(SignalName.CameraRotation, m);
//		if (@event is InputEventKey a) {
//			GD.Print("George Lefter");
//			if (a.IsActionPressed("jump")) EmitSignal(SignalName.Jumped);
//		}
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
				GetTree().Quit();
	}
}
