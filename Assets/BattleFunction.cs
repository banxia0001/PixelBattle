using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static int DamageCalculate(int damMin, int damMax, Unit defender, bool isCharge, bool isJave, bool isAP, bool isRange)
    {
        int dam = Random.Range(damMin, damMax);
        int amr = defender.data.armor;

        if (isJave || isAP)
        {
            amr = amr / 2;
        }

        int finalDam = dam - amr + Random.Range(-4, 4);

        if (isCharge)
        {
            if (defender.data.isSpear) finalDam = finalDam / 2;
        }

        if (isRange)
        {
            if (defender.data.isShielded) finalDam = finalDam / 2;
        }

        if (finalDam <= 0)
        {
            finalDam = 1;
        }
        return finalDam;

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

    public static void PopText(RectTransform rect, float offsetY, string TextType)
    {
     
        GameObject textOb = null;

        textOb = Resources.Load<GameObject>("UI/" + TextType);
     
        GameObject go = Instantiate(textOb, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        go.transform.SetParent(rect.transform);

        go.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
        go.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        go.GetComponent<RectTransform>().localPosition = new Vector3(offsetY, 0, 0);
    }

    public static int[,] CheckGTRate(float ratio)
    {
        ratio = ratio * 10;

        if (ratio <= 3.4f)
        {
            return new int[9, 1];
        }
        if (ratio <= 3.8f)
        {
            return new int[8, 2];
        }
        if (ratio <= 4.2f)
        {
            return new int[7, 3];
        }
        if (ratio <= 4.6f)
        {
            return new int[6, 4];
        }
        if (ratio <= 5f)
        {
            return new int[5, 5];
        }
        if (ratio <= 5.4f)
        {
            return new int[4, 6];
        }
        if (ratio <= 5.8f)
        {
            return new int[3, 7];
        }
        if (ratio <= 6.2f)
        {
            return new int[4, 8];
        }
        if (ratio <= 6.6f)
        {
            return new int[1, 9];
        }

        else return new int[1, 9];
    }
}
