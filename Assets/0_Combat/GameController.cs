using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState { none, gameActive, gameFrozen }

    [Header("Modify")]
    public int popMax = 75;
    public static int _popMax;

    [Header("Stage")]
    public static GameState state;
    public static bool mouseOnUI;
    private float timer;
    private int turn;
    private int turn_GainTax;

    [Header("Score")]
    public float teamScoreRate = 0;
    public int Score,ScoreMax;

    [Header("Reference")]
    public TeamController[] teams;
    public static LandManager land;
    public GameUI UI;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        _popMax = popMax;
    }

    private void Start()
    {
        state = GameState.gameFrozen;
        land = FindObjectOfType<LandManager>();
        Score = ScoreMax / 2;
        timer = 0;
        GameStart();
    }

    public void GameStart()
    {
        state = GameState.gameActive;
        land.SetUp();
        Score = ScoreMax / 2;
        timer = 0;
    }

    #region PlayerInput
    private void Update()
    {
        if (state == GameState.gameActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) teams[0].buttons[0].InputFromButton();
            if (Input.GetKeyDown(KeyCode.Alpha2)) teams[0].buttons[1].InputFromButton();
            if (Input.GetKeyDown(KeyCode.Alpha3)) teams[0].buttons[2].InputFromButton();
            if (Input.GetKeyDown(KeyCode.Alpha4)) teams[0].buttons[3].InputFromButton();
            if (Input.GetKeyDown(KeyCode.Alpha5)) teams[0].buttons[4].InputFromButton();
        }
    }
    #endregion

    void FixedUpdate()
    {
        if (state == GameState.gameActive)
        {
            timer -= Time.fixedDeltaTime;
            if (timer < 0)
            {
                turn++;
                timer = 1f;
                StartCoroutine(NewTurn());
            } 
        }
    }

    public IEnumerator NewTurn()
    {
        //[Update AI]
        if (turn % 2 == 0)
        {
            teams[0].UpdateAction();
            yield return new WaitForSeconds(.3f);
            teams[1].UpdateAction();

            if (teams[0].isAIControl) teams[0].AI.Action();
            if (teams[1].isAIControl) teams[1].AI.Action();
        }
        else
        {
          teams[1].UpdateAction();
            yield return new WaitForSeconds(.3f);
          teams[0].UpdateAction();

            if (teams[1].isAIControl) teams[1].AI.Action();
            if (teams[0].isAIControl) teams[0].AI.Action();
        }


        //[Land]
        yield return new WaitForSeconds(.2f);
        land.UpdateLand();
        land.UpdateScore();

        yield return new WaitForSeconds(.2f);
        land.AutoOccup();
        land.SetUpFrontLine();
        float newFrontLine = land.GetLandConquerRatio();
        teamScoreRate = newFrontLine;

        //[Score]
        yield return new WaitForSeconds(.2f);
        if (land.frontLineMeet)
        {        
            UI.UpdateGBar(teamScoreRate);
            
            //[Score Calculation]
            int AScore = 0; int BScore = 0;
            if (teamScoreRate < 0.1) BScore++; if (teamScoreRate < 0.2) BScore++; if (teamScoreRate < 0.3) BScore++;
            if (teamScoreRate > 0.7) AScore++; if (teamScoreRate > 0.8) AScore++; if (teamScoreRate > 0.9) AScore++;

            //[Score Set Value]
            Score += AScore - BScore;
            float ratio = (float)Score / (float)ScoreMax;
            UI.scoreBar.SetValue(ratio);
            if (Score < 0 || Score > ScoreMax) StartCoroutine(GameWin());
        }

        //[Gain Tax Check]
        yield return new WaitForSeconds(.2f);
        turn_GainTax++;
        bool canGainG = false;
        if (turn_GainTax >= 3) { canGainG = true; turn_GainTax = 0; }

        if (canGainG)
        {
            for (int i = 0; i < 2; i++)
            {
                int G = land.income[i];
                teams[i].G += G; if (teams[i].G > 500) teams[i].G = 500;

                UI.Update_GPer(land.income[i],i);
                UI.Update_G(teams[i].G, i);
            }
            UpdatePText();
        }
    }
    public IEnumerator GameWin()
    {
        state = GameState.gameFrozen;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    public void UpdatePText()
    {
        UI.Update_Population(teams[0].P, 0);
        UI.Update_Population(teams[1].P, 1);
    }
}
