using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitInfantryController))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnitInfantryController unit = (UnitInfantryController)target;


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

[CustomEditor(typeof(UnitDragonController))]
public class UnitEditor4 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UnitDragonController script = (UnitDragonController)target;

        if (GUILayout.Button("SetUp"))
            script.SetUp();
    }
}

[CustomEditor(typeof(ViewPoint))]
public class UnitEditorVP : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ViewPoint VP = (ViewPoint)target;

        if (GUILayout.Button("Copy Vectors"))
            VP.CopyEyesVectors();

        if (GUILayout.Button("Paste Vectors"))
            VP.PasteEyesVectors();
    }
}




