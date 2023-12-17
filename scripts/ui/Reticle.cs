using Godot;
using System;

public partial class Reticle : CenterContainer {
	[Export]
	public float DotRadius { get; set; } = 1f;
	[Export]
	public Color DotColor { get; set; } = new Color(1, 1, 1);

	public override void _Ready() {
		QueueRedraw();
	}

	public override void _Draw() {
		DrawCircle(new Vector2(0, 0), DotRadius, DotColor);
	}
}
