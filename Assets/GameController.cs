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
    public static float frontLine;

    private float actionTimer;
    private int turn;
    private int turn_GIncome;

    public static UnitData.AI_State_Wait waitState;

    public float teamScoreRate = 0;

    public GameObject Frontline;

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


                frontLine = (teamA.teamFrontLine + 54 - teamB.teamFrontLine) / 2;

                //Debug.Log("frontLine" + frontLine);

                Frontline.transform.position = new Vector3(frontLine, 0.1f, 14.94f);

                //int falseScoreForEarlyTurn = 50 - turn;
                //if (falseScoreForEarlyTurn <= 1) falseScoreForEarlyTurn = 1;
                float teamAScore = teamA.teamFrontLine + 10;
                float teamBScore = teamB.teamFrontLine + 10;
                float newFrontLine = (teamAScore + 54 - teamBScore) / 2;

                teamScoreRate = newFrontLine / 54;
                Debug.Log(teamScoreRate);

                UI.UpdateGoldBar(teamScoreRate);

                int ScoreA = 2;
                int ScoreB = 2;
                int ScoreAB = 0;
                int ScoreBB = 0;

   
                if (turn > 40){ ScoreA++; ScoreB++;}
                if (turn > 80){ ScoreA++; ScoreB++;}
                if (turn > 120){ ScoreA++; ScoreB++;}
                if (turn > 240){ ScoreA++; ScoreB++;}
                if (turn > 480){ ScoreA++; ScoreB++;}
                if (turn > 960){ ScoreA++; ScoreB++;}
             

                if (teamScoreRate > 0.6f) { ConquerScore++; ScoreAB += 5;}
                if (teamScoreRate > 0.7f) { ConquerScore++; ScoreAB += 5;}
                if (teamScoreRate > 0.8f) { ConquerScore++; ScoreAB += 5;}
                if (teamScoreRate > 0.9f) { ConquerScore += 2; ScoreAB += 5;}

                if (teamScoreRate < 0.4f) { ConquerScore--; ScoreBB += 5; }
                if (teamScoreRate < 0.3f) { ConquerScore--; ScoreBB += 5; }
                if (teamScoreRate < 0.2f) { ConquerScore--; ScoreBB += 5; }
                if (teamScoreRate < 0.1f) { ConquerScore -= 2; ScoreBB += 5; }
                UI.UpdateScoreBar(ConquerScore, 500);


                if (ConquerScore <= 0 || ConquerScore > 500)
                {
                    StartCoroutine(GameWin());
                }


                //[Check G Income]
                turn_GIncome++;
                bool canGainG = false;
                if (turn_GIncome >= 3)
                {
                    canGainG = true; turn_GIncome = 0;
                }

                int[,] Income = new int[ScoreA + ScoreAB, ScoreB + ScoreBB];


                if (canGainG)
                {
                    teamA.G += Income.GetLength(0); if (teamA.G > 500) teamA.G = 500;
                    teamB.G += Income.GetLength(1); if (teamB.G > 500) teamB.G = 500;
                }

                UpdateGText();
                UpdatePText();
                UI.UpdateGTText(ScoreA,ScoreB, ScoreAB, ScoreBB);
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

        if (turn % 2 == 0)
        {
            teamA.TeamCheck_Action();
            teamB.TeamCheck_Action();
        }
        else
        {
            teamB.TeamCheck_Action();
            teamA.TeamCheck_Action();
        }

        if (teamA.isControl_By_AIPlayer)
        {
            teamA.AI.Action();
        
        }

        if (teamB.isControl_By_AIPlayer)
        {
            teamB.AI.Action();

        }

    }
}
