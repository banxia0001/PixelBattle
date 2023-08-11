using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static bool isOnUI;
    public bool gameStart;
    public TeamController teamA;
    public TeamController teamB;

    private float actionTimer;
    private int turn;

    public static UnitData.AI_State_Wait waitState;

    public TMP_Text GTextA;
    public TMP_Text GTextB;

    private void Start()
    {
        actionTimer = 0;
        waitState = UnitData.AI_State_Wait.advance;
    }

    public void UpdateG()
    {
        GTextA.text = teamA.G.ToString();
        GTextB.text = teamB.G.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) teamA.buttons[0].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha2)) teamA.buttons[1].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha3)) teamA.buttons[2].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha4)) teamA.buttons[3].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha5)) teamA.buttons[4].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha6)) teamB.buttons[0].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha7)) teamB.buttons[1].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha8)) teamB.buttons[2].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha9)) teamB.buttons[3].InputFromButton();
        if (Input.GetKeyDown(KeyCode.Alpha0)) teamB.buttons[4].InputFromButton();
    }
    void FixedUpdate()
    {
        if (gameStart)
        {
            if (actionTimer > 0)
            {
                actionTimer -= Time.fixedDeltaTime;
            }

            else
            {
                turn++;

                teamA.G++;
                if (teamA.G > 250) teamA.G = 250;
                teamB.G++;
                if (teamB.G > 250) teamB.G = 250;

                UpdateG();
                actionTimer = 0.33f;
                if (turn > 16) waitState = UnitData.AI_State_Wait.hold5s;
                if (turn > 32) waitState = UnitData.AI_State_Wait.hold10S;
                if (turn > 48) waitState = UnitData.AI_State_Wait.hold15S;

                TeamCheck();
            }
        }  
    }

    private void TeamCheck()
    {
        teamA.TeamCheck_AddUnitToList();
        teamB.TeamCheck_AddUnitToList();
        teamA.TeamCheck_Action();
        teamB.TeamCheck_Action();
    }

}
