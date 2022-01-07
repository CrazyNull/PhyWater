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


inline float4 SimulationGerstnerWave(float4 worldPos,
        float time,
        float WaveHeight,
        float WaveLenght)
{
    float4 result = worldPos;
    result.x += WaveHeight * cos(WaveLenght * worldPos.x + time);
    result.y += WaveHeight * sin(WaveLenght * worldPos.x + time);
    return result;
}