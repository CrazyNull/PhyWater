using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWater : PhyWater
{

    public Renderer Renderer = null;

    public float WaveSpeed = 1.0f;

    public float WaveHeight1 = 0.25f;
    public float WaveLenght1 = 1f;

    public float WaveHeight2 = 0.25f;
    public float WaveLenght2 = 1f;

    protected float time => Time.time * WaveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        this.ApplyRenderPara();
    }

    // Update is called once per frame
    void Update()
    {
        this.ApplyRenderPara();
    }

    private void FixedUpdate()
    {
        this.RefreshPhyMesh();
    }

    protected override Vector3 CalculationPos(Vector3 worldPos)
    {
        Vector3 result = WaveSimulation.SimulationDefaultWave(worldPos, this.time, WaveHeight1, WaveLenght1, WaveHeight2, WaveLenght2);
        return result;
    }

    public void ApplyRenderPara()
    {
        this.Renderer.sharedMaterial.SetFloat("_WaveHeight1", this.WaveHeight1);
        this.Renderer.sharedMaterial.SetFloat("_WaveLenght1", this.WaveLenght1);
        this.Renderer.sharedMaterial.SetFloat("_WaveHeight2", this.WaveHeight2);
        this.Renderer.sharedMaterial.SetFloat("_WaveLenght2", this.WaveLenght2);
        this.Renderer.sharedMaterial.SetFloat("_time", this.time);
    }
}
