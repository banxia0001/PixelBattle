using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFunctions : MonoBehaviour
{
    public static Unit AI_Find_ShootJavelin(float dis, Transform trans, Unit.UnitTeam unitTeam)
    {
        List<Unit> closeUnit = BattleFunction.Find_UnitsInRange(unitTeam, dis, trans);
        if (closeUnit == null) return null;
        if (closeUnit.Count == 0) return null;

        Unit target = BattleFunction.Find_ClosestUnitInList(closeUnit, trans);
        return target;
    }

    public static Unit AI_Find_ClosestUnit(Unit.UnitTeam targetTeam, Unit startUnit, bool targetFrontline)
    {
        List<Unit> unitGroup = BattleFunction.Find_TargetUnitGroup(targetTeam);

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisDis = 0;

            if (targetFrontline)
            {
                Vector3 pos1 = new Vector3(GameController.frontLine, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2);
            }

            else
            {
                Vector3 pos1 = new Vector3(startUnit.transform.position.x, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2);
            }



            if (closetDis > thisDis)
            {
                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }


    public static Unit AI_Find_ClosestUnit(Unit.UnitTeam targetTeam, UnitData.UnitType targetType, Unit startUnit, bool targetFrontline)
    {
        GameController GC = FindObjectOfType<GameController>(false);
        TeamController targetUnitTeam = null;
        if (targetTeam == Unit.UnitTeam.teamA) targetUnitTeam = GC.teamA;
        if (targetTeam == Unit.UnitTeam.teamB) targetUnitTeam = GC.teamB;

        List<Unit> unitGroup = new List<Unit>();
        if (targetType == UnitData.UnitType.infantry)
        { 
        
            foreach(Unit unit in targetUnitTeam.warriorList)
            {
                unitGroup.Add(unit);    
            }
            foreach (Unit unit in targetUnitTeam.monsterList)
            {
                unitGroup.Add(unit);
            }

        } 
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
            float thisDis = 0;

            if (targetFrontline)
            {
                Vector3 pos1 = new Vector3(GameController.frontLine, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2);
            }

            else
            {
                Vector3 pos1 = new Vector3(startUnit.transform.position.x, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2);
            }

            if (closetDis > thisDis)
            {

                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }

    public static List<Unit> AI_Melee_FindAllUnit_InHitBox(Unit attacker,float hitboxLength, float hitboxWidth, Vector3 postion)
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapBox(postion, new Vector3(hitboxWidth, 1f, hitboxLength), Quaternion.identity, LayerMask.GetMask("Unit"));
        if (overlappingItems == null) return null;
        else return AIFunctions.AI_FindEnemyInList(attacker, overlappingItems);
    }

    public static List<Unit> AI_FindEnemyInList(Unit attacker, Collider[] overlappingItems)
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (attacker.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;
        List<Unit> finalList = new List<Unit>();

        foreach (Collider coll in overlappingItems)
        {
            Unit unit = coll.gameObject.GetComponent<Unit>();

            if (unit.unitTeam == targetTeam)
            {
                finalList.Add(unit);
            }
        }
        return finalList;
    }

    public static List<Unit> AI_FindEnemyInList(Unit.UnitTeam targetTeam, Collider[] overlappingItems)
    {

        List<Unit> finalList = new List<Unit>();

        foreach (Collider coll in overlappingItems)
        {
            Unit unit = coll.gameObject.GetComponent<Unit>();

            if (unit.unitTeam == targetTeam)
            {
                finalList.Add(unit);
            }
        }
        return finalList;
    }

}
