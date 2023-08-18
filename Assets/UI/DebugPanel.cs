using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DebugPanel : MonoBehaviour
{
    public TeamController TeamA;
    public UnitSelectScript uss;

    public TMP_Text autoR_T;
    public TMP_Text drag_T;


    public void Start()
    {
        //switch_AutoRecruit();
        switch_DragSelect();
    }
    public void switch_AutoRecruit()
    {
        Debug.Log("!");
     
        if (TeamA.isControl_By_AIPlayer == true)
        {
            TeamA.isControl_By_AIPlayer = false;
            autoR_T.text = "On";
        }

        else
        {
            TeamA.isControl_By_AIPlayer = true;
            autoR_T.text = "Off";
        }
    }

    public void switch_DragSelect()
    {
        Debug.Log("!");
        if (uss.isEnable == true)
        {
            uss.isEnable = false;
            drag_T.text = "On";
        }

        else
        {
            uss.isEnable = true;
            drag_T.text = "Off";
        }
    }

}
