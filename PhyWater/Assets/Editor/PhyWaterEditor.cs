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
        if (GUILayout.Button("���� Physical Cells"))
        {
            pw.CreatePhyCells();
        }

        if (GUILayout.Button("���� Physical Cells"))
        {
            pw.ClearPhyCells();
        }

        if(!Application.isPlaying)
            pw.ApplyRenderPara();
    }
}
