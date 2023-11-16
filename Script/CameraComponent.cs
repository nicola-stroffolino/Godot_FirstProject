using Godot;
using System;

public partial class CameraComponent : Node {
	[Export]	
	public float HSensitivity { get; set; } = .1f;
	[Export]
	public float VSensitivity { get; set; } = .1f;
	[Signal]
	public delegate void EulerEventHandler();

	private float HCamRotation = 0;
	private float VCamRotation = 0;
	private const float VCam_Min = -55f;
	private const float VCam_Max = 75f;

	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void RotateCamera(InputEventMouseMotion motion) {
		HCamRotation -= motion.Relative.X * HSensitivity;
		VCamRotation -= motion.Relative.Y * VSensitivity;
		VCamRotation = Mathf.Clamp(VCamRotation, VCam_Min, VCam_Max);
		GetNode<Node3D>("Horizontal").RotationDegrees = new Vector3 {
			Y = HCamRotation + 180 // 180 to flip the character
		};
		GetNode<Node3D>("Horizontal/Vertical").RotationDegrees = new Vector3 {
			X = VCamRotation
		};
	}
}
