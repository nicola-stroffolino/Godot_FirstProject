using Godot;
using System;

public partial class HealthComponent : Node3D {
	[Export]
	public double MaxHealth { get; set; }
	public double Health { get; set; }
	public PhysicsBody3D Actor { get; set; }
	public TextureProgressBar Healthbar { get; set; }

	public override void _Ready() {
		Actor = GetNode<PhysicsBody3D>("..");
		Healthbar = GetNode<TextureProgressBar>("Health Bar 3D/Bridge/Health Bar 2D");
		
		Health = MaxHealth;
		Healthbar.Value = 1;
	}

	public void TakeDamage(double amount) {
		Health -= amount;

		if (Health <= 0)
			if (Actor is TestDummy) Health += MaxHealth;
			else Health = 0;

		Healthbar.Value = Health / MaxHealth;

		if (Healthbar.Value <= 0.25) Healthbar.TintProgress = Healthbar.TintProgress.Blend(new Color(1, 0, 0));
		else if (Healthbar.Value <= 0.5) Healthbar.TintProgress = Healthbar.TintProgress.Blend(new Color(1, 1, 0));
		else Healthbar.TintProgress = Healthbar.TintProgress.Blend(new Color(0, 1, 0));
	}
}
