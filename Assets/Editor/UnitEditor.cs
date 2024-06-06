using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Infantry))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Infantry unit = (Infantry)target;


        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}


[CustomEditor(typeof(Ranger))]
public class UnitEditor2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Ranger unit = (Ranger)target;

        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}

[CustomEditor(typeof(Spearman))]
public class UnitEditor_Spearman : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Spearman unit = (Spearman)target;

        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}

[CustomEditor(typeof(Cavarly))]
public class UnitEditor3 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Cavarly unit = (Cavarly)target;

        if (GUILayout.Button("SetUpBone"))
            unit.SetUp();
    }
}

[CustomEditor(typeof(DragonMonster))]
public class UnitEditor4 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DragonMonster script = (DragonMonster)target;

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




