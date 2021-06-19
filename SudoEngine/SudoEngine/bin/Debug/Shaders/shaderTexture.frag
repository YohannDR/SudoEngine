#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform float weight[5] = float[] (0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216);
uniform bool isBlur = false;
uniform int layer;
uniform float transparency;

void main()
{
    if (isBlur)
    {
        vec2 tex_offset = 1.0 / textureSize(texture0, 0);
        vec3 result = texture2D(texture0, texCoord).rgb * weight[0];
        for(int i = 1; i < 5; ++i)
        {
            result += texture(texture0, texCoord + vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
            result += texture(texture0, texCoord - vec2(tex_offset.x * i, 0.0)).rgb * weight[i];
        }
        outputColor = vec4(result, texture2D(texture0, texCoord).a);
    }
    else outputColor = texture2D(texture0, texCoord);
    if (outputColor.a < 0.1) discard;
}