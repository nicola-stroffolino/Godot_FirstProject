using Godot;
using System;

[GlobalClass]
public partial class StatsData : Resource {
	[Export]
	public double MaxHealth { get; set; }
	[Export]
	public double BaseAttack { get; set; }
	[Export]
	public int BaseSpeed { get; set; }
	[Export]
	public double Stamina { get; set; }
	[Export]
	public double Mana { get; set; }
}
