using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveSimulation 
{
    public static Vector3 SimulationDefaultWave(Vector3 worldPos,
        float time, 
        float WaveHeight1, 
        float WaveLenght1, 
        float WaveHeight2, 
        float WaveLenght2)
    {
        Vector3 result = worldPos;
        float y = WaveHeight1 * Mathf.Sin(WaveLenght1 * result.x + time);
        result.y += y;
        y = WaveHeight2 * Mathf.Cos(WaveLenght2 * result.z + time);
        result.y += y;
        return result;
    }

    public static Vector3 SimulationGerstnerWave(Vector3 worldPos,
        float time,
        float WaveHeight,
        float WaveLenght)
    {
        Vector3 result = worldPos;
        result.x += WaveHeight * Mathf.Cos(WaveLenght * worldPos.x + time);
        result.y += WaveHeight * Mathf.Sin(WaveLenght * worldPos.x + time);
        return result;
    }
}
