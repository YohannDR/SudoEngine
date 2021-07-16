#version 330 core

layout (triangles) in;
layout (triangle_strip, max_vertices = 2) out;

in vec2 texCoord;
out vec2 texCoords;

uniform int tile_data[10] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

void main() {
	texCoords = texCoord;
	EndPrimitive();
}