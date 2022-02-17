#ifndef __CRACKS_HLSL_
#define __CRACKS_HLSL_

void Cracks_float(in float3 Background, in UnityTexture2D CracksTex, in UnityTexture2D Gradient, in float GradientMultiplier,
    in float CrackIntensity, in float Depth, in float2 UV, in float2 ViewTS, in int Steps, out float3 Result)
{
    Result = Background;
    float2 position = -ViewTS.xy * Depth;
    float2 delta = ViewTS.xy * (Depth / Steps);
    float invSteps = 1.0 / Steps;

    for (int i = 0; i < Steps; ++i)
    {
        float value = 1 - tex2D(CracksTex, UV + position + delta * i).x;
        value *= i * invSteps;
        float3 color = tex2D(Gradient, float2((1 - i * invSteps) * GradientMultiplier, 0.5)).xyz * CrackIntensity;
        Result = lerp(Result, color, value);
    }
}
#endif