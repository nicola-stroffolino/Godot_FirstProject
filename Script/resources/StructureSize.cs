using Godot;
using System;

[GlobalClass]
public partial class StructureSize : Resource {
	[Export]
	public double LongSide;
	[Export]
	public double ShortSide;
	[Export]
	public double Depth;
}
