using Godot;
using System;
using System.Drawing.Drawing2D;

public class player : KinematicBody {
	[Export]
	public int WalkingSpeed { get; set; } = 5; //km/h
	[Export]
	public int RunningSpeed { get; set; } = 15; //km/h
	[Export]
	public float TimeToJumpPeak { get; set; } = .3f; //second
	[Export]
	public int JumpHeight { get; set; } = 5; //meter

	private int ActualSpeed;
	private float Gravity;
	private float JumpSpeed;
	private float AngularAcceleration = 7;

	public override void _Ready() {
		Gravity = 2 * JumpHeight / (TimeToJumpPeak*TimeToJumpPeak); //m/s^2
		JumpSpeed = Gravity * TimeToJumpPeak; //m/s
	}
	
	private Vector3 Velocity = Vector3.Zero;
	private Vector3 StrafeDir = Vector3.Zero;
	private Vector3 Strafe = Vector3.Zero;
	public override void _PhysicsProcess(float delta) {
		float inertia = delta * 3;
		if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_back") || Input.IsActionPressed("move_forward")){
			var HCamRotation = GetNode<Spatial>("CameraRoot/Horizontal").GlobalTransform.basis.GetEuler().y;
			Vector3 Direction = new Vector3 {
				x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
				y = 0,
				z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
			}.Rotated(Vector3.Up, HCamRotation).Normalized();

			var Armature = GetNode<Spatial>("Armature");
			Armature.Rotation = new Vector3 {
				x = (float)Math.PI/2, // 90 deg = π/2
				y = Mathf.LerpAngle(Armature.Rotation.y, HCamRotation + (float)Math.PI, delta * AngularAcceleration)
			};

			if (Input.IsActionPressed("sprint")) ActualSpeed = RunningSpeed;
			else ActualSpeed = WalkingSpeed;

			Velocity.x = Mathf.Lerp(Velocity.x, Direction.x * ActualSpeed, inertia);
			Velocity.z = Mathf.Lerp(Velocity.z, Direction.z * ActualSpeed, inertia);
			MovementStatus(ActualSpeed == RunningSpeed ? 1f : 0f, delta);
		} else {
			Velocity.x = Mathf.Lerp(Velocity.x, 0f, inertia);
			Velocity.z = Mathf.Lerp(Velocity.z, 0f, inertia);
			MovementStatus(-1f, delta);
		}

		GD.Print(GetNode<AnimationTree>("AnimationTree").Get("parameters/iwr_blend/blend_amount"));
		MoveAndSlide(Velocity, Vector3.Up);
	}

	private void Jump() => Velocity.y = JumpSpeed;
	private void SufferGravity(float delta) => Velocity.y -= Gravity * delta;
	private void MovementStatus(float value, float delta) => GetNode<AnimationTree>("AnimationTree").Set(
		"parameters/iwr_blend/blend_amount", Mathf.Lerp(
			(float)GetNode<AnimationTree>("AnimationTree").Get("parameters/iwr_blend/blend_amount"), value, delta * 3
		)
	);
}
