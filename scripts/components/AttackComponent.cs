using Godot;
using System;

public partial class AttackComponent : Node {
	[Export]
	public double Damage { get; set; }
	
	public Player Actor { get; set; }
	
	public override void _Ready() {
		Actor = GetNode<Player>("..");
	}
	
	public void Attack() {
		Actor.Attack(Damage);
	}
}
