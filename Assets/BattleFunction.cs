using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFunction : MonoBehaviour
{
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
    public static void Add_KnockBack_ToDefender(Transform attacker, Transform defender, Rigidbody rb_Defender, float knockBackForce)
    {
        Vector3 newVector = defender.transform.position - attacker.gameObject.transform.position;
        rb_Defender.velocity = Vector3.zero;
        rb_Defender.AddForce(newVector * knockBackForce, ForceMode.Impulse);
    }
}
