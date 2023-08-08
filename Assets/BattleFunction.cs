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

    public static void Attack(Transform AttackerPos, int damMin,int damMax, Unit defender, bool isCharge)
    {
        int dam = Random.Range(damMin, damMax);
        int finalDam = dam - defender.data.armor + Random.Range(-4,4);

        if (isCharge)
        {
            if (defender.data.antiCharge)
            {
                finalDam = finalDam / 2;
            }
        }
        //Debug.Log("Defender:" + defender.name +" Dam:" +  attackerDam + "," + finalDam);
        if (finalDam > 0)
        {
            defender.AddDamage(finalDam);
        }
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
