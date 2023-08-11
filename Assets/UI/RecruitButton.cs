using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecruitButton : MonoBehaviour
{
    [HideInInspector]
    public TeamController TC;
    [HideInInspector]
    public int RNumber;
    [HideInInspector]
    public UnitData_Local unit;

    public Image image;
    public BarController T_Bar;
    public GameObject T_Image;
    public TMP_Text text;
    public TMP_Text text_HotKey;

    private float Rtimer;

    public Animator anim;

    public enum ButtonState { WaitingForRecruit, TrainingUnit, UnitReady }
    public ButtonState buttonState;

    public void FixedUpdate()
    {
        if (buttonState == ButtonState.TrainingUnit)
        {
            Rtimer -= Time.fixedDeltaTime;
            T_Bar.SetValue(Rtimer, unit.Tcost);
            if (Rtimer < 0)
            {
                UnitReady();
            }
        }

    }

    public void InputData(TeamController TC, UnitData_Local unit, int hotkey)
    {
        this.TC = TC;
        this.unit = unit;
        int num = hotkey + 1;
        text.text = "X" + unit.Num + "<color=#FFD800>|" + unit.Gcost + "G" + "</color>";
        if (TC.unitTeam == Unit.UnitTeam.teamA)
            image.sprite = unit.unitSpriteA;

        else
        {
            image.sprite = unit.unitSpriteB;
            num += 5;
        }
        text_HotKey.text = num.ToString();
    }

    public void InputFromButton()
    {
        if (buttonState == ButtonState.WaitingForRecruit)
        {
            bool canRecruit = TC.RecruitUnit(unit.Gcost);
            if (canRecruit)
            {
                StartTrainUnit();
            }
        }

        if (buttonState == ButtonState.UnitReady)
        {
            SpawnUnit();
        }
    }

    private void StartTrainUnit()
    {
        buttonState = ButtonState.TrainingUnit;
        Rtimer = (float)unit.Tcost;
        T_Image.SetActive(true);
        T_Bar.gameObject.SetActive(true);
        T_Bar.SetValue_Initial(Rtimer, unit.Tcost);
        text.text = "<color=#FF4F00>" + "Train" + "</color>";
    }

    private void UnitReady()
    {
        buttonState = ButtonState.UnitReady;
        T_Image.SetActive(false);
        T_Bar.gameObject.SetActive(false);
        text.text = "<color=#9BFF00>" + "Ready" + "</color>";
    }

    private void SpawnUnit()
    {
        buttonState = ButtonState.WaitingForRecruit;
        TC.SpanwUnit(unit);
        T_Image.SetActive(false);
        T_Bar.gameObject.SetActive(false);
        text.text = "X" + unit.Num + "<color=#FFD800>|" + unit.Gcost + "G" + "</color>";
    }
}
