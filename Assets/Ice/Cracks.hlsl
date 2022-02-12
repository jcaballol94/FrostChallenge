#ifndef __ICE_CRACKS_HLSL
#define __ICE_CRACKS_HLSL

SamplerState crack_linear_clamp_sampler;

float3 World2UV(float3 posWS, float scale)
{
    return posWS / scale + float3(0.5,0.5,0.5);
}

float3 UV2World(float3 posUV, float scale)
{
    return (posUV - float3(0.5, 0.5, 0.5)) * scale;
}

void CrackRayMarch_float(UnityTexture2D crackTex, in float3 startPos, in float3 viewDir, in float crackWidth, in float scale, 
    out float3 crackPos)
{
    viewDir /= length(viewDir.xz);
    crackWidth /= scale;
    crackPos = World2UV(startPos, scale);
    float distToCrack = 0;
    int maxIter = 10;
    [loop]
    do
    {
        crackPos += viewDir * distToCrack;
        distToCrack = crackTex.Sample(crack_linear_clamp_sampler, crackPos.xz).x - crackWidth;
        --maxIter;
    } while (distToCrack > 0 && maxIter);
    crackPos = UV2World(crackPos, scale);
}

#endif