using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TeamController teamA;
    public TeamController teamB;

    private float actionTimer;
    private int turn;

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
            
            actionTimer = .2f;
            if (turn > 30)
            {
                TeamCheck(true);
            }
            else TeamCheck(false);
        }
    }

    private void TeamCheck(bool holdStageEnd)
    {
        teamA.TeamCheck_AddUnitToList();
        teamB.TeamCheck_AddUnitToList();
        teamA.TeamCheck_Action(holdStageEnd);
        teamB.TeamCheck_Action(holdStageEnd);
    }

}
