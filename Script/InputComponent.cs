using Godot;
using System;

public partial class InputComponent : Node {
	[Signal]
	public delegate void DirectionStatusEventHandler(Vector3 direction);
	[Signal]
	public delegate void StrafeStatusEventHandler(Vector3 direction);
	[Signal]
	public delegate void SprintStatusEventHandler(bool sprinting);
	[Signal]
	public delegate void CameraStatusEventHandler(InputEventMouseMotion motion);
	[Signal]
	public delegate void JumpStatusEventHandler();
	[Signal]
	public delegate void BuildingModeStatusEventHandler();
	[Signal]
	public delegate void ScrollStatusEventHandler(char direction);
	[Signal]
	public delegate void LeftClickEventHandler();
	
	public override void _PhysicsProcess(double delta) {
		var Direction = new Vector3 {
			X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			Y = 0,
			Z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
		};
		EmitSignal(SignalName.DirectionStatus, Direction);
		EmitSignal(SignalName.StrafeStatus, Direction);
		EmitSignal(SignalName.SprintStatus, Input.IsActionPressed("sprint"));
		if (Input.IsActionPressed("jump")) EmitSignal(SignalName.JumpStatus);
		if (Input.IsActionJustPressed("building_mode")) EmitSignal(SignalName.BuildingModeStatus);
		if (Input.IsActionJustPressed("left_click")) EmitSignal(SignalName.LeftClick);
	}
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion m) EmitSignal(SignalName.CameraStatus, m);
		if (@event is InputEventKey || @event is InputEventMouseButton){
			if (@event.IsActionPressed("next_building")) EmitSignal(SignalName.ScrollStatus, 'u');
			if (@event.IsActionPressed("previous_building")) EmitSignal(SignalName.ScrollStatus, 'd');
		}
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventKey eventKey)
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
				GetTree().Quit();
	}
}
