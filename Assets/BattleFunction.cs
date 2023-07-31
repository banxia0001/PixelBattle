using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFunction : MonoBehaviour
{
    public static List<Unit> Find_UnitsInRange(UnitData.UnitTeam targetTeam,float range, Transform startPos)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();
                if (unit.data.unitTeam == targetTeam)
                {
                    float thisDis = Vector3.Distance(startPos.transform.position, unit.transform.position);
                    if (thisDis < range)
                        unitList.Add(unit);
                }
            }
        return unitList;
    }

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
    public static List<Unit> Find_TargetUnitGroup(UnitData.UnitTeam targetTeam, UnitData.UnitType targetType)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();
                if (unit.data.unitTeam == targetTeam && unit.data.unitType == targetType)
                {
                    unitList.Add(unit);
                }
            }
        return unitList;
    }
    public static List<Unit> Find_TargetUnitGroup(UnitData.UnitTeam targetTeam)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();
                if (unit.data.unitTeam == targetTeam)
                {
                    unitList.Add(unit);
                }
            }
        return unitList;
    }

    public static void Attack(Transform AttackerPos, int attackerDam, Unit defender)
    {
        int finalDam = attackerDam - defender.data.armor + Random.Range(-2,2);
        //Debug.Log("Defender:" + defender.name +" Dam:" +  attackerDam + "," + finalDam);
        if (finalDam > 0)
        {
            defender.GetDamage(finalDam);
        }
    }

    public static void Add_KnockBack_ToDefender(Transform attacker, Transform defender, Rigidbody rb_Defender, float knockBackForce)
    {
        Vector3 newVector = defender.transform.position - attacker.gameObject.transform.position;
        rb_Defender.velocity = Vector3.zero;
        rb_Defender.AddForce(newVector * knockBackForce, ForceMode.Impulse);
    }
}
