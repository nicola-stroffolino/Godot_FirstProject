using Godot;
using System;

public class CameraRoot : Spatial {
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
		GetNode<ClippedCamera>("Horizontal/Vertical/ClippedCamera").AddException(GetParent());
	}

	public override void _Input(InputEvent ev) {
		if (ev is InputEventMouseMotion m) {
			HCamRotation -= m.Relative.x * HSensitivity;
			VCamRotation -= m.Relative.y * VSensitivity;
			VCamRotation = Mathf.Clamp(VCamRotation, VCam_Min, VCam_Max);
			GetNode<Spatial>("Horizontal").RotationDegrees = new Vector3 {
				y = HCamRotation + 180 // 180 to flip the character
			};
			GetNode<Spatial>("Horizontal/Vertical").RotationDegrees = new Vector3 {
				x = VCamRotation
			};
		}
	}
}
