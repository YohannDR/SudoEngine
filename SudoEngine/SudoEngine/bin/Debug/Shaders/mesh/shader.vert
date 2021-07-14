#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform bool camera = false;
uniform bool MoveX = false;
uniform bool start = true;
uniform vec4 position;

void main(void)
{
    texCoord = aTexCoord;

    gl_Position = vec4(aPosition, 1.0);
    //gl_Position = position;
    
    if (MoveX) gl_Position.x += 1;
}