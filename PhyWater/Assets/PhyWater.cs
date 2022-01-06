using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyWater : MonoBehaviour
{
    public Vector3 Size = new Vector3(3, 1, 2);
    public float PhyCellRadius = 0.1f;
    public Renderer Renderer = null;

    public float WaveSpeed = 1.0f;

    public float WaveHeight1 = 0.25f;
    public float WaveLenght1 = 1f;
    public Vector3 WaveOffset1 = new Vector3();

    public float WaveHeight2 = 0.25f;
    public float WaveLenght2 = 1f;
    public Vector3 WaveOffset2 = new Vector3();


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
    private void LateUpdate()
    {
        this.RefreshCellPos();
    }

    public void CreatePhyCells()
    {
        Vector3 startPos = new Vector3(-this.Size.x + this.PhyCellRadius ,0,this.Size.z  - this.PhyCellRadius);
        for (int i = 0; i < this.Size.z / this.PhyCellRadius; ++i)
        {
            for (int j = 0; j < this.Size.x / this.PhyCellRadius; ++j)
            {
                GameObject go = new GameObject("Phy Cell");
                go.transform.parent = this.transform;
                go.layer = 4;
                CapsuleCollider collider = go.AddComponent<CapsuleCollider>();
                collider.radius = this.PhyCellRadius;
                collider.height = this.Size.y;
                go.transform.localPosition = new Vector3(startPos.x + this.PhyCellRadius * 2f * j, 0, startPos.z - this.PhyCellRadius * 2f * i);
            }
        }
    }

    public void ClearPhyCells()
    {
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            GameObject.DestroyImmediate(this.transform.GetChild(0).gameObject);
            --i;
        }
    }


    public Vector3 CalculationPos(Vector3 localPos)
    {
        Vector3 result = localPos;

        float y = WaveHeight1 * Mathf.Sin(WaveLenght1 * result.x + this.time) + WaveOffset1.y + result.y;
        result.y = y;

        y = WaveHeight2 * Mathf.Cos(WaveLenght2 * result.z + this.time) + WaveOffset2.y + result.y;
        result.y = y;


        return result;
    }

    public void ApplyRenderPara()
    {
        this.Renderer.sharedMaterial.SetFloat("_WaveHeight1", this.WaveHeight1);
        this.Renderer.sharedMaterial.SetFloat("_WaveLenght1", this.WaveLenght1);
        this.Renderer.sharedMaterial.SetVector("_WaveOffset1", this.WaveOffset1);

        this.Renderer.sharedMaterial.SetFloat("_WaveHeight2", this.WaveHeight2);
        this.Renderer.sharedMaterial.SetFloat("_WaveLenght2", this.WaveLenght2);
        this.Renderer.sharedMaterial.SetVector("_WaveOffset2", this.WaveOffset2);

        this.Renderer.sharedMaterial.SetFloat("_time", this.time);
    }

    protected void RefreshCellPos()
    {
        for(int i=0;i < this.transform.childCount;++i)
        {
            Transform child = this.transform.GetChild(i);
            Vector3 wolrdPos = child.position;
            wolrdPos.y = 0;
            child.position = this.CalculationPos(wolrdPos);
        }
    }
}
