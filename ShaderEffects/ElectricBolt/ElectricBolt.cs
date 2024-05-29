using Godot;
using System;

public partial class ElectricBolt : Node2D {
	private bool setup = false;
	private ColorRect colorRect = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.colorRect = this.GetNode<ColorRect>("ColorRect");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (this.setup) {
			return;
		}

		this.setup = true;

		var viewportRect = this.GetViewportRect();
		Vector2 destination = this.GlobalPosition;
		Vector2 viewportCenter = viewportRect.Size;
		Vector2 source = (viewportCenter / 2) - destination;
		Vector2 castingPosition = this.GlobalPosition + source;

		float sizeFactor = castingPosition.DistanceTo(this.GlobalPosition);
		float origSizeX = this.colorRect.Size.X;
		var size = this.colorRect.Size;
		size.X = sizeFactor;
		this.colorRect.Size = size;
		// this.colorRect.Size = new Vector2(sizeFactor, sizeFactor);
		// this.colorRect.Position = new Vector2(0f, sizeFactor / -2f);
		// this.colorRect.PivotOffset = new Vector2(0f, sizeFactor / 2f);
		this.colorRect.Rotation = this.GlobalPosition.AngleToPoint(castingPosition);

		var material = this.colorRect.Material as ShaderMaterial;
		if (material != null) {
			float scale = sizeFactor / origSizeX;
			material.SetShaderParameter("scaling", scale);
			material.SetShaderParameter("speed", .5f * scale);
		}
	}
}
