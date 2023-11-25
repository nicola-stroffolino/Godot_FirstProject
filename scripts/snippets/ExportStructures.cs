using Godot;
using System;

[Tool]
public partial class ExportStructures : Node {
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
		
	}
}
