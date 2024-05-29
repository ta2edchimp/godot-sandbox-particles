# Particle & Shader Effects Godot Sandbox

![Red Godot Robot with a Crosshair](icon.svg "Particle & Shader Effects Godot Sandbox")

Self contained Playground / Sandbox for Particle Effects.

### Disclaimer

The project is built using Godot 4.2 .NET, so most code is written in C# aside
from the Shader code, which is written as gdshader code.

The **assets** are, when not noted otherwise, taken from publicly available
tutorials or made by the goat [Kenney](https://kenney.nl/assets).

### Usage Notes

A Particle Effect has to be created as a scene and afterwards to be selected as
a `PackedScene` property of the Main scene. It will then be instantiated at the
mouse position on clicking the left or right mouse button.

The Particle Effect scenes, as of the time of writing this README, are
`GPUParticle2D` scenes, that also contain an `AnimationPlayer` Node that not
only starts off the Particle Effect (turns `Emits` on for one shot Effects),
but also automatically calls the `queue_free` method upon finishing the
auto-played animation.

Some Experiments consist of Shaders, they're to be found within the
`ShaderEffects` directory. The general structure is the same, including auto
cleanup using an `AnimationPlayer` node within.

**Contributions welcome!**
