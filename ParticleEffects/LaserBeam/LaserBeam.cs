using Godot;
using System;

public partial class LaserBeam : Node2D {
	private bool setup = false;

	private Line2D line = null;
	private GpuParticles2D castingParticles = null;
	private GpuParticles2D impactingParticles = null;
	private GpuParticles2D beamParticles = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.line = this.GetNode<Line2D>("Line2D");
		this.line.Visible = false;
		this.castingParticles = this.GetNode<GpuParticles2D>("CastingParticles");
		this.impactingParticles = this.GetNode<GpuParticles2D>("ImpactingParticles");
		this.beamParticles = this.GetNode<GpuParticles2D>("BeamParticles");
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

		GD.Print($"LaserBeam, from {destination} to {source} ({(viewportRect.Size / 2)})");


		// Reset the Line2D node's start and end point
		this.line.Points = new[] { source, Vector2.Zero };
		this.line.Visible = true;

		Vector2 castingPosition = this.GlobalPosition + source;
		this.castingParticles.GlobalPosition = castingPosition;
		this.castingParticles.Rotation = castingPosition.AngleToPoint(this.GlobalPosition);
		this.castingParticles.Emitting = true;

		this.impactingParticles.GlobalPosition = this.GlobalPosition;
		this.impactingParticles.Rotation = this.GlobalPosition.AngleToPoint(castingPosition);
		this.impactingParticles.Emitting = true;

		this.beamParticles.Position = source * 0.5f;
		this.beamParticles.Rotation = castingPosition.AngleToPoint(this.GlobalPosition);
		var material = this.beamParticles.ProcessMaterial as ParticleProcessMaterial;
		var extents = material.EmissionBoxExtents;
		extents.X = source.Length() * 0.5f;
		material.EmissionBoxExtents = extents;
		this.beamParticles.Emitting = true;
	}
}
