using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BuildingComponent : Node {
	[Export]
	public Player Actor { get; set; }
	[Export]
	public PackedScene StructuresScene { get; set; }
	[Export]
	public Vector3 TileSize { get; set; } = new Vector3(4, 4, 4);
	[Export]
	public Vector3 TilesStructureDistance { get; set; } = new Vector3(2, 1, 2);
	[Export]
	public PanelContainer IconsPanel { get; set; }

	public Array<MeshInstance3D> Structures { get; set; } = new();
	public int StructureSelection { get; set; } = 0;
	public MeshInstance3D CurrentStructureInstance { get; set; }
	public Vector3 StructureDistance { get; private set; }

	// private IEnumerable<PanelContainer> StructureIcons;
	[Export]
	public LineEdit Output { get; set; }
	
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

		StructureDistance = TilesStructureDistance * TileSize;
	}	
	
	public override void _Process (double delta) {
		if (Actor.IsInBuildingMode) ApplyTransformProperties();
	}
	
	private void SwitchBuildingMode() {
		// if (!Actor.HasMethod("DisplayStructurePreview") || !Actor.HasMethod("DisposeStructurePreview")) return;
		
		Actor.IsInBuildingMode = !Actor.IsInBuildingMode;

		// if (Actor.IsInBuildingMode) IconsPanel.Visible = true;
		// else IconsPanel.Visible = false;


		if (Actor.IsInBuildingMode) {
			ChangeCurrentStructureInstance();
			Actor.InstantiateStructure(CurrentStructureInstance);
		} else Actor.DisposeStructure(CurrentStructureInstance);
		
		//EmitSignal(SignalName.SelectedStructure, Structures[StructureSelection].Duplicate(), false);
	}
	
	private void CycleStructureSelection(long direction) {
		if (!Actor.IsInBuildingMode) return;

		// StructureIcons.ElementAt(StructureSelection).GetNode<TextureRect>("Selection").Visible = false;

		if (direction == 'u') StructureSelection = (StructureSelection + 1) % 4;
		else StructureSelection = (Structures.Count + StructureSelection - 1) % 4;

		Actor.DisposeStructure(CurrentStructureInstance);
		ChangeCurrentStructureInstance();
		Actor.InstantiateStructure(CurrentStructureInstance);
	}

	private void ApplyTransformProperties() {
		var a = Actor.GetNode<Node3D>("Armature") ?? Actor;
		CurrentStructureInstance.Position = new Vector3 {
			X = Actor.Position.X,//(float)(Math.Sin(a.Rotation.Y) + Actor.Position.X),
			Y = Actor.Position.Y,
			Z = Actor.Position.Z//(float)(Math.Cos(a.Rotation.Y) + Actor.Position.Z)
		};
		// SnapRotationY(a.Rotation.Y, 90);
		// SnapToPosition(TileSize.X, TileSize.Y, TileSize.Z);
		Output.Text = "X: " + Math.Round(Math.Sin(a.Rotation.Y)) + " Z: " + Math.Round(Math.Cos(a.Rotation.Y)) + " - " + CurrentStructureInstance.Position.Round() + " - " + Actor.Position.Round();
	}

	private void Build() {
		var new_s = (MeshInstance3D) CurrentStructureInstance.Duplicate();
		new_s.CreateTrimeshCollision();
		if (Actor.IsInBuildingMode) Actor.InstantiateStructure(new_s);
	}



	// Functional Methods
	private void SnapRotationY(double rad_angle, int snap_angle_amplitude) {
		double normalized_angle = Mathf.RadToDeg(rad_angle) % 360;
		int snapped_angle = (int)(Math.Round(normalized_angle / snap_angle_amplitude) * snap_angle_amplitude) + 180;

		CurrentStructureInstance.Rotation = new Vector3 {
			X = CurrentStructureInstance.Rotation.X,
			Y = Mathf.DegToRad(snapped_angle),
			Z = CurrentStructureInstance.Rotation.Z
		};
	}

	private void SnapToPosition(float x, float y, float z) {
		int x_tiles = (int) Math.Round(CurrentStructureInstance.Position.X / x) * (int) x;
		int y_tiles = (int) Math.Round(CurrentStructureInstance.Position.Y / y) * (int) y;
		int z_tiles = (int) Math.Round(CurrentStructureInstance.Position.Z / z) * (int) z;

		CurrentStructureInstance.Position = new Vector3 {
			X = x_tiles, //(int) Math.Round(CurrentStructureInstance.Position.X / x) * x,
			Y = y_tiles, //(int) Math.Round(CurrentStructureInstance.Position.Y / y) * y,
			Z = z_tiles, //(int) Math.Round(CurrentStructureInstance.Position.Z / z) * z
		};
	}

	private void ChangeCurrentStructureInstance() {
		CurrentStructureInstance = (MeshInstance3D) Structures[StructureSelection].Duplicate(); // Duplicate is ESSENTIAL !!
	}
}
