using Godot;
using System;

public partial class Main : Node2D {
	[Export]
	public PackedScene UseEffectOnLmb = null;
	[Export]
	public PackedScene UseEffectOnRmb = null;

	private Sprite2D center = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		this.center = this.GetNode<Sprite2D>("Center");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("left-click")) {

			GD.Print(this.GetGlobalMousePosition());

			if (this.UseEffectOnLmb == null) {
				GD.PushError("No Effect to spawn on LMB Click chosen!");
				return;
			}

			// var instance = scene.Instantiate<Node2D>();
			var instance = this.UseEffectOnLmb.Instantiate<Node2D>();

			this.AddChild(instance);
			instance.GlobalPosition = this.GetGlobalMousePosition();

		} else if (Input.IsActionJustPressed("right-click")) {

			GD.Print(this.GetGlobalMousePosition());

			if (this.UseEffectOnRmb == null) {
				GD.PushError("No Effect to spawn on RMB Click chosen!");
				return;
			}

			// var instance = scene.Instantiate<Node2D>();
			var instance = this.UseEffectOnRmb.Instantiate<Node2D>();

			this.AddChild(instance);
			instance.GlobalPosition = this.GetGlobalMousePosition();

		} else if (Input.IsActionJustPressed("quit")) {
			this.GetTree().Quit();
		}
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (this.center == null) {
			return;
		}

		var mousePosition = this.GetGlobalMousePosition();
		var centerPosition = this.GetViewportRect().Size / 2;
		var rotation = centerPosition.AngleToPoint(mousePosition);
		this.center.Rotation = rotation;
	}
}
