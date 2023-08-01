using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public UnitData.UnitTeam unitTeam;
    public List<Unit> warriorList;
    public List<Unit> archerList;
    public List<Unit> cavalryList;
    public List<Unit> monsterList;


    public void Awake()
    {
        TeamCheck_AddUnitToList();
    }

    public void TeamCheck_AddUnitToList()
    {
        warriorList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.infantry);
        archerList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.archer);
        cavalryList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.cavalry);
        monsterList = BattleFunction.Find_TargetUnitGroup(unitTeam, UnitData.UnitType.monster);
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
    }

}
