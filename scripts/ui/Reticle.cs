using Godot;
using System;

public partial class Reticle : CenterContainer {
	[Export]
	public float DotRadius { get; set; } = 1f;
	[Export]
	public Color DotColor { get; set; } = new Color(1, 1, 1);

	CenterContainer c;

	public override void _Ready() {
		c = GetNode<CenterContainer>("Center");
		c.QueueRedraw();
	}

	public override void _Draw() {
		c.DrawCircle(new Vector2(0, 0), DotRadius, DotColor);
	}
}
