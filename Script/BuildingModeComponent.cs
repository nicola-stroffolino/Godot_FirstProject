using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public partial class BuildingModeComponent : Node {
	[Signal]
	public delegate void SelectedStructureEventHandler(MeshInstance3D structure, bool selectionCycled);
	[Signal]
	public delegate void BuildStructureEventHandler(MeshInstance3D structure);
	
	[Export]
	public PanelContainer IconsPanel;

	public MeshInstance3D[] Structures;
	private bool InBuildingMode = false;
	private IEnumerable<PanelContainer> StructureIcons;
	private int StructureSelection = 0;
	
	public override void _Ready() {
		IconsPanel.Visible = false;
		StructureIcons = IconsPanel.GetNode<HBoxContainer>("HBoxContainer").GetChildren().Cast<PanelContainer>();
		StructureSelection = StructureIcons.First().GetIndex();
		StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = true;

		Structures = new MeshInstance3D[StructureIcons.Count()];
		var scn = ResourceLoader.Load<PackedScene>("res://scenes/structures.tscn");
		var mesh = (Node3D) scn.Instantiate();
		var children = mesh.GetChildren();
		for (int i = 0; i < Structures.Length; i++) {
			GD.Print(children[i]);
			Structures[i] = (MeshInstance3D) children[i].Duplicate(); // Duplicate is ESSENTIAL !!
		}
		GD.Print(Structures);
	}	
	
	public override void _Process (double delta) {
		if (!InBuildingMode) return;
	}
	
	private void SwitchBuildingMode() {
		InBuildingMode = !InBuildingMode;
		if (InBuildingMode) IconsPanel.Visible = true;
		else IconsPanel.Visible = false;
		
		EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection], false);
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
		
		EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection], true);
	}
	
	private void Build() {
		EmitSignal(SignalName.BuildStructure, Structures[StructureSelection]);
	}
}

