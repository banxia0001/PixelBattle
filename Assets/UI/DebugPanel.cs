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

    public void switch_AutoRecruit()
    {
        if (TeamA.isAIControl == true)
        {
            TeamA.isAIControl = false;
            autoR_T.text = "On";
        }

        else
        {
            TeamA.isAIControl = true;
            autoR_T.text = "Off";
        }
    }
}
