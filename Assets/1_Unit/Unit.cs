using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    public bool debug_Unit;
    public enum UnitTeam { teamA, teamB }

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

    [Header("AI Behaviors")]
    private Ranger ranger;
    private Cavarly cavarly;
    private Infantry infantry;
    private Spearman spearman;
    private DragonMonster dragonMonster;

    [Header("Physics")]
    [HideInInspector] public Rigidbody rb;
    private float chargeSpeed;
    [HideInInspector] public float knockBackTimer;
    [HideInInspector] public float currentAgentSpeed;

    [Header("UI")]
    public BarController BC;
    public GameObject unitSprite;

    void Start()
    {
        this.gameObject.name = data_local.data.unitType.ToString() +"||"+ unitTeam.ToString();

        //[Get All the AI Behavior Scripts]
        ranger = this.transform.GetChild(2).GetComponent<Ranger>();
        cavarly = this.transform.GetChild(2).GetComponent<Cavarly>();
        infantry = this.transform.GetChild(2).GetComponent<Infantry>();
        spearman = this.transform.GetChild(2).GetChild(0).GetComponent<Spearman>();
        dragonMonster = this.transform.GetChild(2).GetComponent<DragonMonster>();

        //[NavMesh]
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        if (data.unitType == UnitData.UnitType.cavalry) { agent.avoidancePriority = 50; }
        if (data.unitType == UnitData.UnitType.archer) { agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.monster) { agent.avoidancePriority = 20; }
        if (data.unitType == UnitData.UnitType.infantry) { agent.avoidancePriority = 300; }


        attackTarget = null;
        InputData();
        //selectionCircle.SetActive(false);
    }

    public void InputData()
    {
        this.data = data_local.data;
        health = data.health;
        attackCD = 0;

        BC.SetValue_Initial(health, data.health);
        agent.speed = data.moveSpeed;

        if (unitTeam == UnitTeam.teamA)
            BC.gameObject.transform.GetChild(1).GetChild(0).
                GetComponent<Image>().color = new Color32(255, 99, 0, 255);

        else
            BC.gameObject.transform.GetChild(1).GetChild(0).
                GetComponent<Image>().color = new Color32(99, 255, 0, 255);

        rb.mass = data.mass;
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
            if (data.unitType == UnitData.UnitType.infantry)
            {
                if (infantry != null) infantry.AI_Warrior_Action(false);
                if (spearman != null) spearman.AI_Warrior_Action(false);
            }

            if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery) ranger.AI_RangeUnit_Action(false);
            if (data.unitType == UnitData.UnitType.cavalry) cavarly.AI_Cavalry_Action(false);
            if (data.unitType == UnitData.UnitType.monster) dragonMonster.AI_Monster_Action(false);
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
