using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public bool isControl_By_AIPlayer;
    private GameController GC;

    [Header("TeamStats")]
    public int G;
    public int P;
    public UnitRecruitList Rlist;
    public Unit.UnitTeam unitTeam;

    [Header("TeamStartPos")]
    public int mapLength;
    public int startXAxis;

    [Header("TeamButton")]
    public List<RecruitButton> buttons;

    public float teamFrontLine;

    public List<Unit> warriorList;
    [HideInInspector]
    public List<Unit> archerList;
    [HideInInspector]
    public List<Unit> cavalryList;
    [HideInInspector]
    public List<Unit> monsterList;
    [HideInInspector]
    public List<Unit> artilleryList;
    [HideInInspector]
    public AIPlayer AI;



    public void Awake()
    {
        GC = FindObjectOfType<GameController>();

        AI = gameObject.GetComponent<AIPlayer>();
        //if (AI != null && AI.enabled == true) isControl_By_AIPlayer = true;
        //else isControl_By_AIPlayer = false;

        UploadDataToButtons();
        TeamCheck_AddUnitToList();
    }

    private void UploadDataToButtons()
    {
        for (int i = 0; i < Rlist.UnitPrefabs.Count; i++)
        {
            buttons[i].InputData(this, Rlist.UnitPrefabs[i],i);
        }
    }

    public bool RecruitUnit(int G)
    {
        if (G > this.G) return false;

        this.G = this.G - G;
        return true;
    }

    public bool Check_UnitPop(int P)
    {
        if (this.P + P > 50) return false;
        else return true;
    }

    public void SpanwUnit(UnitData_Local data)
    {
        int num = data.Num;

        this.P = this.P + num;
        GC.UpdatePText();

        Vector3 spawnPos = new Vector3(0, 0, 0);
        GameObject spawnOb = data.UnitPrefabA;

        if(unitTeam == Unit.UnitTeam.teamB) spawnOb = data.UnitPrefabB;
        int y = Random.Range(5, mapLength - 5);
        if (data.Num == 2) y = Random.Range(5, mapLength - 7);
        if (data.Num == 3) y = Random.Range(5, mapLength - 8);
        if (data.Num == 4) y = Random.Range(5, mapLength - 9);
        if (data.Num == 5) y = Random.Range(5, mapLength - 10);

        for (int i = 0; i < num; i++)
        {
           GameObject ob =  Instantiate(spawnOb, new Vector3(startXAxis, 0.1f, y + i), Quaternion.identity);

            if (unitTeam == Unit.UnitTeam.teamB) 
            ob.transform.eulerAngles = new Vector3(0, -90, 0);

            else ob.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void TeamCheck_AddUnitToList()
    {
        warriorList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.infantry);
        archerList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.archer);
        cavalryList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.cavalry);
        monsterList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.monster);
        artilleryList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.artillery);
    }

    public void TeamCheck_Action()
    {
        //float teamScore = 0;
        float teamFrontLine = 0;
        int P = 0;
        if (warriorList != null)
            if (warriorList.Count != 0)
            {
                foreach (Unit unit in warriorList)
                {
                    unit.AI_DecideAction();
                    float x = CalcualteUnitFront(unit);
                    //teamScore += CalcualteUnitValue(unit, x);
                    teamFrontLine += x;
                    P++;
                }
            }

        if (archerList != null)
            if (archerList.Count != 0)
            {
                foreach (Unit unit in archerList)
                {
                    unit.AI_DecideAction();
                    float x = CalcualteUnitFront(unit);
                    //teamScore += CalcualteUnitValue(unit, x);
                    teamFrontLine += x;
                    P++;
                }
            }

        if (cavalryList != null)
            if (cavalryList.Count != 0)
            {
                foreach (Unit unit in cavalryList)
                {
                    unit.AI_DecideAction();
                    float x = CalcualteUnitFront(unit);
                    //teamScore += CalcualteUnitValue(unit, x);
                    teamFrontLine += x;
                    P++;
                }
            }

        if (monsterList != null)
            if (monsterList.Count != 0)
            {
                foreach (Unit unit in monsterList)
                {
                    unit.AI_DecideAction();
                    float x = CalcualteUnitFront(unit);
                    //teamScore += CalcualteUnitValue(unit, x);
                    teamFrontLine += x;
                    P++;
                }
            }

        if (artilleryList != null)
            if (artilleryList.Count != 0)
            {
                foreach (Unit unit in artilleryList)
                {
                    unit.AI_DecideAction();
                    float x = CalcualteUnitFront(unit);
                    //teamScore += CalcualteUnitValue(unit, x);
                    teamFrontLine += x;
                    P++;
                }
            }

        //this.teamScore = (int)teamScore;
        this.teamFrontLine = teamFrontLine/((float)P + 0.1f);
        this.P = P;
    }


    //public float CalcualteUnitValue(Unit unit, float xAxis)
    //{
    //    UnitData_Local data = unit.data_local;
    //    float cost = data.Gcost / data.Num;
    //    return xAxis * data.UnitValue;
    //}

    public float CalcualteUnitFront(Unit unit)
    {
        float xAxis = unit.transform.position.x - startXAxis;
        if (unitTeam == Unit.UnitTeam.teamB) xAxis = startXAxis - unit.transform.position.x;

        return xAxis * unit.data_local.UnitValue;
    }

}
