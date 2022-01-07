using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyWater : MonoBehaviour
{
    public Vector3 Size = new Vector3(6, 2, 4);
    public float PhyCellRadius = 0.5f;

    protected MeshCollider _meshCollider = null;
    protected Mesh _mesh = null;
    protected List<Vector3> _originVS = null;

    protected virtual Vector3 CalculationPos(Vector3 worldPos)
    {
        return worldPos;
    }

    protected void RefreshPhyMesh()
    {
        if (null == this._meshCollider)
        {
            GameObject go = new GameObject("Phy Mesh");
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;
            go.layer = 4;

            this._meshCollider = go.AddComponent<MeshCollider>();
            this._mesh = new Mesh();
            this._meshCollider.sharedMesh = this._mesh;

            int x = (int)(this.Size.x * 2 / this.PhyCellRadius) + 1;
            int y = (int)(this.Size.z * 2 / this.PhyCellRadius) + 1;

            Vector3 startPos = new Vector3(-this.Size.x, 0, this.Size.z);

            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < y; ++i)
            {
                for (int j = 0; j < x; ++j)
                {
                    Vector3 p = new Vector3(startPos.x + j * this.PhyCellRadius, this.Size.y * 0.5f, startPos.z - i * this.PhyCellRadius);
                    vertices.Add(p);
                }
            }
            for (int i = 0; i < y; ++i)
            {
                for (int j = 0; j < x; ++j)
                {
                    Vector3 p = new Vector3(startPos.x + j * this.PhyCellRadius, this.Size.y * -0.5f, startPos.z - i * this.PhyCellRadius);
                    vertices.Add(p);
                }
            }

            this._meshCollider.sharedMesh.vertices = vertices.ToArray();
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

            for (int i = 0; i < y; ++i)
            {
                if (i != y - 1)
                {
                    for (int j = 0; j < x; ++j)
                    {
                        if (j != x - 1)
                        {
                            triangles.Add(j + x * i + x + (x * y));
                            triangles.Add(j + x * i + 1 + (x * y));
                            triangles.Add(j + x * i + (x * y));

                            triangles.Add(j + x * i + x + (x * y));
                            triangles.Add(j + x * i + 1 + x + (x * y));
                            triangles.Add(j + x * i + 1 + (x * y));
                        }
                    }
                }
            }

            int top1 = 0;
            int top2 = x - 1;
            int top3 = x * (y - 1);
            int top4 = x * y - 1;

            for (int i = 0; i < x; ++i)
            {
                if (i != x - 1)
                {
                    int s = top3 + i;

                    triangles.Add(s);
                    triangles.Add(s + 1);
                    triangles.Add(s + x * y);

                    triangles.Add(s + 1);
                    triangles.Add(s + 1 + x * y);
                    triangles.Add(s + x * y);
                }
            }
            for (int i = 0; i < x; ++i)
            {
                if (i != x - 1)
                {
                    int s = top2 - i;

                    triangles.Add(s);
                    triangles.Add(s - 1);
                    triangles.Add(s + x * y);

                    triangles.Add(s - 1);
                    triangles.Add(s - 1 + x * y);
                    triangles.Add(s + x * y);
                }
            }

            for (int i = 0; i < y; ++i)
            {
                if (i != y - 1)
                {
                    int s = top1 + i * x;

                    triangles.Add(s);
                    triangles.Add(s + x);
                    triangles.Add(s + x * y);

                    triangles.Add(s + x);
                    triangles.Add(s + x + x * y);
                    triangles.Add(s + x * y);
                }
            }


            for (int i = 0; i < y; ++i)
            {
                if (i != y - 1)
                {
                    int s = top4 - i * x;

                    triangles.Add(s);
                    triangles.Add(s - x);
                    triangles.Add(s + x * y);

                    triangles.Add(s - x);
                    triangles.Add(s - x + x * y);
                    triangles.Add(s + x * y);
                }
            }



            this._meshCollider.sharedMesh.triangles = triangles.ToArray();

            this._meshCollider.sharedMesh.RecalculateNormals();
            this._meshCollider.sharedMesh.RecalculateTangents();

        }

        Vector3[] vs = this._meshCollider.sharedMesh.vertices;
        for (int i = 0; i < this._originVS.Count; ++i)
        {
            Vector3 pos = this._originVS[i];
            if (pos.y > 0)
            {
                Vector3 worldPos = this._meshCollider.transform.TransformPoint(pos);
                worldPos = this.CalculationPos(worldPos);
                pos = this._meshCollider.transform.InverseTransformPoint(worldPos);
                vs[i] = pos;
            }
        }
        this._meshCollider.sharedMesh.vertices = vs;
        this._meshCollider.sharedMesh.RecalculateNormals();
        this._meshCollider.sharedMesh = this._mesh;
    }

    protected void OnApplicationQuit()
    {
        if (null != this._mesh)
        {
            Object.Destroy(this._mesh);
        }
    }
}
