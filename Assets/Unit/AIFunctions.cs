using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFunctions : MonoBehaviour
{
    //public static List<Unit> Find_UnitsInRange(float range, Transform startPos)
    //{
    //    GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
    //    List<Unit> unitList = new List<Unit>();

    //    if (allUnits != null)
    //        foreach (GameObject unitOb in allUnits)
    //        {
    //            Unit unit = unitOb.GetComponent<Unit>();

    //            float thisDis = Vector3.Distance(startPos.transform.position, unit.transform.position);
    //            if (thisDis < range)
    //                unitList.Add(unit);
    //        }
    //    return unitList;
    //}
    //public static List<Unit> Find_UnitsInRange(Unit.UnitTeam targetTeam, float range, Transform startPos)
    //{
    //    GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
    //    List<Unit> unitList = new List<Unit>();

    //    if (allUnits != null)
    //        foreach (GameObject unitOb in allUnits)
    //        {
    //            Unit unit = unitOb.GetComponent<Unit>();

    //            if (unit.unitTeam == targetTeam)
    //            {
    //                float thisDis = Vector3.Distance(startPos.transform.position, unit.transform.position);
    //                if (thisDis < range)
    //                    unitList.Add(unit);
    //            }
    //        }
    //    return unitList;
    //}

    public static Unit Find_ClosestUnitInList(List<Unit> unitGroup, Transform startPos)
    {
        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisDis = Vector3.Distance(startPos.transform.position, unitGroup[i].transform.position);

            if (closetDis > thisDis)
            {
                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }

    public static Unit AI_Find_ClosestUnit(Unit.UnitTeam targetTeam, Unit startUnit, bool targetFrontline, bool targetValue)
    {
        List<Unit> unitGroup = BattleFunction.Find_TargetUnitGroup(targetTeam);

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetDis = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisDis = 0;

            float value = 0;

            if (targetValue)
            {
                value = unitGroup[i].data_local.UnitValue * 3;

            }
            if (targetFrontline)
            {
                Vector3 pos1 = new Vector3(GameController.frontLine, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2) - value;
            }

            else
            {
                Vector3 pos1 = new Vector3(startUnit.transform.position.x, 0, startUnit.transform.position.z * 2);
                Vector3 pos2 = new Vector3(unitGroup[i].transform.position.x, 0, unitGroup[i].transform.position.z * 2);
                thisDis = Vector3.Distance(pos1, pos2) - value;
            }


            if (closetDis > thisDis)
            {
                closetDis = thisDis;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }

    public static Unit AI_Find_ValueableUnit(Unit.UnitTeam targetTeam, Unit startUnit, bool targetFrontline)
    {
        GameController GC = FindObjectOfType<GameController>(false);
        TeamController targetUnitTeam = null;
        if (targetTeam == Unit.UnitTeam.teamA) targetUnitTeam = GC.teamA;
        if (targetTeam == Unit.UnitTeam.teamB) targetUnitTeam = GC.teamB;
        List<Unit> unitGroup = new List<Unit>();

        if (targetFrontline)
        {
            Vector3 pos = new Vector3(GameController.frontLine, 0, startUnit.transform.position.z);
            unitGroup = BattleFunction.FindAllUnit_InSphere(pos, 8f, startUnit);
        }

        else
        {
            float offset = 5f;
            if (startUnit.unitTeam == Unit.UnitTeam.teamB) offset = -5f;
            Vector3 pos = new Vector3(startUnit.transform.position.x + offset, 0, startUnit.transform.position.z);
            unitGroup = BattleFunction.FindAllUnit_InSphere(pos, 8f, startUnit);
        }

        if (unitGroup == null) return null;
        if (unitGroup.Count == 0) return null;

        Unit closedUnit = unitGroup[0];
        float closetValue = 9999999;

        for (int i = 0; i < unitGroup.Count; i++)
        {
            float thisValue = -unitGroup[i].data_local.UnitValue;
            if (closetValue > thisValue)
            {
                closetValue = thisValue;
                closedUnit = unitGroup[i];
            }
        }
        return closedUnit;
    }
    public static Unit AI_Find_ClosestUnit_2(Unit.UnitTeam targetTeam, UnitData.AI_State_FindTarget targetType, Unit startUnit, bool targetFrontline)
    {
        GameController GC = FindObjectOfType<GameController>(false);
        TeamController targetUnitTeam = null;
        if (targetTeam == Unit.UnitTeam.teamA) targetUnitTeam = GC.teamA;
        if (targetTeam == Unit.UnitTeam.teamB) targetUnitTeam = GC.teamB;

        List<Unit> unitGroup = new List<Unit>();

        if (targetType == UnitData.AI_State_FindTarget.findClosestTarget_InFrontline)
        {
            Vector3 pos = new Vector3(GameController.frontLine, 0, startUnit.transform.position.z);
            unitGroup = BattleFunction.FindAllUnit_InSphere(pos, 8f, startUnit);
        }

        if (targetType == UnitData.AI_State_FindTarget.findClosest)
        {
            float offset = 5f;
            if (startUnit.unitTeam == Unit.UnitTeam.teamB) offset = -5f;
             Vector3 pos = new Vector3(startUnit.transform.position.x + offset, 0, startUnit.transform.position.z);
            unitGroup = BattleFunction.FindAllUnit_InSphere(pos, 8f, startUnit);
        }

        if (targetType == UnitData.AI_State_FindTarget.findClosestArcher) unitGroup = targetUnitTeam.archerList;
        if (targetType == UnitData.AI_State_FindTarget.findClosestCavalry) unitGroup = targetUnitTeam.cavalryList;
        if (targetType == UnitData.AI_State_FindTarget.findClosestMonster) unitGroup = targetUnitTeam.monsterList;
        if (targetType == UnitData.AI_State_FindTarget.findClosestArtillery) unitGroup = targetUnitTeam.artilleryList;

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

    //public static List<Unit> AI_Melee_FindAllUnit_InHitBox(Unit attacker,float hitboxLength, float hitboxWidth, Vector3 postion)
    //{

    //    Collider[] overlappingItems;
    //    overlappingItems = Physics.OverlapBox(postion, new Vector3(hitboxWidth, 1f, hitboxLength), Quaternion.identity, LayerMask.GetMask("Unit"));
    //    if (overlappingItems == null) return null;
    //    else return AIFunctions.AI_FindEnemyInList(attacker, overlappingItems);
    //}

    //public static List<Unit> AI_Melee_FindAllUnit_InHitBox(Unit.UnitTeam attackerTeam, float hitboxLength, float hitboxWidth, Vector3 postion)
    //{
    //    Unit.UnitTeam targetTeam = Unit.UnitTeam.teamA;
    //    if (attackerTeam == Unit.UnitTeam.teamA) targetTeam = Unit.UnitTeam.teamB;

    //    Collider[] overlappingItems;
    //    overlappingItems = Physics.OverlapBox(postion, new Vector3(hitboxWidth, 1f, hitboxLength), Quaternion.identity, LayerMask.GetMask("Unit"));
    //    if (overlappingItems == null) return null;
    //    else return AIFunctions.AI_FindEnemyInList(targetTeam, overlappingItems);
    //}



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
