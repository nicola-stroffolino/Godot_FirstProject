using Godot;
using System;

public partial class CameraComponent : Node3D {
	[Export]
	public float HSensitivity { get; set; } = .1f;
	[Export]
	public float VSensitivity { get; set; } = .1f;
	public float HCamRotation { get; set; } = 0;
	public float VCamRotation { get; set; } = 0;
	public float VCam_Min { get; set; } = -55f;
	public float VCam_Max { get; set; } = 75f;
	
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		//GetNode<Camera3D>("Horizontal/Vertical/Camera3D").AddException(GetParent());
	}

	public override void _Input(InputEvent ev) {
		if (ev is InputEventMouseMotion m) {
			HCamRotation -= m.Relative.X * HSensitivity;
			VCamRotation -= m.Relative.Y * VSensitivity;
			VCamRotation = Mathf.Clamp(VCamRotation, VCam_Min, VCam_Max);
			GetNode<Node3D>("Horizontal").RotationDegrees = new Vector3 {
				Y = HCamRotation + 180 // 180 to flip the character
			};
			GetNode<Node3D>("Horizontal/Vertical").RotationDegrees = new Vector3 {
				X = VCamRotation
			};
		}
	}
}
