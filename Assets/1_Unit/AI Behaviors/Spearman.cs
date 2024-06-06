using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : UnitAIController
{
    public override void Start()
    {
      
        unit = this.transform.parent.parent.GetComponent<Unit>();
        viewPoint = this.transform.parent.parent.GetChild(1).GetComponent<ViewPoint>();
        anim = this.GetComponent<Animator>();
        parentOB = this.transform.parent;
        angleShould = parentOB.parent.transform.eulerAngles.y + 5f;
        angleNow = angleShould;
    }

    public enum SpearmanState
    { 
        moveToTarget,
        moveAwayFromTarget,
    }

    public SpearmanState spearmanState;

    private float changeBodyRatio;
    public void AI_Warrior_Action(bool dontChangeAttackTarget)
    {

        unit.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[FindEnemy]
        if(!dontChangeAttackTarget)
        FindAttackTarget();

        //[Stay]
        if (unit.attackTarget == null)
        {
            AI_GoToEnemyBase(unit.unitTeam);
            return;
        }

        Unit closestCav = AI_FindClosestTargetInList(UnitData.AI_State_FindTarget.findClosestCavalry, false);
        if (closestCav != null)
        {
            float dis_CavAlerm = Vector3.Distance(unit.transform.position, closestCav.transform.position);
            if (dis_CavAlerm < 4f)
            {
                unit.attackTarget = closestCav;
                AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }

        float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
        //if (unit.debug_Unit) Debug.Log(dis);

        //[Find Enemy]
        if (unit.data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
        {
            //Debug.Log(dis);

            if (spearmanState == SpearmanState.moveAwayFromTarget) 
            {
                gobackT--;
                if(gobackT == 0) spearmanState = SpearmanState.moveToTarget;
            }
            if (dis < unit.data.moveStopDis * 2.5f && unit.attackCD < unit.data.attackCD/2)
            {
                if (spearmanState != SpearmanState.moveAwayFromTarget)
                {
                    gobackT = 4;
                    angleNow = this.parentOB.transform.eulerAngles.y;
                    spearmanState = SpearmanState.moveAwayFromTarget;
                }
            }

            if (spearmanState == SpearmanState.moveAwayFromTarget)
            {
                AI_GoToBase(unit.unitTeam);
            }

            else
            {
                AI_MoveToward(unit.attackTarget.gameObject.transform);
            }
        }
    }

    private int gobackT;
    public float angleNow;
    public float angleShould;
    public override void LateUpdate()
    {
        parentOB.parent.GetChild(1).eulerAngles = new Vector3(0, this.transform.parent.parent.eulerAngles.y, 0);
        this.unit.unitSprite.transform.eulerAngles = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, 0, 90);

        ////[Setup Attack]
        if (unit.attackTarget != null && unit.attackCD < 0)
        {
            //float dis = Vector3.Distance(unit.transform.position, unit.attackTarget.transform.position);
            //Check can attack
            bool canAttack_InHitBox = viewPoint.CheckHitShpere(1);

            if (canAttack_InHitBox)
            {
                AI_LookAt(unit.attackTarget.transform);
                angleNow = parentOB.parent.transform.eulerAngles.y + 5f;
                unit.attackCD = unit.data.attackCD + Random.Range(-1, 1);
                SetUpAttack(unit.data.damageMin, unit.data.damageMax, unit.data.weaponAOENum, unit.data.isAP);
            }
        }

        angleShould = parentOB.parent.transform.eulerAngles.y + 5f;
        parentOB.eulerAngles = new Vector3(0, angleNow, 0);
        if (gobackT < 2)
        {
            if (Mathf.Abs(angleNow - angleShould) < 5)
            {
                angleNow = angleShould;
            }
            else
            {
                if (angleNow > angleShould) angleNow -= 120 * Time.deltaTime;
                if (angleNow < angleShould) angleNow += 120 * Time.deltaTime;
            }
        }
    }

    public Transform parentOB;

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    public override void SetUpAttack(int damMin, int damMax, int weaponAOENum, bool causeAP)
    {
        attackTrigger.InputData(this, damMin, damMax, weaponAOENum, causeAP);

        //if (VP.CheckSphere("Right"))
        //{
        //    AI_LookAt(VP.eyeR.transform);
        //    anim.SetTrigger("attack");
        //    return;
        //}

        //if (VP.CheckSphere("Left"))
        //{
        //    AI_LookAt(VP.eyeL.transform);
        //    anim.SetTrigger("attack");
        //    return;
        //}

        anim.SetTrigger("attack");
    }
}
