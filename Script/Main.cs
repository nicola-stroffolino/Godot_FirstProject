using Godot;
using Godot.NativeInterop;
using System;

public partial class Main : Node3D {
	private const float UNIT = 3.5f / 2;
	private const float DEPTH = 0.1f;
	private const float HEIGHT = 3f / 2;
	private MeshLibrary StructMeshLib = ResourceLoader.Load<MeshLibrary>("res://Art/structures_meshlib.tres");

	public override void _Ready() {
		for (int i = 0; i < StructMeshLib.GetItemList().Length; i++) {
			GD.Print(StructMeshLib.GetItemMesh(i));
			//AddChild((StructMeshLib.GetItemMesh(i) as MeshInstance3D));
		}
	}
}
