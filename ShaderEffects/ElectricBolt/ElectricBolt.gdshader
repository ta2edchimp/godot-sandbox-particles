shader_type canvas_item;

uniform vec3 lightning_color: source_color = vec3(0.2, 0.3, 0.8);
uniform int octave_count: hint_range(1, 20) = 10;
uniform float amplitude_start = 0.5;
uniform float amplitude_coefficent = 0.5;
uniform float frequency_coefficient = 2.0;
uniform float speed = 1;
uniform float scaling = 1.0;

// Pseudo random number generator based on time
float hash12(vec2 x) {
	return fract(cos(mod(dot(x, vec2(13.9898, 8.141)), 3.14)) * 43758.5453);
}

vec2 hash22(vec2 uv) {
	uv = vec2(dot(uv, vec2(127.1, 311.7)),
				dot(uv, vec2(269.5, 183.3)));
				
	return 2.0 * fract(sin(uv) * 43758.5453123) - 1.0;
}

float noise(vec2 uv) {
	// Different noises:
	//return sin(uv.x + uv.y);
	//return sin(uv.x + uv.y);
	//return sin(uv.x + uv.y);
	// noice:
	//return sin(uv.x + uv.y);
	
	vec2 iuv = floor(uv);
	vec2 fuv = fract(uv);
	vec2 blur = smoothstep(0.0, 1.0, fuv);
	
	return mix(mix(dot(hash22(iuv + vec2(0.0, 0.0)), fuv - vec2(0.0, 0.0)),
					dot(hash22(iuv + vec2(1.0, 0.0)), fuv - vec2(1.0, 0.0)), blur.x),
				mix(dot(hash22(iuv + vec2(0.0, 1.0)), fuv - vec2(0.0, 1.0)),
					dot(hash22(iuv + vec2(1.0, 1.0)), fuv - vec2(1.0, 1.0)), blur.x), blur.y) + 0.5;
}

float fractal_brownian_motion(vec2 uv, int octaves) {
	float value = 0.0;
	float amplitude = amplitude_start;
	
	for (int i = 0; i < octaves; i++) {
		value += amplitude * noise(uv);
		uv *= frequency_coefficient;
		amplitude *= amplitude_coefficent;
	}
	
	return value;
}

// Called for every pixel the material is visible on.
void fragment() {
	vec2 uv = (2.0 * UV - 1.0) * vec2(scaling, 1.0);
	uv += 1.5 * fractal_brownian_motion(uv + TIME * speed, octave_count) - 0.75;
	
	// Calculate the distance from the far ends
	// we want the lightning to go from left to right
	// so the distance will be calculated on the y-axis
	float dist = abs(uv.y);
	
	// Add some "random" flicker
	float flicker = mix(0.01, 0.03, hash12(vec2(TIME)));
	
	// Color value at uv coordinates, based on the
	// calculated distance from the left and right edges
	vec3 color = lightning_color * flicker / dist;
	
	// Use color brightness as basis for the alpha channel
	float alpha = (mix(0.0, 1.0, color.b) * 1.2) - 0.2;
	
	COLOR = vec4(color, alpha);
}
