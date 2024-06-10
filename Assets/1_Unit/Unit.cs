using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class Unit : MonoBehaviour
{
    public bool debug_Unit;
    public enum UnitTeam { teamA, teamB }

    [Header("UnitData")]
    public UnitTeam unitTeam;
    public UnitData_Local data_local;
    [HideInInspector] public UnitData data;
    [HideInInspector] public int health;
    [HideInInspector] public int attackCD;

    [Header("Movement")]
    public bool inRootMotion;
    public bool inKnockBack;

    [HideInInspector] public Unit attackTarget;
    private Vector3 movetoTarget_Command;
    private int CommandTimer;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    [Header("AI Behaviors")]
    private Ranger ranger;
    private Cavarly cavarly;
    private Infantry infantry;
    private Spearman spearman;
    private DragonMonster dragonMonster;
    private bool receivedTarget = false;

    [Header("Physics")]
    [HideInInspector] public Rigidbody rb;
    private float chargeSpeed;
    [HideInInspector] public float knockBackTimer;
    [HideInInspector] public float currentAgentSpeed;

    [Header("UI")]
    public BarController healthBar;
    public GameObject canvas;

    [Header("Reference")]
    public GameController GC;

    void Start()
    {
        this.gameObject.name = data_local.data.unitType.ToString() +"||"+ unitTeam.ToString();

        //[Get All the AI Behavior Scripts]
        ranger = this.transform.GetChild(2).GetComponent<Ranger>();
        cavarly = this.transform.GetChild(2).GetComponent<Cavarly>();
        infantry = this.transform.GetChild(2).GetComponent<Infantry>();
        if (this.transform.GetChild(2).childCount != 0)
            spearman = this.transform.GetChild(2).GetChild(0).GetComponent<Spearman>();
        dragonMonster = this.transform.GetChild(2).GetComponent<DragonMonster>();

        //[NavMesh]
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        GC = FindObjectOfType<GameController>();

        if (data.unitType == UnitData.UnitType.cavalry) { agent.avoidancePriority = 50; }
        if (data.unitType == UnitData.UnitType.archer) { agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.monster) { agent.avoidancePriority = 20; }
        if (data.unitType == UnitData.UnitType.infantry) { agent.avoidancePriority = 300; }

        //[Set Up]
        canvas.gameObject.SetActive(false);
        attackTarget = null;
        inKnockBack = false;
        inRootMotion = false;
        InputData();

        //[Get Into List]
        FindObjectOfType<GameController>().teams[(int)unitTeam].InsertUnit(this);
    }

    public void InputData()
    {
        this.data = data_local.data;
        health = data.health;
        attackCD = 0;

        healthBar.SetValue_Initial(health, data.health);
        agent.speed = data.moveSpeed;

        if (unitTeam == UnitTeam.teamA) healthBar.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 99, 0, 255);
        else healthBar.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(99, 255, 0, 255);

        rb.mass = data.mass;
    }

   

    void FixedUpdate()
    {
        currentAgentSpeed = agent.velocity.magnitude;
        if (!inKnockBack && !inRootMotion)
        {
            if (agent.enabled == false)
            {
                agent.enabled = true;
                rb.freezeRotation = false;
            }
        }
    }


    public void DecideAction()
    {
        if (health <= 0) return;
        attackCD--;

        if (agent.enabled)
        {
            if (data.unitType == UnitData.UnitType.infantry)
            {
                if (infantry != null) infantry.AI_Warrior_Action();
                if (spearman != null) spearman.AI_Warrior_Action();
            }

            if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery) ranger.AI_RangeUnit_Action();
            if (data.unitType == UnitData.UnitType.cavalry) cavarly.AI_Cavalry_Action();
            if (data.unitType == UnitData.UnitType.monster) dragonMonster.AI_Monster_Action();
        }
    }


    public void FindTarget()
    {
        if (health <= 0) return;
        if (receivedTarget)
        {
            receivedTarget = false; return;
        }

        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;
        //If no unit in enemy list, return null;
        if (GC.teams[(int)targetTeam].P <= 0)
        { 
            attackTarget = null; return; 
        }

        //If unit is melee, try find enemy in frontline
        bool focusFrontline = false;
        if (data.unitType == UnitData.UnitType.archer ||data.unitType == UnitData.UnitType.artillery ||data.unitType == UnitData.UnitType.cavalry) focusFrontline = false;
        else focusFrontline = true;

        //[If target empty, Go find new target]
        if (attackTarget == null)
        {
            FindTarget_2(targetTeam, focusFrontline);
        }

        //[See if unit should find new target]
        else
        {
            float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if (data.unitType == UnitData.UnitType.infantry)
            { if (dis > 2.5f) FindTarget_2(targetTeam, focusFrontline); }

            else if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery)
            { if (dis > data.shootDis * 1.2f) FindTarget_2(targetTeam, focusFrontline); }

            else if (data.unitType == UnitData.UnitType.cavalry)
            { if (dis > 5f) FindTarget_2(targetTeam, focusFrontline); }

            else if (dis > 3f) attackTarget = FindClosestTarget(data.current_AI_Target, false);
        }

        if (attackTarget != null)
        {
            SendAttackTarget(attackTarget);
        }
    }
    public void FindTarget_2(Unit.UnitTeam targetTeam, bool targetFrontline)
    {
        int choice = UnityEngine.Random.Range(0, 20);

        if (choice < 12) attackTarget = FindClosestTarget(data.current_AI_Target, targetFrontline);
        else attackTarget = FindClosestTarget(data.current_AI_Target_Secondly, targetFrontline);

        if (attackTarget == null)
        {
            if (choice < 12)attackTarget = FindClosestTarget(data.current_AI_Target_Secondly, targetFrontline);
            else attackTarget = FindClosestTarget(data.current_AI_Target, targetFrontline);
        }

        if (attackTarget == null)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this, false, targetFrontline);
        }
    }

    public Unit FindClosestTarget(UnitData.AI_State_FindTarget current_AI_Target, bool targrtFrontLine)
    {
        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        if (current_AI_Target == UnitData.AI_State_FindTarget.findValueableTarget_InDistance)
        {
            return AIFunctions.AI_Find_ValueableUnit(targetTeam, this, false);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findValueableTarget_InFrontline)
        {
            return AIFunctions.AI_Find_ValueableUnit(targetTeam, this, true);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findClosest)
        {
            return AIFunctions.AI_Find_ClosestUnit(targetTeam, this, true, true);
        }

        if (current_AI_Target == UnitData.AI_State_FindTarget.findClosestWarrior || current_AI_Target == UnitData.AI_State_FindTarget.findClosestTarget_InFrontline)
        {
            return AIFunctions.AI_Find_ClosestUnit_2(targetTeam, current_AI_Target, this, true);
        }

        else
        {
            return AIFunctions.AI_Find_ClosestUnit_2(targetTeam, current_AI_Target, this, false);
        }
    }
    public void ReceiveAttackTarget(Unit target)
    {
        this.attackTarget = target;
        receivedTarget = true;
    }
    public void SendAttackTarget(Unit attackTarget)
    {
        List<Unit> unitGroup = BattleFunction.FindFriendlyUnit_InSphere(this.transform.position, 3f, this);
        if (unitGroup != null && unitGroup.Count != 0)
        {
            foreach (Unit unit in unitGroup)
            {
                if (unit.data_local.ID == this.data_local.ID)
                {
                    unit.ReceiveAttackTarget(attackTarget);
                }
            }
        }
    }









    public void AddDamage(int dam)
    {
        health -= dam;
        canvas.gameObject.SetActive(true);
        healthBar.SetValue_Initial(health, data.health);
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }

    private bool isDying;
    private IEnumerator Death()
    {
        if (!isDying)
        {
            isDying = true;
            //[Remove Into List]
            FindObjectOfType<GameController>().teams[(int)unitTeam].RemoveUnit(this);
            yield return new WaitForSeconds(0.45f);
            Destroy(this.gameObject);
        }
    }










    #region Velocity Functions

    public void SetChargeSpeed(float speed)
    {
       chargeSpeed = speed;
    }
    public void AddChargeSpeed()
    {
        if (chargeSpeed <= data.chargeSpeed_Max)
            chargeSpeed += Time.fixedDeltaTime * data.chargeSpeed_Accererate;

        agent.speed = data.moveSpeed + chargeSpeed;
    }
    public void AddKnockBack(Transform attacker, float knockBackForce, float waitTime, bool isRange)
    {
        StartCoroutine(_AddKnockBack(attacker.position, knockBackForce, waitTime, isRange));
    }
    public void AddKnockBack(Vector3 attacker, float knockBackForce, float waitTime, bool isRange)
    {
        StartCoroutine(_AddKnockBack(attacker, knockBackForce, waitTime, isRange));
    }

    private IEnumerator _AddKnockBack(Vector3 attacker, float knockBackForce, float waitTime, bool isRange)
    {
        if (knockBackForce > (float)data.toughness/2)
        {
            if (attacker != null && agent != null)
            {
                inKnockBack = true;
                agent.enabled = false;
                rb.freezeRotation = true;
                yield return new WaitForSeconds(waitTime / 2);
                rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(waitTime / 2);

                knockBackForce = knockBackForce -= data.toughness / 2;
                if (data.isShielded) knockBackForce = knockBackForce / 2;

                Vector3 newVector = this.transform.position - attacker;

                if (knockBackForce > 30) knockBackForce = 30;

                if (isRange)
                {
                    rb.AddForce(data.knockBackDecrease * newVector * knockBackForce, ForceMode.Impulse);
                }

                else
                {
                    rb.AddForce(2f * data.knockBackDecrease * newVector * knockBackForce, ForceMode.Impulse);
                }

                StartCoroutine(RecoverFromKnockback(0.1f + knockBackForce / 15));
            }
        }
    }

    private IEnumerator RecoverFromKnockback(float timer)
    {
        inKnockBack = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        yield return new WaitForSeconds(timer / 3);
        inKnockBack = false;
        GetComponent<CapsuleCollider>().isTrigger = false;
    }
    #endregion
}
