using Godot;

public partial class Wall : MeshInstance3D {
	[Export] public StructureSize StructureSettings;

	public override void _Ready() {
		if (Mesh is BoxMesh m) {
			m.Size = new Vector3((float)StructureSettings.LongSide, (float)StructureSettings.ShortSide, (float)StructureSettings.Depth);
			Mesh = m;	
		}
	}
}
