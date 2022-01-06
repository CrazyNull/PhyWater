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

    public bool UsePhyMesh = false;
    protected MeshCollider _meshCollider = null;
    protected Mesh _mesh = null;
    protected Vector3[] _originVS = null;

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
        if (!this.UsePhyMesh)
        {
            this.RefreshCellPos();
        }
        else
        {
            this.RefreshPhyMesh();
        }
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


    public Vector3 CalculationPos(Vector3 worldPos)
    {
        Vector3 result = worldPos;
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
        if (this.UsePhyMesh) return;
        for(int i=0;i < this.transform.childCount;++i)
        {
            Transform child = this.transform.GetChild(i);
            Vector3 wolrdPos = child.position;
            wolrdPos.y = 0;
            child.position = this.CalculationPos(wolrdPos);
        }
    }

    protected void RefreshPhyMesh()
    {
        if (!this.UsePhyMesh) return;
        if (null == this._meshCollider)
        {
            GameObject go = new GameObject("Phy Mesh");
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;

            this._meshCollider = go.AddComponent<MeshCollider>();
            this._mesh = new Mesh();
            this._meshCollider.sharedMesh = this._mesh;

            int x = (int)(this.Size.x * 2 / this.PhyCellRadius) + 1;
            int y = (int)(this.Size.z * 2/ this.PhyCellRadius) + 1;

            Vector3 startPos = new Vector3(-this.Size.x, 0, this.Size.z );

            Vector3[] vertices = new Vector3[x * y];
            this._originVS = new Vector3[x * y];
            for (int i = 0; i < y; ++i)
            {
                for (int j = 0; j < x; ++j)
                {
                    int index = j + x * i;
                    vertices[index] = new Vector3(startPos.x + j * this.PhyCellRadius,this.Size.y * 0.5f, startPos.z - i * this.PhyCellRadius);
                    this._originVS[index] = vertices[index];
                }
            }
            this._meshCollider.sharedMesh.vertices = vertices;
            this._originVS = vertices;

            List<int> triangles = new List<int>();
            for (int i = 0; i < y; ++i)
            {
                if (i != y - 1)
                {
                    for (int j = 0; j < x; ++j)
                    {
                        if (j != x - 1)
                        {
                            triangles.Add(j + x * i);
                            triangles.Add(j + x * i + 1);
                            triangles.Add(j + x * i + x);

                            triangles.Add(j + x * i + 1);
                            triangles.Add(j + x * i + 1 + x);
                            triangles.Add(j + x * i + x);
                        }
                    }
                }
            }
            this._meshCollider.sharedMesh.triangles = triangles.ToArray();

            this._meshCollider.sharedMesh.RecalculateNormals();
            this._meshCollider.sharedMesh.RecalculateTangents();
        }

        Vector3[] vs = this._meshCollider.sharedMesh.vertices;
        for (int i = 0; i < this._originVS.Length; ++i)
        {
            Vector3 pos = this._originVS[i];
            Vector3 worldPos = this._meshCollider.transform.TransformPoint(pos);
            worldPos = this.CalculationPos(worldPos);
            pos = this._meshCollider.transform.InverseTransformPoint(worldPos);
            vs[i] = pos;
        }
        this._meshCollider.sharedMesh.vertices = vs;
        this._meshCollider.sharedMesh = this._mesh;
    }

    private void OnDrawGizmos()
    {
        //if (null != this._meshCollider)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawWireMesh(this._meshCollider.sharedMesh, this.transform.position);
        //}
    }

    private void OnApplicationQuit()
    {
        if (null != this._mesh)
        {
            Object.Destroy(this._mesh);
        }
    }

}
