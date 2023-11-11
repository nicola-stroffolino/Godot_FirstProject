using System;
using Godot;

public partial class MovementComponent : Node {
	[Export]
	private CharacterBody3D Actor;
	[Export]
	public int WalkingSpeed { get; set; } = 5; //km/h
	[Export]
	public int RunningSpeed { get; set; } = 15; //km/h
	[Export]
	public float TimeToJumpPeak { get; set; } = .3f; //second
	[Export]
	public int JumpHeight { get; set; } = 2; //meter

	[Signal]
	public delegate void MotionStateEventHandler(double value, double delta);
	[Signal]
	public delegate void JumpStateEventHandler(double value, double delta);

	private int ActualSpeed;
	private float Gravity;
	private float JumpSpeed;
	private float AngularAcceleration = 7;
	private AnimationTree AnimTree;

	public override void _Ready() {
		AnimTree = GetNode<AnimationTree>("../AnimationTree");
	}
	
	public override void _Process(double delta) {
		Gravity = 2 * JumpHeight / (TimeToJumpPeak*TimeToJumpPeak); //m/s^2
		JumpSpeed = Gravity * TimeToJumpPeak; //m/s
	}

	private Vector3 Direction = Vector3.Zero;
	private Vector3 Velocity = Vector3.Zero;
	private Vector3 StrafeDirection = Vector3.Zero;
	private Vector3 Strafe = Vector3.Zero;
	private bool Ascending = false;
	private bool Airborne = false;
	private double DeltaTot = 0f;

	public override void _PhysicsProcess(double delta) {
		double inertia = delta * 3;

		if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_back") || Input.IsActionPressed("move_forward")) {
			var HCamRotation = GetNode<Node3D>("../CameraComponent/Horizontal").GlobalTransform.Basis.GetEuler().Y;
			Direction = new Vector3 {
				X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
				Y = 0,
				Z = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward")
			};
			StrafeDirection = Direction; // Direction copy without rotation and normalization
			Direction = Direction.Rotated(Vector3.Up, HCamRotation).Normalized();

			var Armature = GetNode<Node3D>("../Armature");
			Armature.Rotation = new Vector3 {
				X = (float)Math.PI/2, // 90 deg = π/2
				Y = (float)Mathf.LerpAngle(Armature.Rotation.Y, HCamRotation + (float)Math.PI, delta * AngularAcceleration)
			};

			if (Input.IsActionPressed("sprint")) ActualSpeed = RunningSpeed;
			else ActualSpeed = WalkingSpeed;

			Velocity.X = Mathf.Lerp(Velocity.X, Direction.X * ActualSpeed, (float)inertia);
			Velocity.Z = Mathf.Lerp(Velocity.Z, Direction.Z * ActualSpeed, (float)inertia);
			
			if (ActualSpeed == RunningSpeed) {
				AnimTree.Set("parameters/StateMachine/conditions/idle", false);
				AnimTree.Set("parameters/StateMachine/conditions/walking", false);
				AnimTree.Set("parameters/StateMachine/conditions/running", true);
			} else {
				AnimTree.Set("parameters/StateMachine/conditions/idle", false);
				AnimTree.Set("parameters/StateMachine/conditions/walking", true);
				AnimTree.Set("parameters/StateMachine/conditions/running", false);
			}
			//EmitSignal(SignalName.MotionState, ActualSpeed == RunningSpeed ? 1 : 0, delta);
		} else {
			Velocity.X = Mathf.Lerp(Velocity.X, 0f, (float)inertia);
			Velocity.Z = Mathf.Lerp(Velocity.Z, 0f, (float)inertia);
			StrafeDirection = Vector3.Zero;
			
			AnimTree.Set("parameters/StateMachine/conditions/idle", true);
			AnimTree.Set("parameters/StateMachine/conditions/walking", false);
			AnimTree.Set("parameters/StateMachine/conditions/running", false);
		}

		if (Actor.IsOnFloor()) {
			DeltaTot = 0;
			
			if (Input.IsActionPressed("jump")) {
				Velocity.Y = JumpSpeed;
				Ascending = true;
			}
			
			if (Airborne) {
				Airborne = false;
				AnimTree.Set("parameters/StateMachine/conditions/landed", true);
				AnimTree.Set("parameters/StateMachine/conditions/falling", false);
			} else {
				AnimTree.Set("parameters/StateMachine/conditions/landed", false);
			}
		} else {
			Velocity.Y -= (float)(Gravity * delta);
			Airborne = true;
			AnimTree.Set("parameters/StateMachine/conditions/falling", true);
			
			if (Ascending) {
				DeltaTot += delta;
				//AnimTree.Set("parameters/jump_blend/blend_amount", DeltaTot / TimeToJumpPeak);
				if (DeltaTot >= TimeToJumpPeak) {
					Ascending = false;
				}
			} else {
				DeltaTot -= delta;

				if (DeltaTot <= 0) {
					DeltaTot = 0;
				}
			}
			if (Actor.IsOnFloor()) GD.Print("Just Landed");
		}
		
		

		Strafe = Strafe.Lerp(StrafeDirection, (float)inertia);
		AnimTree.Set("parameters/StateMachine/Walking/blend_position", new Vector2(-Strafe.X, -Strafe.Z));
		AnimTree.Set("parameters/StateMachine/Running/blend_position", new Vector2(-Strafe.X, -Strafe.Z));
		
		Actor.Velocity = Velocity;
		Actor.MoveAndSlide();
	}	
	
	private void OnAnimationFinished(StringName anim_name) {
		//GD.Print(anim_name);
		switch (anim_name) {
			case "falling_to_land":
			case "falling_to_roll":
				//GD.Print("Landing Animation has finished.");
				AnimTree.Set("parameters/jump_blend/blend_amount", 0);
				break;
			default:
				break;
		}
	}
}


