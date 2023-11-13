using Godot;
using System;

public partial class AnimationComponent : Node {
	[Export]
	private AnimationTree TargetAnimationTree;

	public override void _Ready(){
		//var ciao = GetNode<MovementComponent>("").Connect(SignalName.MovementStatusChanged, );
		//(Owner as Node3D).Connect("MovementStatusChanged", Callable.From(() => Owner as Node3D));
	}

	private void MotionStatusChanged(double value, double delta) {
		TargetAnimationTree.Set("parameters/animation_state_machine/movement/iwr_blend/blend_amount", Mathf.Lerp(
			(float)TargetAnimationTree.Get("parameters/animation_state_machine/movement/iwr_blend/blend_amount"), value, delta * 5
		));
	}

	private void JumpStateChanged(bool value) {
		TargetAnimationTree.Set("parameters/animation_state_machine/conditions/airborne", value);
		TargetAnimationTree.Set("parameters/animation_state_machine/conditions/landed", !value);
	}

	private void StrafeStateChanged(Vector2 strafe) {
		TargetAnimationTree.Set("parameters/animation_state_machine/movement/walk_blend/blend_position", strafe);
		TargetAnimationTree.Set("parameters/animation_state_machine/movement/run_blend/blend_position", strafe);
	}
}





