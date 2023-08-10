using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangerController : UnitAIController
{
    private bool canAttack;
    private Transform target;

    public override void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        canAttack = false;
        SetUp();
    }

    public void AI_RangeUnit_Action()
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        //[Attack]
        float dis = Vector3.Distance(this.transform.position, unit.attackTarget.transform.position);

        if (dis < unit.data.shootDis && unit.attackCD <= 0)
        {
            if (unit.data.unitType == UnitData.UnitType.archer)
            {
                unit.attackCD = unit.data.attackCD + Random.Range(-2, 2);
                SetUpAttack(unit.attackTarget.transform, unit.data.damageMin, unit.data.damageMax, false);
                AI_Stay(false);
            }
        }

        //[Stay]
        if (!CheckHoldStage()) {AI_Stay(false); return; }

        //[Find Enemy]
        else
        {
            if (unit.current_AI_Tactic == UnitData.AI_State_Tactic.shoot_n_keepDis)
            {
                bool shouldFlee = AI_CheckIfShouldFlee();

                if (dis < unit.data.shootDis * 0.7f) AI_Stay(false);

                else if (dis < unit.data.shootDis * 0.35f || shouldFlee) AI_Flee();

                else AI_MoveToward(unit.attackTarget.transform);
            }


            else if (unit.current_AI_Tactic == UnitData.AI_State_Tactic.shoot)
            {
                if (dis < unit.data.shootDis * 0.7f) AI_Stay(false);

                else if (dis < unit.data.shootDis * 0.35f) AI_Flee();

                else AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }
    }
    public void SetUpAttack(Transform target, int damMin, int damMax, bool causeAOE)
    {
        AI_LookAt(target);
        anim.SetTrigger("shoot");
        canAttack = true;
        this.target = target;
    }

    public void Update()
    {
        if (unit.attackCD <= unit.data.attackCD * 0.4f) unit.SetSpeed(0.1f);
        else unit.SetSpeed(unit.data.moveSpeed);

        if (canAttack)
        {
            if (target == null) return;

            if (attackTrigger.inAttacking)
            {
                canAttack = false;
                Shoot();
            }
        }
    }
    public void Shoot()
    {
        if (target == null) return;
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        int damMin = unit.data.damageMin;
        int damMax = unit.data.damageMax;

       
        GameObject Arrow = Instantiate(unit.data.ProjectilePrefab, this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();

        float offset = unit.data.arrowOffset;
        float dis = Vector3.Distance(unit.transform.position, target.transform.position);
        offset = offset * dis / 15;

        Vector3 targetPos = target.transform.position + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        ArrowScript.SetUpArror(targetPos, damMin, damMax, unit.data.arrowSpeed, targetTeam, unit.data.isJavelin);
    }
}
