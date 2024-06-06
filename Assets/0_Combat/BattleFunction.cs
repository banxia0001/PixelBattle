using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFunction : MonoBehaviour
{
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



    public static List<Unit> FindEnemyUnit_InSphere(Vector3 postion, float range, Unit unit)
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapSphere(postion, range, LayerMask.GetMask("Unit"));
        if (overlappingItems == null) return null;
        else return AIFunctions.AI_FindEnemyInList(unit, overlappingItems);
    }

    public static List<Unit> FindFriendlyUnit_InSphere(Vector3 postion, float range, Unit unit)
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapSphere(postion, range, LayerMask.GetMask("Unit"));
        if (overlappingItems == null) return null;
        else return AIFunctions.AI_FindFriendlInList(unit, overlappingItems);
    }


    public static List<Unit> FindAllUnit_InSphere(Vector3 postion, float range, Unit.UnitTeam unitTeamFrom)
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamA;
        if (unitTeamFrom == Unit.UnitTeam.teamA) targetTeam = Unit.UnitTeam.teamB;

        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapSphere(postion, range, LayerMask.GetMask("Unit"));
        if (overlappingItems == null) return null;
        else return AIFunctions.AI_FindEnemyInList(targetTeam, overlappingItems);
    }


    public static int GetDamage(Vector2Int damage, Unit defender, bool isCharge, bool isJave, bool isAP, bool isRange)
    {
        int dam = Random.Range(damage.x, damage.y);
        int prot = defender.data.protection;

        if (isJave || isAP) prot = prot / 2;

        float finalDam = (float)dam * (100 - (float)prot)/100;
        if (isCharge)
        {
            if (defender.data.isSpear) 
                finalDam = finalDam / 2;
        }

        if (isRange)
        {
            if (defender.data.isShielded) 
                finalDam = finalDam / 2;
        }

        if (finalDam <= 0)
        {
            finalDam = 1;
        }
        return (int)finalDam;
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
}
