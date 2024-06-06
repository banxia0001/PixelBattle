using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanel : MonoBehaviour
{
    public TMP_Text stats,unitName,unitDes,Gcost,Tcost;
    public Image image;

    public TraitButtonPanel trait1, trait2;
    public void InputUnit(UnitData_Local unit)
    {
        //string range = "Melee";
        //if (unit.data.unitType == UnitData.UnitType.archer || unit.data.unitType == UnitData.UnitType.artillery)
        //{
        //    range = unit.data.shootDis.ToString();
        //}
        //stats.text = "Health:" + unit.data.health + "\n" +
        //    "Damage:" + unit.data.damageMin + "~" + unit.data.damageMax + "\n" +
        //    "Armor:" + unit.data.armor + "\n" +
        //    "Range:" + range + "\n" +
        //    "Speed:" + unit.data.moveSpeed;

        //unitName.text = unit.ID.ToString();

        //string des = "";

        //Trait _trait1 = null;
        //Trait _trait2 = null;

        //des = unit.description;
        //_trait1 = unit.trait1;
        //_trait2 = unit.trait2;

        //unitDes.text = des;
        //trait1.InputTrait(_trait1);
        //trait2.InputTrait(_trait2);
        //Gcost.text = unit.Gcost.ToString() + "G";
        //Tcost.text = unit.Tcost.ToString() + "S";

        //image.sprite = unit.unitSpriteA;
    }
}
