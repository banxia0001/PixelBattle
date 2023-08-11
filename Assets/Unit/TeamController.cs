using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    private GameController GC;
    public int G;
    public UnitRecruitList Rlist;
    public Unit.UnitTeam unitTeam;

    public int mapLength;
    public int startXAxis;

    public List<RecruitButton> buttons;


    public List<Unit> warriorList;
    [HideInInspector]
    public List<Unit> archerList;
    [HideInInspector]
    public List<Unit> cavalryList;
    [HideInInspector]
    public List<Unit> monsterList;
    [HideInInspector]
    public List<Unit> artilleryList;



    public void Awake()
    {
        GC = FindObjectOfType<GameController>();
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
        if (G > this.G)
            return false;

        else
        {
            this.G = this.G - G;
            GC.UpdateG();
            return true;
        }
    }

    public void SpanwUnit(UnitData_Local data)
    {
        int num = data.Num;
      
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
        if (warriorList != null)
            if (warriorList.Count != 0)
            {
                foreach (Unit unit in warriorList)
                {
                    unit.AI_DecideAction();
                }
            }

        if (archerList != null)
            if (archerList.Count != 0)
            {
                foreach (Unit unit in archerList)
                {
                    unit.AI_DecideAction();
                }
            }

        if (cavalryList != null)
            if (cavalryList.Count != 0)
            {
                foreach (Unit unit in cavalryList)
                {
                    unit.AI_DecideAction();
                }
            }

        if (monsterList != null)
            if (monsterList.Count != 0)
            {
                foreach (Unit unit in monsterList)
                {
                    unit.AI_DecideAction();
                }
            }

        if (artilleryList != null)
            if (artilleryList.Count != 0)
            {
                foreach (Unit unit in artilleryList)
                {
                    unit.AI_DecideAction();
                }
            }
    }

}
