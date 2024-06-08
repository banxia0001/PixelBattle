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

    //[HideInInspector]
    public List<Unit> unitList;
    [HideInInspector]
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


    public void InsertUnit(Unit unit)
    {
        unitList.Add(unit);
        if(unit.data.unitType == UnitData.UnitType.infantry) warriorList.Add(unit);
        if(unit.data.unitType == UnitData.UnitType.archer) archerList.Add(unit);
        if(unit.data.unitType == UnitData.UnitType.cavalry) cavalryList.Add(unit);
        if(unit.data.unitType == UnitData.UnitType.artillery) artilleryList.Add(unit);
        if(unit.data.unitType == UnitData.UnitType.monster) monsterList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        RemoveUnit_2(unitList, unit);
        if (unit.data.unitType == UnitData.UnitType.archer) RemoveUnit_2(archerList, unit);
        if (unit.data.unitType == UnitData.UnitType.cavalry) RemoveUnit_2(cavalryList, unit);
        if (unit.data.unitType == UnitData.UnitType.artillery) RemoveUnit_2(artilleryList, unit);
        if (unit.data.unitType == UnitData.UnitType.monster) RemoveUnit_2(monsterList, unit);
    }

    private void RemoveUnit_2(List<Unit> list , Unit unit)
    {
        if (list.Contains(unit))
        {
            list.Remove(unit);
        }
        else Debug.LogWarning("Missing From List.");
    }
    public bool RecruitUnit(int G)
    {
        if (G > this.G) return false;
        this.G = this.G - G;
        return true;
    }

    public bool Check_UnitPop(int P)
    {
        if (this.P + P > GameController._popMax) return false;
        else return true;
    }

    public void SpanwUnit(UnitData_Local data)
    {
        //Increase Pop
        int num = data.Num;
        this.P = this.P + num;
        GC.UpdatePText();

        Vector3 spawnPos = new Vector3(0, 0, 0);
        GameObject spawnOb = data.prefabs[(int)unitTeam];

        float y = Random.Range(5, mapHeight - 5);
        if (data.Num == 2) y = Random.Range(5, mapHeight - 7);
        if (data.Num == 3) y = Random.Range(5, mapHeight - 8);
        if (data.Num == 4) y = Random.Range(5, mapHeight - 9);
        if (data.Num == 5) y = Random.Range(5, mapHeight - 10);

        for (int i = 0; i < num; i++)
        {
            GameObject ob = Instantiate(spawnOb, new Vector3(startX, 0.1f, y + i), Quaternion.identity);
            if (unitTeam == Unit.UnitTeam.teamB) ob.transform.eulerAngles = new Vector3(0, -90, 0);
            else ob.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void UpdateAction()
    {
        this.P = unitList.Count;
        UpdateAction_2(warriorList);
        UpdateAction_2(archerList);
        UpdateAction_2(cavalryList);
        UpdateAction_2(monsterList);
        UpdateAction_2(artilleryList);
    }
    public void FindTarget()
    {
        FindTarget_2(warriorList);
        FindTarget_2(archerList);
        FindTarget_2(cavalryList);
        FindTarget_2(monsterList);
        FindTarget_2(artilleryList);
    }
    private void UpdateAction_2(List<Unit> units)
    {
        if (units != null && units.Count != 0)
            for(int i = 0; i < units.Count; i++)
            {
                units[i].DecideAction();
            }
    }
    private void FindTarget_2(List<Unit> units)
    {
        if (units != null && units.Count != 0)
            for (int i = 0; i < units.Count; i++)
            {
                units[i].FindTarget();
            }
    }

    public float CalcualteUnitFront(Unit unit)
    {
        float xAxis = unit.transform.position.x - startX;
        if (unitTeam == Unit.UnitTeam.teamB) xAxis = startX - unit.transform.position.x;
        return xAxis * unit.data_local.UnitValue;
    }
}
