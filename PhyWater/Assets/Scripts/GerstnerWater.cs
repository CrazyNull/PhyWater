using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerstnerWater : PhyWater
{
    public Renderer Renderer = null;

    public float WaveSpeed = 1.0f;


    public float WaveHeight = 0.1f;
    public float WaveLenght = 1f;

    protected float time => Time.time * WaveSpeed;

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
        Vector3 result = WaveSimulation.SimulationGerstnerWave(worldPos,this.time,this.WaveHeight,this.WaveLenght);
        return result;
    }

    public void ApplyRenderPara()
    {
        this.Renderer.sharedMaterial.SetFloat("_WaveHeight", this.WaveHeight);
        this.Renderer.sharedMaterial.SetFloat("_WaveLenght", this.WaveLenght);
        this.Renderer.sharedMaterial.SetFloat("_time", this.time);
    }
}
