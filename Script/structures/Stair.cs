using Godot;
using System;

public partial class Stair : MeshInstance3D {
	[Export] public StructureSize StructureSettings;

	public override void _Ready() {
		if (Mesh is BoxMesh m) {
			float Hypotenuse = (float)Math.Sqrt(StructureSettings.LongSide * StructureSettings.LongSide + StructureSettings.ShortSide * StructureSettings.LongSide);
			m.Size = new Vector3((float)StructureSettings.LongSide, Hypotenuse, (float)StructureSettings.Depth);
			Mesh = m;
			Transform = Transform.Rotated(new Vector3(1, 0, 0), (float)Math.Asin(StructureSettings.ShortSide / Hypotenuse));
		}
	}
}
