using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    
    public bool debug_Unit;
    public enum UnitTeam { teamA, teamB }
    public enum Unit_State { AIDecide, CommandMoveTo, CommandAttackTo }
    public Unit_State state;

    [Header("UnitData")]
    public UnitTeam unitTeam;
   
    public UnitData_Local data_local;
    public UnitData data;
    public int health;
    public int attackCD;

    [Header("Movement")]
    public Unit attackTarget;
    private Vector3 movetoTarget_Command;
    private int CommandTimer;


    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;

    private UnitAIController UAC;
    private UnitRangerController URC;
    private UnitCavalryController UCC;
    private UnitInfantryController UIC;
    private UnitSpearmanController USC;
    private UnitDragonController UDC;

    [HideInInspector]
    public  Rigidbody rb;
    private float chargeSpeed;
    [HideInInspector]
    public float knockBackTimer;
    [HideInInspector]
    public float currentAgentSpeed;

    [Header("UI")]
    public BarController BC;
    public GameObject selectionCircle;
    public GameObject unitSprite;

    void Start()
    {
        this.gameObject.name = data_local.data.unitType.ToString() +"||"+ unitTeam.ToString();
        UAC = this.transform.GetChild(2).GetComponent<UnitAIController>();
        URC = this.transform.GetChild(2).GetComponent<UnitRangerController>();
        UCC = this.transform.GetChild(2).GetComponent<UnitCavalryController>();
        UIC = this.transform.GetChild(2).GetComponent<UnitInfantryController>();
        USC = this.transform.GetChild(2).GetChild(0).GetComponent<UnitSpearmanController>();
        UDC = this.transform.GetChild(2).GetComponent<UnitDragonController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        attackTarget = null;

        if (data.unitType == UnitData.UnitType.cavalry) { agent.avoidancePriority = 50; }
        if (data.unitType == UnitData.UnitType.archer) { agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.monster) { agent.avoidancePriority = 20; }
        if (data.unitType == UnitData.UnitType.infantry) { agent.avoidancePriority = 300;}

        InputData();
        selectionCircle.SetActive(false);
    }

    public void InputData()
    {
        this.data = data_local.data;
        health = data.health;
        attackCD = 0;
        BC.SetValue_Initial(health, data.health);
        SetSpeed(data.moveSpeed);

        if (unitTeam == UnitTeam.teamA)
            BC.gameObject.transform.GetChild(1).GetChild(0).
                GetComponent<Image>().color = new Color32(255, 99, 0, 255);

        else
            BC.gameObject.transform.GetChild(1).GetChild(0).
                GetComponent<Image>().color = new Color32(99, 255, 0, 255);


        rb.mass = data.mass;
    }

    public void UnitSelect()
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(true);
    }
    public void UnitDeselect()
    {
        if(selectionCircle != null)
        selectionCircle.SetActive(false);
    }

    public void UnitAddTargetUnit(Unit _attackTarget)
    {
        if (_attackTarget == null) return;
        Debug.Log("3Count:" + _attackTarget.gameObject.name);

        float dis = Vector3.Distance(this.transform.position, _attackTarget.gameObject.transform.position);
        float timer = 3 + 1f * (float)dis / (float)data.moveSpeed;

        Debug.Log("timer:" + timer);
        CommandTimer = (int)timer;
        //selectionCircle.SetActive(false);
        this.attackTarget = _attackTarget;
        state = Unit_State.CommandAttackTo;
    }

    public void UnitAddTargetPos(Vector3 attackPos)
    {
        Debug.Log(attackPos);
        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
        float timer = 3 + 1f * (float)dis / (float)data.moveSpeed;

        Debug.Log("timer:" + timer);
        CommandTimer = (int)timer;

        //selectionCircle.SetActive(false);
        this.movetoTarget_Command = attackPos;
        state = Unit_State.CommandMoveTo;
    }


    void FixedUpdate()
    {
        currentAgentSpeed = agent.velocity.magnitude;
     
        knockBackTimer -= 4 * Time.fixedDeltaTime;
        if (knockBackTimer < 0)
        {
            rb.freezeRotation = false;
            agent.enabled = true;
            GetComponent<CapsuleCollider>().isTrigger = false;
        } 
    }


   
    public void AI_DecideAction()
    {
        if (health <= 0) return;
        attackCD--;

        if (agent.enabled)
        {

            //Move
            if (state == Unit_State.CommandMoveTo)
            {
                CommandTimer--;

                float dis = Vector3.Distance(this.transform.position, movetoTarget_Command);

                if (CommandTimer < 0 || dis < 1)
                {
                    state = Unit_State.AIDecide;
                }
                else
                {
                    agent.SetDestination(movetoTarget_Command);
                }
            }


            //Attack
            bool dontChangeAttackTarget = false;
            if (state == Unit_State.CommandAttackTo)
            {
                CommandTimer--;
                dontChangeAttackTarget = true;
                if (CommandTimer < 0 ||  attackTarget == null)
                {
                    state = Unit_State.AIDecide;
                    dontChangeAttackTarget = false;
                }
            }

            if (state == Unit_State.AIDecide ||state == Unit_State.CommandAttackTo)
            {
                if (data.unitType == UnitData.UnitType.infantry)
                {
                    if (UIC != null) UIC.AI_Warrior_Action(dontChangeAttackTarget);
                    if (USC != null) USC.AI_Warrior_Action(dontChangeAttackTarget);
                }

                if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery)
                {
                    URC.AI_RangeUnit_Action(dontChangeAttackTarget);
                }

                if (data.unitType == UnitData.UnitType.cavalry)
                {
                    UCC.AI_Cavalry_Action(dontChangeAttackTarget);
                }

                if (data.unitType == UnitData.UnitType.monster)
                {
                    UDC.AI_Monster_Action(dontChangeAttackTarget);
                }
            }
        }
    }

   

    public void AddDamage(int dam)
    {
        health -= dam;
        BC.SetValue_Initial(health, data.health);
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.45f);
        Destroy(this.gameObject);
    }
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
    public void SetChargeSpeed(float speed)
    {
       chargeSpeed = speed;
    }
    public void AddChargeSpeed()
    {
        if (chargeSpeed <= data.chargeSpeed_Max)
            chargeSpeed += Time.deltaTime * data.chargeSpeed_Accererate;

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
        //Debug.Log(knockBackForce);

        if (knockBackForce > (float)data.toughness/2)
        {
            agent.enabled = false;
            yield return new WaitForSeconds(waitTime/2);
            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(waitTime/2);

            if (attacker != null)
            {
                if (agent != null)
                {
                    knockBackForce = knockBackForce -= data.toughness / 2;
                    if (data.isShielded) knockBackForce = knockBackForce / 2;

                    Vector3 newVector = this.transform.position - attacker;

                    if (knockBackForce > 30) knockBackForce = 30;

                    //knockBackForce = knockBackForce + (30 - knockBackForce) * 0.5f;

                    if (isRange)
                    {
                        rb.AddForce(data.knockBackBonus * newVector * knockBackForce * 5f);
                    }

                    else
                    {
                        rb.AddForce(2f * data.knockBackBonus * newVector * knockBackForce, ForceMode.Impulse);
                    }
                  
                    knockBackTimer = 0.1f + knockBackForce / 15;
                }
            }
        }
    }
}
