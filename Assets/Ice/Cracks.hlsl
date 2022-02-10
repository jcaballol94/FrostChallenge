#ifndef __ICE_CRACKS_HLSL
#define __ICE_CRACKS_HLSL

SamplerState crack_linear_clamp_sampler;

float2 World2UV(float3 posWS, float scale)
{
    return posWS.xz / scale + float2(0.5, 0.5);
}

void CrackRayMarch_float(UnityTexture2D crackTex, in float3 startPos, in float3 viewDir, in float crackWidth, in float scale, 
    out float3 crackPos, out float2 crackUV)
{
    viewDir /= length(viewDir.xz);
    crackPos = startPos;
    float distToCrack = 0;
    int maxIter = 10;
    [loop]
    do
    {
        crackPos += viewDir * distToCrack;
        crackUV = World2UV(crackPos, scale);
        distToCrack = crackTex.Sample(crack_linear_clamp_sampler, crackUV).x * scale - crackWidth;
        --maxIter;
    } while (distToCrack > 0.001 && maxIter);
}

#endif