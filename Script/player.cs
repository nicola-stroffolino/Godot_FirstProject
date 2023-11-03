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

	private Vector3 Direction = Vector3.Zero;
	private Vector3 Velocity = Vector3.Zero;
	private Vector3 StrafeDirection = Vector3.Zero;
	private Vector3 Strafe = Vector3.Zero;
	public override void _PhysicsProcess(float delta) {
		float inertia = delta * 3;
		if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_back") || Input.IsActionPressed("move_forward")) {
			var HCamRotation = GetNode<Spatial>("CameraRoot/Horizontal").GlobalTransform.basis.GetEuler().y;
			Direction = new Vector3 {
				x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
				y = 0,
				z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
			};
			StrafeDirection = Direction; // Direction copy without rotation and normalization
			Direction = Direction.Rotated(Vector3.Up, HCamRotation).Normalized();

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
			StrafeDirection = Vector3.Zero;
			MovementStatus(-1f, delta);
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump")) {
			Velocity.y = JumpSpeed;
		} else if (!IsOnFloor()) {
			Velocity.y -= Gravity * delta;
		} else {
			Velocity.y = 0;
		}

		Strafe = Strafe.LinearInterpolate(StrafeDirection, inertia);

		GetNode<AnimationTree>("AnimationTree").Set("parameters/walk_blend/blend_position", new Vector2(-Strafe.x, -Strafe.z));
		GetNode<AnimationTree>("AnimationTree").Set("parameters/run_blend/blend_position", new Vector2(-Strafe.x, -Strafe.z));

		MoveAndSlide(Velocity, Vector3.Up);
	}

	private void MovementStatus(float value, float delta) => GetNode<AnimationTree>("AnimationTree").Set(
		"parameters/iwr_blend/blend_amount", Mathf.Lerp(
			(float)GetNode<AnimationTree>("AnimationTree").Get("parameters/iwr_blend/blend_amount"), value, delta * 3
		)
	);
}
