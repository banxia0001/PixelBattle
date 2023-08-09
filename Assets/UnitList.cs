using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptaleObjects/UnitList")]
public class UnitList : ScriptableObject
{
    public List<UnitInList> UnitPrefabs;
}

[System.Serializable]
public class UnitInList
{
   
    public UnitData.UnitListID ID;
    public GameObject UnitPrefabA;
    public GameObject UnitPrefabB;
    public int Cost;

    public Trait trait1, trait2;

    [TextArea(6,3)]
    public string description;
}
