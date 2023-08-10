using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptaleObjects/UnitData_Local")]
[System.Serializable]
public class UnitData_Local : ScriptableObject
{
    public UnitData.UnitListID ID;
    public GameObject UnitPrefabA;
    public GameObject UnitPrefabB;
    public int Cost;
    public Trait trait1, trait2;
    [TextArea(6, 3)]
    public string description;
    public UnitData data;
}

