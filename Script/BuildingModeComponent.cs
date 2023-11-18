using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public partial class BuildingModeComponent : Node {
	[Signal]
	public delegate void BuildSelectedEventHandler(int selection);
	[Export]
	private PanelContainer IconsPanel;
	private bool InBuildingMode = false;
	private IEnumerable<PanelContainer> StructureIcons;
	private int StructureSelection;
	
	public override void _Ready() {
		IconsPanel.Visible = false;
		StructureIcons = IconsPanel.GetNode<HBoxContainer>("HBoxContainer").GetChildren().Cast<PanelContainer>();
		StructureSelection = StructureIcons.First().GetIndex();
		StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = true;
	}
	
	public override void _Process (double delta) {
		if (!InBuildingMode) return;
	}
	
	private void SwitchBuildingMode() {
		InBuildingMode = !InBuildingMode;
		if (InBuildingMode) IconsPanel.Visible = true;
		else IconsPanel.Visible = false;
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
	}
	
	private void Build() {
		EmitSignal(SignalName.BuildSelected, StructureSelection);
	}
}


