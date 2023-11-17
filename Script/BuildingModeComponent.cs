using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public partial class BuildingModeComponent : Node {
	[Export]
	private PanelContainer IconsPanel;
	private bool InBuildingMode = false;
	private IEnumerable<TextureRect> Structures;
	private int StructureSelection;
	
	public override void _Ready() {
		IconsPanel.Visible = false;
		Structures = IconsPanel.GetNode<HBoxContainer>("HBoxContainer").GetChildren().Cast<TextureRect>();
		StructureSelection = Structures.First().GetIndex();
		GD.Print(string.Join(", ", Structures.Select(s => s.Name)));
		GD.Print("Currently selected: " + Structures.ElementAt(StructureSelection).Name);
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
		
		if (direction == 'u') StructureSelection++;
		else if (direction == 'd') StructureSelection--;

		StructureSelection %= 4;

		GD.Print("Currently selected: " + Structures.ElementAt(StructureSelection).Name);
	}
}



