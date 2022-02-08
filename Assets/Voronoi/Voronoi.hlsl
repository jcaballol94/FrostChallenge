#ifndef _VORONOI_HLSL
#define _VORONOI_HLSL

// Base implementation from https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Voronoi-Node.html
// Distance to edge from https://www.ronja-tutorials.com/post/028-voronoi-noise/

inline float2 VoronoiNoiseRandomVector (float2 uv, float offset)
{
    float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
    uv = frac(sin(mul(uv, m)) * 46839.32);
    return float2(sin(uv.y * offset) * 0.5 + 0.5, cos(uv.x * offset) * 0.5 + 0.5);
}

void Voronoi_float(in float2 uv, in float angleOffset, in float cellDensity, out float distToEdge)
{
    float2 cellCorner = floor(uv * cellDensity);
    float2 samplePoint = frac(uv * cellDensity);

    float3 closest = float3(8.0, 0.0, 0.0);
    float2 toClosest;
    for (int y = -1; y <= 1; ++y)
    {
        for (int x = -1; x <= 1; ++x)
        {
            float2 lattice = float2(x, y);
            float2 offset = VoronoiNoiseRandomVector(lattice + cellCorner, angleOffset);
            float2 cell = lattice + offset;
            float2 toCell = cell - samplePoint;
            float d = length(toCell);
            if (d < closest.x)
            {
                closest = float3(d, cell.x, cell.y);
                toClosest = toCell;
            }
        }
    }
    float minEdgeDistance = 10.0;
    for (int y2 = -1; y2 <= 1; ++y2)
    {
        for (int x2 = -1; x2 <= 1; ++x2)
        {
            float2 lattice = float2(x2, y2);
            float2 offset = VoronoiNoiseRandomVector(lattice + cellCorner, angleOffset);
            float2 cell = lattice + offset;
            float2 toCell = cell - samplePoint;
            float2 diffClosestCell = abs(closest.yz - cell);

            if ((diffClosestCell.x + diffClosestCell.y) > 0.1)
            {
                float2 toCenter = (toClosest + toCell) * 0.5;
                float2 cellDifference = normalize(toCell - toClosest);
                float edgeDistance = dot(toCenter, cellDifference);
                minEdgeDistance = min(minEdgeDistance, edgeDistance);
            }
        }
    }

    distToEdge = minEdgeDistance / cellDensity;
}

#endif