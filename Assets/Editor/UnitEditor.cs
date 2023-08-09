using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitAttackController))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnitAttackController unit = (UnitAttackController)target;


        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}


[CustomEditor(typeof(UnitRangerController))]
public class UnitEditor2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnitRangerController unit = (UnitRangerController)target;

        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}


[CustomEditor(typeof(UnitCavalryController))]
public class UnitEditor3 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnitCavalryController unit = (UnitCavalryController)target;

        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}

