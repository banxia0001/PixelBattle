using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFunctions : MonoBehaviour
{
    public static Unit AI_Find_ClosestUnit(UnitData.UnitTeam targetTeam, Unit startUnit)
    {
        List<Unit> unitGroup = BattleFunction.Find_TargetUnitGroup(targetTeam);

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisDis = Vector3.Distance(startUnit.transform.position, unitGroup[i].transform.position);

            if (closetDis > thisDis)
            {

                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }
    public static Unit AI_Find_ClosestUnit(UnitData.UnitTeam targetTeam, UnitData.UnitType targetType, Unit startUnit)
    {
        GameController GC = FindObjectOfType<GameController>(false);
        TeamController targetUnitTeam = null;
        if (targetTeam == UnitData.UnitTeam.teamA) targetUnitTeam = GC.teamA;
        if (targetTeam == UnitData.UnitTeam.teamB) targetUnitTeam = GC.teamB;

        List<Unit> unitGroup = new List<Unit>();
        if (targetType == UnitData.UnitType.warrior) unitGroup = targetUnitTeam.warriorList;
        if (targetType == UnitData.UnitType.archer) unitGroup = targetUnitTeam.archerList;
        if (targetType == UnitData.UnitType.cavalry) unitGroup = targetUnitTeam.cavalryList;
        if (targetType == UnitData.UnitType.monster) unitGroup = targetUnitTeam.monsterList;

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisDis = Vector3.Distance(startUnit.transform.position, unitGroup[i].transform.position);

            if (closetDis > thisDis)
            {

                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }

}
