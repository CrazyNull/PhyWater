using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PhyWater))]
public class PhyWaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PhyWater pw = this.target as PhyWater;
        if (!Application.isPlaying)
        {
            if (pw.UsePhyMesh)
            {
                pw.ClearPhyCells();
            }
            else
            {
                if (GUILayout.Button("���� Physical Cells"))
                {
                    pw.CreatePhyCells();
                }
                if (GUILayout.Button("���� Physical Cells"))
                {
                    pw.ClearPhyCells();
                }
            }
            pw.ApplyRenderPara();
        }
    }
}
