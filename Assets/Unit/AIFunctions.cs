using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFunctions : MonoBehaviour
{
    public static Unit AI_Find_ShootJavelin(float dis, Transform trans, UnitData.UnitTeam unitTeam)
    {
        List<Unit> closeUnit = BattleFunction.Find_UnitsInRange(unitTeam, dis, trans);
        if (closeUnit == null) return null;
        if (closeUnit.Count == 0) return null;

        Unit target = BattleFunction.Find_ClosestUnitInList(closeUnit, trans);
        return target;
    }

    public static Unit AI_Find_ClosestUnit(UnitData.UnitTeam targetTeam, Unit startUnit)
    {
        List<Unit> unitGroup = BattleFunction.Find_TargetUnitGroup(targetTeam);

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            Vector3 pos1 = new Vector3(startUnit.transform.position.x, 0, startUnit.transform.position.z * 2);
            Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
            float thisDis = Vector3.Distance(pos1,pos2);

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
        if (targetType == UnitData.UnitType.infantry) unitGroup = targetUnitTeam.warriorList;
        if (targetType == UnitData.UnitType.archer) unitGroup = targetUnitTeam.archerList;
        if (targetType == UnitData.UnitType.cavalry) unitGroup = targetUnitTeam.cavalryList;
        if (targetType == UnitData.UnitType.monster) unitGroup = targetUnitTeam.monsterList;
        if (targetType == UnitData.UnitType.artillery) unitGroup = targetUnitTeam.artilleryList;

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {

            Vector3 pos1 = new Vector3(startUnit.transform.position.x, 0, startUnit.transform.position.z * 2);
            Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
            float thisDis = Vector3.Distance(pos1, pos2);

            if (closetDis > thisDis)
            {

                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }

}
