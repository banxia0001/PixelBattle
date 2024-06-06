using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    [Header("Map Data")]
    public static float _landLength, _landHeight;

    public float landLength;
    public float landHeight;
    public int landNum;

    public int scorePerLand;
    private int scoreTotal;
    private int scoreNow;


    [Header("LandStats")]
    public int[] lands;
    [HideInInspector]public bool frontLineMeet;
    [HideInInspector] public int[] units_A;
    [HideInInspector] public int[] units_B;



    [Header("Score")]
    public int[] score;
    public int[] income;
    public GameObject[] frontLine;
    private GameController GC;


    private void Awake()
    {
        _landHeight = landHeight;
        _landLength = landLength;
        frontLineMeet = false;
        GC = FindObjectOfType<GameController>();
        scoreTotal = scorePerLand * landNum * 2;
    }

    public void SetUp()
    {
        lands = new int[landNum];
        for (int i = 0; i < landNum; i++)
        {
            lands[i] = 0;
        }
    }
    public float GetLandConquerRatio()
    {
        return (float)scoreNow / (float)scoreTotal;
    }
    public void SetUpFrontLine()
    {
        float length = landLength / scoreTotal;

        if (!frontLineMeet)
        {
            frontLine[0].transform.position = new Vector3((float)score[0] * length, 0.1f, landHeight/2);
            frontLine[1].transform.position = new Vector3((landLength - ((float)score[1] * length)), 0.1f, landHeight/2);
        }

        else
        {
            float aDis = (float)GC.teamScoreRate * landLength;
            frontLine[0].transform.position = new Vector3(aDis, 0.1f, landHeight/2);
            frontLine[1].transform.position = new Vector3(aDis, 0.1f, landHeight/2);
        }
    }
    public void UpdateLand()
    {
        units_A = new int[lands.Length];
        units_B = new int[lands.Length];
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        //[Add Unit to the Land List]
        foreach (GameObject ob in units)
        {
            for (int i = 1; i < lands.Length + 1; i++)
            {
                if (ob.transform.position.x < i * landLength/ landNum)
                {
                    AddUnitToLand(ob, i - 1);
                    break;
                }
            }
        }

        //[Calculate Unit Occup Score]
        for (int i = 0; i < lands.Length; i++)
        {
            if (!frontLineMeet)
            {
                bool AAppear = false;
                bool BAppear = false;
                if (units_A[i] > 0) AAppear = true;
                if (units_B[i] > 0) BAppear = true;

                if (AAppear && BAppear) frontLineMeet = true;
                else if (AAppear) lands[i] += 2000;
                else if (BAppear) lands[i] += -2000;
            }
            else lands[i] += units_A[i] - units_B[i];

            if (lands[i] > scorePerLand) lands[i] = scorePerLand;
            if (lands[i] < -scorePerLand) lands[i] = -scorePerLand;
        }
    }
    public void UpdateScore()
    {
        int AScore = 0;
        int BScore = 0;
        int AIncome = 0;
        int BIncome = 0;
        scoreNow = 0;

        for (int i = 0; i < landNum; i++)
        {
            if (lands[i] > 0) AScore += lands[i];
            if (lands[i] < 0) BScore += lands[i];

            if (lands[i] > scorePerLand / 2) AIncome++;
            if (lands[i] < -scorePerLand / 2) BIncome++;

            scoreNow += scorePerLand + lands[i];
        }

        score[0] = AScore;
        score[1] = Mathf.Abs(BScore);
        income[0] = AIncome;
        income[1] = BIncome;
    }

    public void AutoOccup()
    {
        for (int i = 0; i < lands.Length; i++)
        {
            if (i != 0 && i != lands.Length - 1)
            {
                if (lands[i - 1] > scorePerLand / 2 && lands[i + 1] > scorePerLand / 2)
                {
                    lands[i] += 30;
                }
                if (lands[i - 1] < -scorePerLand / 2 && lands[i + 1] < -scorePerLand / 2)
                {
                    lands[i] -= 30;
                }
            }      
        }
    }
    private void AddUnitToLand(GameObject unitOb, int i)
    {
        Unit unit = unitOb.GetComponent<Unit>();
        if (unit.unitTeam == Unit.UnitTeam.teamA) units_A[i] += unit.data_local.UnitValue;
        else units_B[i] += unit.data_local.UnitValue;
    }

    public Vector3 Get_GuideLine(Unit unit)
    {
       return frontLine[(int)unit.unitTeam].transform.position;
    }
}
