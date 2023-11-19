using Godot;
using System;

public partial class Floor : MeshInstance3D {
	[Export] public StructureSize StructureSettings;

	public override void _Ready() {
		if (Mesh is BoxMesh m) {
			m.Size = new Vector3((float)StructureSettings.LongSide, (float)StructureSettings.Depth, (float)StructureSettings.LongSide);
			Mesh = m;	
		}
	}
}
