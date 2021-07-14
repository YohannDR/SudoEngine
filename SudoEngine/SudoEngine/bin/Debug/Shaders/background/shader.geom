#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 2) out;

in vec2 texCoord;
out vec2 texCoords;

uniform int tile_data[] = int[6] (5, 4, 3, 2, 1, 0);

void main() {
	texCoords = texCoord;
	EndPrimitive();
}