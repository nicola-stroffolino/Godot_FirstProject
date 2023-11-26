using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BuildingComponent : Node {
	// [Signal]
	// public delegate void SelectedStructureEventHandler(MeshInstance3D structure, bool selectionCycled);
	// [Signal]
	// public delegate void BuildStructureEventHandler(MeshInstance3D structure);
	
	[Export]
	public Player Actor { get; set; }
	[Export]
	public PackedScene StructuresScene { get; set; }
	[Export]
	public PanelContainer IconsPanel { get; set; }

	public Array<MeshInstance3D> Structures { get; set; } = new();
	public int StructureSelection { get; set; } = 0;
	public MeshInstance3D CurrentStructureInstance { get; set; }
	// private IEnumerable<PanelContainer> StructureIcons;
	
	public override void _Ready() {
		// IconsPanel.Visible = false;
		// StructureIcons = IconsPanel.GetNode<HBoxContainer>("HBoxContainer").GetChildren().Cast<PanelContainer>();
		// StructureSelection = StructureIcons.First().GetIndex();
		// StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = true;

		var scene = (Node3D) StructuresScene.Instantiate();
		var mesh_list = scene.GetChildren();
		Structures.Resize(mesh_list.Count);
		for (int i = 0; i < Structures.Count; i++) {
			Structures[i] = (MeshInstance3D) mesh_list[i];
		}
	}	
	
	public override void _Process (double delta) {
		if (Actor.IsInBuildingMode) Actor.ProjectStructureDisplay(CurrentStructureInstance, delta);
	}
	
	private void SwitchBuildingMode() {
		Actor.IsInBuildingMode = !Actor.IsInBuildingMode;
		GD.Print(Actor.IsInBuildingMode);

		// if (Actor.IsInBuildingMode) IconsPanel.Visible = true;
		// else IconsPanel.Visible = false;

		if (!Actor.HasMethod("DisplayStructurePreview") || !Actor.HasMethod("DisposeStructurePreview")) return;

		if (Actor.IsInBuildingMode) {
			ChangeCurrentStructureInstance();
			Actor.DisplayStructurePreview(CurrentStructureInstance);
		} else Actor.DisposeStructurePreview(CurrentStructureInstance.GetInstanceId());
		
		//EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection].Duplicate(), false);
	}
	
	private void CycleStructureSelection(long direction) {
		if (!Actor.IsInBuildingMode) return;

		// StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = false;

		if (direction == 'u') StructureSelection = (StructureSelection + 1) % 4;
		else StructureSelection = (Structures.Count + StructureSelection - 1) % 4;

		Actor.DisposeStructurePreview(CurrentStructureInstance.GetInstanceId());
		ChangeCurrentStructureInstance();
		Actor.DisplayStructurePreview(CurrentStructureInstance);
	}
	
	private void Build() {
		if (Actor.IsInBuildingMode) Actor.Build(CurrentStructureInstance);
	}

	private void ChangeCurrentStructureInstance() {
		CurrentStructureInstance = (MeshInstance3D) Structures[StructureSelection].Duplicate(); // Duplicate is ESSENTIAL !!
	}
}
