using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState { none, gameActive, gameFrozen }
    public GameState state;

    public static bool isOnUI;
   
    public TeamController teamA;
    public TeamController teamB;
    public GameUI UI;

    public int ConquerScore;

    private float actionTimer;
    private int turn;
    private int turn_GIncome;

    public static UnitData.AI_State_Wait waitState;

    public float teamScoreRate = 0;

    private void Start()
    {
        actionTimer = 0;
        waitState = UnitData.AI_State_Wait.advance;

        ConquerScore = 250;
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
        if (state == GameState.gameActive)
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

                //[Check MapConquer Income]
                int falseScoreForEarlyTurn = 50 - turn;
                if (falseScoreForEarlyTurn <= 1) falseScoreForEarlyTurn = 1;

                float teamAScore = ((teamA.teamScore / falseScoreForEarlyTurn) + 100) * 100;
                float teamBScore = ((teamB.teamScore / falseScoreForEarlyTurn) + 100) * 100;

                teamScoreRate = teamAScore / (teamBScore + teamAScore);
                UI.UpdateGoldBar(teamScoreRate);

                if (teamScoreRate > 0.54f) ConquerScore++;
                if (teamScoreRate > 0.62f) ConquerScore++;
                if (teamScoreRate > 0.66f) ConquerScore++;
                if (teamScoreRate > 0.7f) ConquerScore += 2;

                if (teamScoreRate < 0.46f) ConquerScore--;
                if (teamScoreRate < 0.38f) ConquerScore--;
                if (teamScoreRate < 0.34f) ConquerScore--;
                if (teamScoreRate < 0.3f) ConquerScore -=2;
                UI.UpdateScoreBar(ConquerScore, 500);


                //[Check G Income]
                turn_GIncome++;
                bool canGainG = false;
                if (turn_GIncome >= 3)
                {
                    canGainG = true; turn_GIncome = 0;
                }

                int[,] Income = BattleFunction.CheckGTRate(teamScoreRate);


                if (canGainG)
                {
                    teamA.G += Income.GetLength(1); if (teamA.G > 500) teamA.G = 500;
                    teamB.G += Income.GetLength(0); if (teamB.G > 500) teamB.G = 500;
                }

                UpdateGText();
                UpdatePText();
                UI.UpdateGTText(Income.GetLength(1), Income.GetLength(0));
            }
        }  
    }


    public IEnumerator GameWin()
    {
        state = GameState.gameFrozen;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    
    public void UpdateGText()
    {
        UI.UpdateGText(teamA.G, teamB.G);
    }

    public void UpdatePText()
    {
        UI.UpdatePText(teamA.P, teamB.P);
    }
    private void TeamCheck()
    {
        teamA.TeamCheck_AddUnitToList();
        teamB.TeamCheck_AddUnitToList();
        teamA.TeamCheck_Action();
        teamB.TeamCheck_Action();
    }
}
