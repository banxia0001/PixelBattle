using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TeamController teamA;
    public TeamController teamB;

    private float actionTimer;
    private int turn;

    public static UnitData.AI_State_Wait waitState;

    private void Start()
    {
        actionTimer = 0;
        waitState = UnitData.AI_State_Wait.advance;
    }
    void FixedUpdate()
    {
        if (actionTimer > 0)
        {
            actionTimer -= Time.fixedDeltaTime;
        }

        else
        {
            turn++;
            actionTimer = 0.33f;
            if (turn > 16) waitState = UnitData.AI_State_Wait.hold5s;
            if (turn > 32) waitState = UnitData.AI_State_Wait.hold10S;
            if (turn > 48) waitState = UnitData.AI_State_Wait.hold15S;

            TeamCheck();
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