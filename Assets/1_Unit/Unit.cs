using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.XR;

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

    [Header("Physics")]
    [HideInInspector] public Rigidbody rb;
    private float chargeSpeed;
    [HideInInspector] public float knockBackTimer;
    [HideInInspector] public float currentAgentSpeed;

    [Header("UI")]
    public BarController healthBar;
    public GameObject canvas;

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
        if (data.unitType == UnitData.UnitType.cavalry) { agent.avoidancePriority = 50; }
        if (data.unitType == UnitData.UnitType.archer) { agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.monster) { agent.avoidancePriority = 20; }
        if (data.unitType == UnitData.UnitType.infantry) { agent.avoidancePriority = 300; }

        //[Set Up]
        attackTarget = null;
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
                agent.enabled = false;
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
        GetComponent<CapsuleCollider>().isTrigger = true;
        yield return new WaitForSeconds(timer / 3);
        rb.freezeRotation = false;
        agent.enabled = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
    }
}
