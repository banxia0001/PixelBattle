using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFunction : MonoBehaviour
{
    public static List<Unit> Find_UnitsInRange(float range, Transform startPos)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();

                float thisDis = Vector3.Distance(startPos.transform.position, unit.transform.position);
                if (thisDis < range)
                    unitList.Add(unit);
            }
        return unitList;
    }
    public static List<Unit> Find_UnitsInRange(Unit.UnitTeam targetTeam,float range, Transform startPos)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();

                if (unit.unitTeam == targetTeam)
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
    public static List<Unit> Find_TargetUnitGroup(Unit.UnitTeam targetTeam, UnitData.UnitType targetType)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();
                if (unit.unitTeam == targetTeam && unit.data.unitType == targetType)
                {
                    unitList.Add(unit);
                }
            }
        return unitList;
    }
    public static List<Unit> Find_TargetUnitGroup(Unit.UnitTeam targetTeam)
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
        List<Unit> unitList = new List<Unit>();

        if (allUnits != null)
            foreach (GameObject unitOb in allUnits)
            {
                Unit unit = unitOb.GetComponent<Unit>();
                if (unit.unitTeam == targetTeam)
                {
                    unitList.Add(unit);
                }
            }
        return unitList;
    }

    public static int DamageCalculate(int damMin, int damMax, Unit defender, bool isCharge, bool isJave)
    {
        int dam = Random.Range(damMin, damMax);
        int amr = defender.data.armor;

        if (isJave)
        {
            amr = amr / 2;
        }

        int finalDam = dam - amr + Random.Range(-4, 4);

        if (isCharge)
        {
            if (defender.data.isSpear)
            {
                finalDam = finalDam / 2;
            }

        }

        if (finalDam < 0)
        {
            finalDam = 0;
        }
        return dam;

    }
    public static void Attack(Transform AttackerPos, int dam, Unit defender)
    {
        defender.AddDamage(dam);
    }


    public static Vector3 DistantPoint(Vector3 pointS, Vector3 pointM)
    {
        GameObject MyTransform = Instantiate(Resources.Load<GameObject>("Null"), pointS, Quaternion.identity);
        MyTransform.transform.LookAt(pointM);
        Vector3 distantPosition = pointM + Quaternion.Euler(0, 0, MyTransform.transform.eulerAngles.y) * Vector3.right * -3f;
        Destroy(MyTransform);
        return distantPosition;
    }
}
