// public class player : KinematicBody {
// 	[Export]
// 	public int WalkingSpeed { get; set; } = 5; //km/h
// 	[Export]
// 	public int RunningSpeedMultiplier { get; set; } = 3; //times
// 	[Export]
// 	public int JumpHeight { get; set; } = 10; //meter
// 	[Export]
// 	public float Gravity { get; set; } = 9.81f; //m/s^2
// 	private float TimeToJumpPeak;
// 	private float JumpSpeed;
// 	private float AngularAcceleration = 7;

// 	private Vector3 Velocity = Vector3.Zero;
// 	private int ActualSpeed;

// 	public override void _Ready() {
// 		Gravity *= 2; // Makes game more responsive as it shouldn't 100% represent reality
// 		TimeToJumpPeak = (float)Math.Sqrt(2 * JumpHeight / Gravity);
// 		JumpSpeed = Gravity * TimeToJumpPeak; //m/s
// 	}

// 	private Vector3 StrafeDir = Vector3.Zero;
// 	private Vector3 Strafe = Vector3.Zero;
// 	private bool IsInJumpingMotion = false;
// 	private float TimeToJumpCopy = 0f;
// 	private bool IsJumpActionPerformed = false;
// 	public override void _PhysicsProcess(float delta) {
// 		if (!IsOnFloor()) SufferGravity(delta);
// 		else if (Input.IsActionPressed("jump")) {
// 			Jump();
// 			IsInJumpingMotion = IsJumpActionPerformed = true;
// 			TimeToJumpCopy = TimeToJumpPeak;
// 		} else if (IsOnFloor()) Velocity.y = 0;


// 		if (IsJumpActionPerformed){
// 			TimeToJumpCopy -= delta;
// 			if (IsInJumpingMotion) {
// 				GetNode<AnimationTree>("AnimationTree").Set(
// 					"parameters/jump_blend/blend_amount", (TimeToJumpPeak - TimeToJumpCopy) / TimeToJumpPeak
// 				);
// 				if ((float)GetNode<AnimationTree>("AnimationTree").Get("parameters/jump_blend/blend_amount") + delta >= 1f) {
// 					IsInJumpingMotion = false;
// 					TimeToJumpCopy = (float)Math.Sqrt(2 * JumpHeight / Gravity);
// 					// GD.Print("Velocity: " + Velocity.y);
// 					// GD.Print("Jump Speed: " + JumpSpeed);
// 				}
// 			} else {
// 				GetNode<AnimationTree>("AnimationTree").Set(
// 					"parameters/jump_blend/blend_amount", 1 - (TimeToJumpPeak - TimeToJumpCopy) / TimeToJumpPeak					
// 				);
// 				if ((float)GetNode<AnimationTree>("AnimationTree").Get("parameters/jump_blend/blend_amount") - delta <= 0f) IsJumpActionPerformed = false;
// 			}
			
// 			GD.Print((TimeToJumpPeak - TimeToJumpCopy) / TimeToJumpPeak);
// 			GD.Print("copy: " + TimeToJumpCopy);
// 		}
		
// 		if (Input.IsActionPressed("move_right") ||
// 			Input.IsActionPressed("move_left") ||
// 			Input.IsActionPressed("move_back") ||
// 			Input.IsActionPressed("move_forward")
// 		){
// 			var HCamRotation = GetNode<Spatial>("CameraRoot/Horizontal").GlobalTransform.basis.GetEuler().y;

// 			Vector3 Direction = new Vector3 {
// 				x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
// 				y = 0,
// 				z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
// 			};

// 			StrafeDir = Direction;

// 			Direction = Direction.Rotated(Vector3.Up, HCamRotation).Normalized();

// 			Velocity.x = Mathf.Lerp(Velocity.x, Direction.x * ActualSpeed, delta * 3);
// 			Velocity.z = Mathf.Lerp(Velocity.z, Direction.z * ActualSpeed, delta * 3);

// 			// Rotate Armature Facing Direction
// 			var Armature = GetNode<Spatial>("Armature");
// 			Armature.Rotation = new Vector3 {
// 				x = (float)Math.PI/2, // 90 deg = π/2
// 				y = Mathf.LerpAngle(Armature.Rotation.y, HCamRotation + (float)Math.PI, delta * AngularAcceleration)
// 			};

// 			if (Input.IsActionPressed("sprint")) {
// 				ActualSpeed = WalkingSpeed * RunningSpeedMultiplier;
// 				MovementStatus(1, delta);
// 			} else {
// 				ActualSpeed = WalkingSpeed;
// 				MovementStatus(0, delta);
// 			}
// 		} else {	
// 			Velocity.x = Mathf.Lerp(Velocity.x, 0f, delta * 3);
// 			Velocity.z = Mathf.Lerp(Velocity.z, 0f, delta * 3);
// 			StrafeDir = Vector3.Zero;
// 			MovementStatus(-1, delta);
// 		}

// 		Strafe = Strafe.LinearInterpolate(StrafeDir, delta * 3);

// 		GetNode<AnimationTree>("AnimationTree").Set("parameters/walk_blend/blend_position", new Vector2(-Strafe.x, -Strafe.z));
// 		GetNode<AnimationTree>("AnimationTree").Set("parameters/run_blend/blend_position", new Vector2(-Strafe.x, -Strafe.z));

// 		MoveAndSlide(Velocity, Vector3.Up);
// 	}

// 	private void Jump() => Velocity.y = JumpSpeed;
// 	private void SufferGravity(float delta) => Velocity.y -= Gravity * delta;
// 	private void MovementStatus(float value, float delta) => GetNode<AnimationTree>("AnimationTree").Set(
// 		"parameters/iwr_blend/blend_amount", Mathf.Lerp(
// 			(float)GetNode<AnimationTree>("AnimationTree").Get("parameters/iwr_blend/blend_amount"), value, delta * 3
// 		)
// 	);
// }
using Godot;
using System;

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

		if (IsOnFloor() && Input.IsActionPressed("jump")) {
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
