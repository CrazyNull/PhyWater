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
        if (GUILayout.Button("创建 Physical Cells"))
        {
            pw.CreatePhyCells();
        }

        if (GUILayout.Button("清理 Physical Cells"))
        {
            pw.ClearPhyCells();
        }

        if(!Application.isPlaying)
            pw.ApplyRenderPara();
    }
}
