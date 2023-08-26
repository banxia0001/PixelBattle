using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamConquerManager : MonoBehaviour
{
    public int length;
    public int maxScoreInLand = 5000;
    public int maxScoreAll = 5000;
    public int currentScore_ForRatio = 5000;


    public bool frontLineMeet;
    public int[] lands;
    public int[] unitInlands_A;
    public int[] unitInlands_B;

    public int scoreA,scoreB;
    public int incomeA,incomeB;
    public GameObject frontLineGuide_A;
    public GameObject frontLineGuide_B;


    //public UnitListInLand[] allUnitsInLands_A;
    //public UnitListInLand[] allUnitsInLands_B;
    //public List<Unit> unitsInFrontline_A;
    //public List<Unit> unitsInFrontline_B;


    [HideInInspector]
    public int ConquerScore;
    public int ConquerScoreMax;
    private GameController GC;


    private void Start()
    {
        frontLineMeet = false;
        GC = FindObjectOfType<GameController>();
        maxScoreAll = maxScoreInLand * lands.Length * 2;
        //allUnitsInLands_A = new UnitListInLand[lands.Length];
        //allUnitsInLands_B = new UnitListInLand[lands.Length];
    }



    public void SetUp()
    {
        ConquerScore = ConquerScoreMax / 2;

        for (int i = 0; i < lands.Length; i++)
        {
            lands[i] = 0;
        }
    }

    public void SetUpFrontLine()
    {
        float Length = (float)this.length;
        Length = Length / maxScoreInLand;


        if (!frontLineMeet)
        {
            frontLineGuide_A.transform.position = new Vector3((float)scoreA * Length, 0.1f, 15);
            frontLineGuide_B.transform.position = new Vector3((54 - ((float)scoreB * Length)), 0.1f, 15);
        }

        else
        {
            //int scoreB2 = maxScoreAll - scoreA; 
            float aDis = (float)GC.teamScoreRate * 54;
            frontLineGuide_A.transform.position = new Vector3(aDis, 0.1f, 15);
            frontLineGuide_B.transform.position = new Vector3(aDis, 0.1f, 15);
        }
    }
    public void CheckUnitInLand()
    {
        unitInlands_A = new int[lands.Length];
        unitInlands_B = new int[lands.Length];

        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject ob in units)
        {
            for (int i = 1; i < lands.Length + 1; i++)
            {
                if (ob.transform.position.x < i * length)
                {
                    AddUnitToLand(ob, i-1);
                    break;
                }
            }
        }

        for (int i = 0; i < lands.Length; i++)
        {
            if (!frontLineMeet)
            {
                bool AAppear = false;
                bool BAppear = false;

                if (unitInlands_A[i] > 0) AAppear = true;
                if (unitInlands_B[i] > 0) BAppear = true;
                if (AAppear && BAppear)
                {
                    frontLineMeet = true;
                }
                else
                {
                    if (AAppear) lands[i] += 20;
                    if (BAppear) lands[i] += -20;
                }
            }
            else
            {
                lands[i] += unitInlands_A[i] - unitInlands_B[i];
            }

            if (lands[i] > maxScoreInLand) lands[i] = maxScoreInLand;
            if (lands[i] < -maxScoreInLand) lands[i] = -maxScoreInLand;
        }
    }
    public void CheckLandScore()
    {
        int AScore = 0;
        int BScore = 0;

        int AIncome = 0;
        int BIncome = 0;
        currentScore_ForRatio = 0;

        for (int i = 0; i < lands.Length; i++)
        {
            if (lands[i] > 0) AScore += lands[i];
            if (lands[i] < 0) BScore += lands[i];

            if (lands[i] > maxScoreInLand / 2) AIncome++;
            if (lands[i] < -maxScoreInLand / 2) BIncome++;

            currentScore_ForRatio += maxScoreInLand + lands[i];
        }

        Debug.Log(AScore + "," + BScore);
        scoreA = AScore;
        scoreB = Mathf.Abs(BScore);
        incomeA = AIncome;
        incomeB = BIncome;
    }

    public void CheckLandConquer_AutoOccup()
    {
        for (int i = 0; i < lands.Length; i++)
        {
            if (i != 0 && i != lands.Length - 1)
            {
                if (lands[i - 1] > maxScoreInLand / 2 && lands[i + 1] > maxScoreInLand / 2)
                {
                    lands[i] += 20;
                }
                if (lands[i - 1] < -maxScoreInLand / 2 && lands[i + 1] < -maxScoreInLand / 2)
                {
                    lands[i] -= 20;
                }
            }      
        }
    }
    private void AddUnitToLand(GameObject unitOb, int i)
    {
        Unit unit = unitOb.GetComponent<Unit>();
        if (unit.unitTeam == Unit.UnitTeam.teamA) unitInlands_A[i] += unit.data_local.UnitValue;
        else unitInlands_B[i] += unit.data_local.UnitValue;
    }

    public Vector3 ReturnUnitFrontLinePosition_GuideLine(Unit unit)
    {
        if (unit.unitTeam == Unit.UnitTeam.teamA) return frontLineGuide_A.transform.position;
       else return frontLineGuide_B.transform.position;
    }
}
