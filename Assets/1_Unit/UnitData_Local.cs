using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptaleObjects/Card")]
[System.Serializable]
public class UnitData_Local : ScriptableObject
{
    [Header("Basic Stats")]
    public string[] names;
    [TextArea(6, 3)]
    public string[] descriptions;
    public Sprite[] unitSprites;

    [Header("Recruit Stats")]
    public UnitData.UnitListID ID;
    public GameObject[] prefabs;

    [Range(1, 500)] public int Gcost;
    [Range(1, 50)] public int Tcost;
    [Range(1, 9)] public int Num;
    [Range(1, 15)] public int UnitValue;

    [Header("Unit Stats")]
    public Trait trait1, trait2;
    public UnitData data;
}
