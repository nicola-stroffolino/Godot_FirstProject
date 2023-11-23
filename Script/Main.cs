using Godot;
using Godot.NativeInterop;
using System;

public partial class Main : Node3D {
	private const float UNIT = 3.5f;
	private const float DEPTH = 0.1f;
	private BaseMaterial3D StructMat = ResourceLoader.Load<BaseMaterial3D>("res://Art/structure_material.tres");
	public override void _Ready() {
		StructMat.CullMode = BaseMaterial3D.CullModeEnum.Disabled;
		var vertices = new Vector3[] {
			new Vector3(0, 1, 0),
			new Vector3(UNIT, 1, 0),
			new Vector3(UNIT, 1, .5f)
			// new Vector3(.5f, 1, .5f),
			// new Vector3(0, 0, 0),
			// new Vector3(1, 0, 0),
		};

		// Initialize the ArrayMesh.
		var arrMesh = new ArrayMesh();
		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);
		arrays[(int)Mesh.ArrayType.Vertex] = vertices;

		// Create the Mesh.
		arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
		var m = new MeshInstance3D {
			Mesh = arrMesh,
			Name = "Prova",
			MaterialOverride = StructMat
		};




		//m.SetSurfaceOverrideMaterial(0, StructMat);
		
		AddChild(m);
	}
}
