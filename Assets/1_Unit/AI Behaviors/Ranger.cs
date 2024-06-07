using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : UnitAIController
{
    private float restreatTimer;
    private bool waitForFire;
    private Transform target;

    public override void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        waitForFire = false;
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
        viewPoint = this.transform.parent.GetComponent<ViewPoint>();
    }

    public void AI_RangeUnit_Action(bool remainAttackTarget)
    {
        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

        //[FindTarget]
        if(!remainAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        //[Attack]
        float dis = Vector3.Distance(this.transform.position, unit.attackTarget.transform.position);

        if (dis < unit.data.shootDis && unit.attackCD <= 0)
        {
            if (unit.data.unitType == UnitData.UnitType.archer)
            {
                unit.attackCD = unit.data.attackCD + Random.Range(-2, 2);
                SetUpAttack(unit.attackTarget.transform);
                AI_Stay(false);
            }
        }


        //[Find Enemy]
        else
        {
            if (restreatTimer > 0)
            {
                AI_GoToBase(unit.unitTeam);
            }
            if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot_n_keepDis)
            {
                bool haveEnemy = viewPoint.CheckHitShpere(1.55f);
                if (haveEnemy)
                {
                    restreatTimer = 2f;
                }
                else if (dis < unit.data.shootDis * 0.5f) AI_Stay(false);

                else AI_MoveToward(unit.attackTarget.gameObject.transform);
            }


            else if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot)
            {
                bool haveEnemy = viewPoint.CheckHitShpere(1);

                if (haveEnemy)
                {
                    restreatTimer = 3f;
                }
                else if (dis < unit.data.shootDis * 0.5f) AI_Stay(false);

                else AI_MoveToward(unit.attackTarget.gameObject.transform);

            }
        }
    }





    public void SetUpAttack(Transform target)
    {
        AI_LookAt(target);
        anim.SetTrigger("shoot");
        waitForFire = true;
        this.target = target;
    }

    //[Updates]
    public void FixedUpdate()
    {
        restreatTimer -= Time.fixedDeltaTime;

        if (waitForFire)
        {
            if (target == null) return;
            if (attackTrigger.canAttack)
            {
                waitForFire = false;
                Shoot();
            }
        }
    }
    public void Shoot()
    {
        if (target == null) return;
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unit.unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        GameObject Arrow = Instantiate(unit.data.ProjectilePrefab, this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();

        float offset = unit.data.arrowOffset;
        float dis = Vector3.Distance(unit.transform.position, target.transform.position);
        offset = offset * dis / 15;

        Vector3 targetPos = target.transform.position + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        ArrowScript.SetUpArror(targetPos,this.unit.transform.position, unit.data.damage, unit.data.knockBackForce, unit.data.arrowSpeed, targetTeam, unit.data.isJavelin);
    }
}
