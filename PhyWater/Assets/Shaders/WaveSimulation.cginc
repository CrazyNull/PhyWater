inline float4 SimulationDefaultWave(float4 worldPos,float time,float _WaveHeight1,float _WaveLenght1,float3 _WaveOffset1 ,float _WaveHeight2,float _WaveLenght2,float3 _WaveOffset2)
{
    float4 result = worldPos;
    float y = _WaveHeight1 * sin(_WaveLenght1 * result.x + time) + _WaveOffset1.y;
    result.y += y;
    y = _WaveHeight2 * cos(_WaveLenght2 * result.z + time) + _WaveOffset2.y;
    result.y += y;
    return result;
}
