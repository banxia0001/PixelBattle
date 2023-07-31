using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TeamController teamA;
    public TeamController teamB;

    private float actionTimer;

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
