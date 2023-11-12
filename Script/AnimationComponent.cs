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
		//GD.Print("Movement Status has changed: " + value);

		TargetAnimationTree.Set("parameters/iwr_blend/blend_amount", Mathf.Lerp(
			(float)TargetAnimationTree.Get("parameters/iwr_blend/blend_amount"), value, delta * 5
		));
	}
}



