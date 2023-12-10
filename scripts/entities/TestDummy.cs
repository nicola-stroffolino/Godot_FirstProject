using Godot;
using System;

public partial class TestDummy : CharacterBody3D {
	public void TakeDamage(double amount) {
		GetNode<HealthComponent>("Health").TakeDamage(amount);
	}
}
