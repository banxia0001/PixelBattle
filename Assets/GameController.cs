using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState { none, gameInStartPanel, gameActive, gameFrozen }
    public static GameState state;

    public static bool isOnUI;
   
    public TeamController teamA;
    public TeamController teamB;
    public static TeamConquerManager conquerManager;
    public GameUI UI;


    private float actionTimer;
    [HideInInspector]
    public int turn;
    private int turn_GIncome;
    public float teamScoreRate = 0;

    public int Score,ScoreMax;


    private void Start()
    {
        actionTimer = 0;
        state = GameState.gameInStartPanel;
        conquerManager = FindObjectOfType<TeamConquerManager>();
        Score = ScoreMax / 2;
        //teamScoreRate = 0.5f;
    }

    public void GameStart()
    {
        conquerManager.SetUp();
        state = GameState.gameActive;
        Score = ScoreMax / 2;
        actionTimer = 0;
    }


    private void Update()
    {
        if (state == GameState.gameActive)
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

                TeamCheck();
                StartCoroutine(TurnCheck());
            }
        }  
    }


    public IEnumerator TurnCheck()
    {
        yield return new WaitForSeconds(.05f);
        conquerManager.CheckUnitInLand();
        yield return new WaitForSeconds(.05f);
        conquerManager.CheckLandScore();
        conquerManager.CheckLandConquer_AutoOccup();
        yield return new WaitForSeconds(.05f);
        conquerManager.SetUpFrontLine();

        float newFrontLine = (float)conquerManager.currentScore_ForRatio / (float)conquerManager.maxScoreAll;
        teamScoreRate = newFrontLine;

        if (conquerManager.frontLineMeet)
        {
            //[Check MapConquer Income]

          
                
                //((float)conquerManager.scoreA + (float)conquerManager.scoreB);


            //[TeamRate]
        
            UI.UpdateGoldBar(teamScoreRate);

            int AScore = 0;
            int BScore = 0;

            if (teamScoreRate < 0.1) BScore++;
            if (teamScoreRate < 0.2) BScore++;
            if (teamScoreRate < 0.3) BScore++;

            if (teamScoreRate > 0.7) AScore++;
            if (teamScoreRate > 0.8) AScore++;
            if (teamScoreRate > 0.9) AScore++;

            //[Score]
            Score += AScore - BScore;
            float ratio = (float)Score / (float)ScoreMax;
            UI.scoreBar.SetValue(ratio);

            if (Score < 0 || Score > ScoreMax)
            {
                StartCoroutine(GameWin());
            }
        }

        turn_GIncome++;
        bool canGainG = false;

        if (turn_GIncome >= 3)
        {
            canGainG = true; turn_GIncome = 0;
        }

        if (canGainG)
        {
            int GA = conquerManager.incomeA;
            int GB = conquerManager.incomeB;

            teamA.G += GA; if (teamA.G > 500) teamA.G = 500;
            teamB.G += GB; if (teamB.G > 500) teamB.G = 500;

            UpdateGText();
            UpdatePText();
            UI.UpdateGTText(GA,GB);
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
