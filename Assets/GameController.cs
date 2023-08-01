using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TeamController teamA;
    public TeamController teamB;

    private float actionTimer;
    private int turn;

    public static bool holdStage;

    private void Start()
    {
        actionTimer = 0;
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
            if (turn > 30)
            {
                holdStage = false;
                TeamCheck();
            }
            else
            {
                holdStage = true;
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
