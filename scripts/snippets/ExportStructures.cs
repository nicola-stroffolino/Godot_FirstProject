using Godot;
using System;
using System.Linq;

[Tool]
public partial class ExportStructures : Node3D {
	private bool _exportAsScenes;
	[Export]
	public bool ExportAsScenes { 
		get { 
			return _exportAsScenes; 
		}
		set {
			_exportAsScenes = value;
			Export();
		}
	}

	private void Export() {
		if (!_exportAsScenes) return;

		var a = GetChildren().Duplicate().Cast<MeshInstance3D>();
		foreach (var s in a) {
			s.Position = Vector3.Zero;
			s.CreateTrimeshCollision();
			if (s.HasNode($"{s.Name}_col/CollisionShape3D")) GD.Print("Ok");
			else GD.Print("Failure");
		}
	}
}
