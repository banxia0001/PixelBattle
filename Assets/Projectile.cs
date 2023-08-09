using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
 
    public float flySpeed;
    public bool cuaseAOE;
    public bool isJave;
    public float attackDistance;
    private Vector3 arrowDropLocation;
    private int damMin;
    private int damMax;
    private UnitData.UnitTeam targetAttackTeam;
    private bool isDead = false;

    void Start()
    {
        
    }

    public void SetUpArror(Vector3 arrowDropLocation, int damMin, int damMax, float flySpeed, UnitData.UnitTeam targetAttackTeam, bool isJave)
    {
        this.arrowDropLocation = arrowDropLocation;
        this.flySpeed = flySpeed;
        this.damMin = damMin;
        this.damMax = damMax;
        this.isJave = isJave;
        transform.LookAt(arrowDropLocation);
        this.targetAttackTeam = targetAttackTeam;
        isDead = false;
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead) return;

        float dist = Vector3.Distance(transform.position, arrowDropLocation);
        if (dist < 0.01f)
        {
            StartCoroutine(Death());
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, arrowDropLocation,  flySpeed);
        }
    }

    public IEnumerator Death()
    {
        isDead = true;

        //[an arrow]
        if (!cuaseAOE)
        {
            Unit targetUnit = BattleFunction.Find_ClosestUnitInList(BattleFunction.Find_UnitsInRange(targetAttackTeam, attackDistance, this.transform), this.transform);
            if (targetUnit != null)
            {
                int dam = BattleFunction.DamageCalculate(damMin, damMax, targetUnit, false,isJave);
                //Debug.Log("!!");
                BattleFunction.Attack(this.transform, dam, targetUnit);
                StartCoroutine(targetUnit.AddKnockBack(this.transform, 2f));
            }
        }

        //[an shell]
        if (cuaseAOE)
        {
            List<Unit> targetUnits = BattleFunction.Find_UnitsInRange(attackDistance, this.transform);
            if (targetUnits != null)
                if (targetUnits.Count != 0)
                {
                    foreach (Unit unit in targetUnits)
                    {
                        int dam = BattleFunction.DamageCalculate(damMin, damMax, unit, false,isJave);
                        //Debug.Log("!!");
                        BattleFunction.Attack(this.transform, dam, unit);
                        StartCoroutine(unit.AddKnockBack(this.transform, 3f));
                    }
                }
        }


        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
