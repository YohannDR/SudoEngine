#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform float transparency;

void main()
{
    if (transparency != 0) 
    {
        vec4 textureTransparency = texture2D(texture0, texCoord);
        textureTransparency.a = 1 - transparency;
        outputColor = textureTransparency;
    }
    else outputColor = texture2D(texture0, texCoord);
}