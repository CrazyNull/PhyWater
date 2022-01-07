inline float4 SimulationDefaultWave(float4 worldPos,
    float time,
    float _WaveHeight1,
    float _WaveLenght1,
    float _WaveHeight2,
    float _WaveLenght2)
{
    float4 result = worldPos;
    float y = _WaveHeight1 * sin(_WaveLenght1 * result.x + time);
    result.y += y;
    y = _WaveHeight2 * cos(_WaveLenght2 * result.z + time);
    result.y += y;
    return result;
}

struct GerstnerResult
{
    float4 pos;
    float3 normal;
};

inline GerstnerResult SimulationGerstnerWave(float4 worldPos,
        float time,
        float WaveHeight,
        float WaveLenght)
{

    GerstnerResult result;

    result.pos = worldPos;
    float k = 2 * 3.141592654 / WaveLenght;
    float f = k * (worldPos.x + time);
    result.pos.x += WaveHeight * cos(f);
    result.pos.y += WaveHeight * sin(f);

    float3 t = normalize(float3(1 - k * WaveHeight * sin(f) ,k * WaveHeight * cos(f),0));
    result.normal = float3(-t.y,t.x,0);

    return result;
}