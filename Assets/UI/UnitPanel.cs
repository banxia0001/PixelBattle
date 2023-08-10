using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanel : MonoBehaviour
{
    //public Unit test;

    //public void Start()
    //{
    //    InputUnit(test);
    //}
    public TMP_Text stats,unitName,unitDes,Gcost;
    public Image image;

    public TraitButtonPanel trait1, trait2;
    public void InputUnit(Unit unit)
    {
        string range = "Melee";
        if (unit.data.unitType == UnitData.UnitType.archer || unit.data.unitType == UnitData.UnitType.artillery)
        {
            range = unit.data.shootDis.ToString();
        }
        stats.text = "Health:" + unit.data.health + "\n" +
            "Damage:" + unit.data.damageMin + "~" + unit.data.damageMax + "\n" +
            "Armor:" + unit.data.armor + "\n" +
            "Range:" + range + "\n" +
            "Speed:" + unit.data.moveSpeed;

        unitName.text = unit.data_local.ID.ToString();


        UnitList list = Resources.Load<UnitList>("List");
        string des = "";

        Trait _trait1 = null;
        Trait _trait2 = null;
        int gold = 0;
        foreach (UnitData_Local _unit in list.UnitPrefabs)
        {
            if (_unit.ID == unit.data_local.ID)
            {
                des = _unit.description;
                _trait1 = _unit.trait1;
                _trait2 = _unit.trait2;
                gold = _unit.Cost;
            }
        }

        unitDes.text = des;
        trait1.InputTrait(_trait1);
        trait2.InputTrait(_trait2);
        Gcost.text = gold.ToString();
    }
}
