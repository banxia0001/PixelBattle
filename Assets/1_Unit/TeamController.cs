using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public bool isAIControl;
    private GameController GC;

    [Header("TeamStats")]
    public int G;
    public int P;
    public UnitRecruitList Rlist;
    public Unit.UnitTeam unitTeam;

    [Header("TeamStartPos")]
    private float mapHeight;
    private float startX;


    [Header("TeamButton")]
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
    [HideInInspector]
    public AIPlayer AI;



    public void Awake()
    {
        GC = FindObjectOfType<GameController>();
        AI = gameObject.GetComponent<AIPlayer>();
        UploadDataToButtons();
        TeamCheck_AddUnitToList();
    }
    private void Start()
    {
        mapHeight = LandManager._landHeight;
        startX = 1;
        if (unitTeam == Unit.UnitTeam.teamB) startX = LandManager._landLength - 1;
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
        float y = Random.Range(5, mapHeight - 5);
        if (data.Num == 2) y = Random.Range(5, mapHeight - 7);
        if (data.Num == 3) y = Random.Range(5, mapHeight - 8);
        if (data.Num == 4) y = Random.Range(5, mapHeight - 9);
        if (data.Num == 5) y = Random.Range(5, mapHeight - 10);

        for (int i = 0; i < num; i++)
        {
           GameObject ob =  Instantiate(spawnOb, new Vector3(startX, 0.1f, y + i), Quaternion.identity);

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
        int P = 0;
        if (warriorList != null && warriorList.Count != 0)
            foreach (Unit unit in warriorList)
            {
                unit.AI_DecideAction();
                P++;
            }

        if (archerList != null && archerList.Count != 0)
            foreach (Unit unit in archerList)
            {
                unit.AI_DecideAction();
                P++;
            }

        if (cavalryList != null && cavalryList.Count != 0)
            foreach (Unit unit in cavalryList)
            {
                unit.AI_DecideAction();
                P++;
            }

        if (monsterList != null && monsterList.Count != 0)
            foreach (Unit unit in monsterList)
            {
                unit.AI_DecideAction();
                P++;
            }

        if (artilleryList != null && artilleryList.Count != 0)
            foreach (Unit unit in artilleryList)
            {
                unit.AI_DecideAction();
                P++;
            }

        this.P = P;
    }



    public float CalcualteUnitFront(Unit unit)
    {
        float xAxis = unit.transform.position.x - startX;
        if (unitTeam == Unit.UnitTeam.teamB) xAxis = startX - unit.transform.position.x;
        return xAxis * unit.data_local.UnitValue;
    }

}
