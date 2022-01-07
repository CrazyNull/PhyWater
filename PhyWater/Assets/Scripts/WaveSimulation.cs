using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveSimulation 
{
    public static Vector3 SimulationDefaultWave(Vector3 worldPos,float time, float WaveHeight1, float WaveLenght1, Vector3 WaveOffset1, float WaveHeight2, float WaveLenght2, Vector3 WaveOffset2)
    {
        Vector3 result = worldPos;
        float y = WaveHeight1 * Mathf.Sin(WaveLenght1 * result.x + time) + WaveOffset1.y;
        result.y = y;
        y = WaveHeight2 * Mathf.Cos(WaveLenght2 * result.z + time) + WaveOffset2.y;
        result.y = y;
        return result;
    }
}
