using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public partial class BuildingModeComponent : Node {
	[Signal]
	public delegate void SelectedStructureEventHandler(PackedScene structure, bool selectionCycled);
	[Signal]
	public delegate void BuildStructureEventHandler(PackedScene structure);
	
	[Export]
	public PanelContainer IconsPanel;
	[Export]
	public Godot.Collections.Array<PackedScene> Structures;
	
	private bool InBuildingMode = false;
	private IEnumerable<PanelContainer> StructureIcons;
	private int StructureSelection = 0;

	private const float UNIT = 3.5f / 2;
	private const float DEPTH = 0.1f;
	private const float HEIGHT = 3f / 2;
	private MeshInstance3D Ceiling;
	
	public override void _Ready() {
		IconsPanel.Visible = false;
		StructureIcons = IconsPanel.GetNode<HBoxContainer>("HBoxContainer").GetChildren().Cast<PanelContainer>();
		StructureSelection = StructureIcons.First().GetIndex();
		StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = true;

		Ceiling = CreateCeilingMesh();
	}
	
	public override void _Process (double delta) {
		if (!InBuildingMode) return;
	}
	
	private void SwitchBuildingMode() {
		InBuildingMode = !InBuildingMode;
		if (InBuildingMode) IconsPanel.Visible = true;
		else IconsPanel.Visible = false;
		if (StructureSelection == 3) {
			var scene = new PackedScene();
			scene.Pack(Ceiling);
			EmitSignal(SignalName.SelectedStructure, scene, false);
		} else EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection], false);
	}
	
	private void CycleStructureSelection(long direction) {
		if (!InBuildingMode) return;
		
		StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = false;
		
		if (direction == 'u') StructureSelection = (StructureSelection + 1) % 4;
		else if (direction == 'd') {
			StructureSelection--;
			if (StructureSelection < 0) StructureSelection = 3;
		}

		StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = true;
		
		if (StructureSelection == 3) {
			var scene = new PackedScene();
			scene.Pack(Ceiling);
			EmitSignal(SignalName.SelectedStructure, scene, true);
		} else EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection], true);
	}
	
	private void Build() {
		if (StructureSelection == 3) {
			var scene = new PackedScene();
			scene.Pack(Ceiling);
			EmitSignal(SignalName.BuildStructure, scene);
		} else EmitSignal(SignalName.BuildStructure, Structures[StructureSelection]);
	}

	private MeshInstance3D CreateCeilingMesh() {
		BaseMaterial3D StructMat = ResourceLoader.Load<BaseMaterial3D>("res://Art/structure_material.tres");
		StructMat.CullMode = BaseMaterial3D.CullModeEnum.Disabled;

		var TOP_LEFT = new Vector3(UNIT, 0, UNIT);
		var TOP_RIGHT = new Vector3(-UNIT, 0, UNIT);
		var BOTTOM_RIGHT = new Vector3(-UNIT, 0, -UNIT);
		var BOTTOM_LEFT = new Vector3(UNIT, 0, -UNIT);

		var TOP_LEFT_INNER = new Vector3(UNIT - DEPTH, 0, UNIT - DEPTH);
		var TOP_RIGHT_INNER = new Vector3(-(UNIT - DEPTH), 0, UNIT - DEPTH);
		var BOTTOM_RIGHT_INNER = new Vector3(-(UNIT - DEPTH), 0, -(UNIT - DEPTH));
		var BOTTOM_LEFT_INNER = new Vector3(UNIT - DEPTH, 0, -(UNIT - DEPTH));

		var PEAK = new Vector3(0, HEIGHT, 0);
		var PEAK_INNER = new Vector3(0, HEIGHT - DEPTH, 0);

		var vertices = new Vector3[] {
			// FRAME
			TOP_LEFT, TOP_RIGHT, TOP_LEFT_INNER,
			TOP_RIGHT, TOP_LEFT_INNER, TOP_RIGHT_INNER,
			
			TOP_RIGHT, BOTTOM_RIGHT, TOP_RIGHT_INNER,
			BOTTOM_RIGHT, TOP_RIGHT_INNER, BOTTOM_RIGHT_INNER,
			
			BOTTOM_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT_INNER,
			BOTTOM_LEFT, BOTTOM_RIGHT_INNER, BOTTOM_LEFT_INNER,
			
			BOTTOM_LEFT, TOP_LEFT, BOTTOM_LEFT_INNER,
			TOP_LEFT, BOTTOM_LEFT_INNER, TOP_LEFT_INNER,

			//PYRAMID
			TOP_LEFT_INNER, TOP_RIGHT_INNER, PEAK_INNER,
			TOP_RIGHT_INNER, BOTTOM_RIGHT_INNER, PEAK_INNER,
			BOTTOM_RIGHT_INNER, BOTTOM_LEFT_INNER, PEAK_INNER,
			BOTTOM_LEFT_INNER, TOP_LEFT_INNER, PEAK_INNER,

			TOP_LEFT, TOP_RIGHT, PEAK,
			TOP_RIGHT, BOTTOM_RIGHT, PEAK,
			BOTTOM_RIGHT, BOTTOM_LEFT, PEAK,
			BOTTOM_LEFT, TOP_LEFT, PEAK
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
			Name = "Ceiling",
			MaterialOverride = StructMat
		};

		return m;
	}
}

