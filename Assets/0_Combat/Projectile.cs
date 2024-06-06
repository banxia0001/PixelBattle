using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Dynamic Stats")] 
    public bool cuaseAOE;
    public float attackDistance;

    [Header("Static Stats")]
    private float flySpeed;
    private bool isJave;
    private Vector3 arrowDropLocation;
    private Vector3 arrowStartLocation;
    private Vector2Int damage;
    private int knockBackBonus;
    private Unit.UnitTeam targetAttackTeam;
    private bool isDead = false;

    void Start()
    {
        
    }

    public void SetUpArror(Vector3 arrowDropLocation,Vector3 arrowStartLocation, Vector2Int damage, int knockback,float flySpeed, Unit.UnitTeam targetAttackTeam, bool isJave)
    {
        this.arrowDropLocation = arrowDropLocation;
        this.arrowStartLocation = arrowStartLocation;
        this.flySpeed = flySpeed;
        this.damage = damage;

        this.isJave = isJave;
        transform.LookAt(arrowDropLocation);
        this.targetAttackTeam = targetAttackTeam;
        this.knockBackBonus = knockback;

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
            transform.position = Vector3.MoveTowards(transform.position, arrowDropLocation, flySpeed);
        }
    }

    public List<Unit> Arrow_FindAllUnit_InSphere()
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapSphere(this.transform.position, attackDistance, LayerMask.GetMask("Unit"));
        if (overlappingItems == null) return null;
        else return AIFunctions.AI_FindEnemyInList(targetAttackTeam, overlappingItems);
    }

    public IEnumerator Death()
    {
        isDead = true;

        //[Arrow]
        if (!cuaseAOE)
        {
            Unit targetUnit = AIFunctions.Find_ClosestUnitInList(Arrow_FindAllUnit_InSphere(),this.transform);
            if (targetUnit != null)
            {
                int dam = BattleFunction.GetDamage(damage, targetUnit, false, isJave, false, true);
                BattleFunction.Attack(this.transform, dam, targetUnit);
                targetUnit.AddKnockBack(arrowStartLocation, knockBackBonus, 0.01f,true);
            }
        }

        //[an shell]
        //if (cuaseAOE)
        //{
        //    List<Unit> targetUnits = BattleFunction.Find_UnitsInRange(attackDistance, this.transform);
        //    if (targetUnits != null)
        //        if (targetUnits.Count != 0)
        //        {
        //            foreach (Unit unit in targetUnits)
        //            {
        //                int dam = BattleFunction.DamageCalculate(damMin, damMax, unit, false,isJave, false,true);
        //                //Debug.Log("!!");
        //                BattleFunction.Attack(this.transform, dam, unit);
        //                unit.AddKnockBack(this.transform, damMax / 2 + damMin / 2, 0.01f);
        //            }
        //        }
        //}


        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
