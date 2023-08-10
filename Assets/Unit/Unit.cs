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
    [Header("UnitAI")]
    public UnitData.AI_State_Tactic current_AI_Tactic;
    public UnitData.AI_State_FindTarget current_AI_Target;
    public UnitData.AI_State_Wait current_AI_Wait;

    public UnitData_Local data_local;
    public UnitData data;
    public int health;
    public int attackCD;


    [Header("Movement")]
    public Unit attackTarget;
    public GameObject unitSprite;

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent agent;

    private UnitAIController UAC;
    private UnitRangerController URC;
    private UnitCavalryController UCC;
    private UnitInfantryController UIC;

    private Rigidbody rb;
    private float chargeSpeed;
    private float knockBackTimer;
    public float currentAgentSpeed;

    [Header("UI")]
    public BarController BC;

    public void PasteDataFromScriptObject()
    {
        this.data = data_local.data;
    }

    void Start()
    {
        UAC = this.transform.GetChild(2).GetComponent<UnitAIController>();
        URC = this.transform.GetChild(2).GetComponent<UnitRangerController>();
        UCC = this.transform.GetChild(2).GetComponent<UnitCavalryController>();
        UIC = this.transform.GetChild(2).GetComponent<UnitInfantryController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        attackTarget = null;

        if (data.unitType == UnitData.UnitType.cavalry) { agent.avoidancePriority = 50; }
        if (data.unitType == UnitData.UnitType.archer) { agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.monster) { agent.avoidancePriority = 20; }
        if (data.unitType == UnitData.UnitType.infantry) { agent.avoidancePriority = 300; }

        InputData();
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
    }


    void FixedUpdate()
    {
        currentAgentSpeed = agent.velocity.magnitude;
     
        knockBackTimer -= 4 * Time.fixedDeltaTime;
        if (knockBackTimer < 0)
        {
            agent.enabled = true;
        } 
    }
    void LateUpdate()
    {
        this.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        this.transform.GetChild(2).transform.localEulerAngles = new Vector3(90, 0, 90);
        unitSprite.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void AI_DecideAction()
    {
        if (health <= 0) return;
        attackCD--;

        //[If target empty]
        if (attackTarget == null)
        {
            AI_FindTarget();
        }
        //[See if there are new unit that are closer]
        else
        {
            float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if (data.unitType == UnitData.UnitType.infantry)
            {
                if (dis > 3f) AI_FindTarget();
            }
            else if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery)
            {
                if (dis > data.shootDis * 1.2f) AI_FindTarget();
            } 
            else if (data.unitType == UnitData.UnitType.cavalry)
            {
                if (dis > 4f) AI_FindTarget();
            }
            else if (data.unitType == UnitData.UnitType.monster)
            {
                if (dis > 3f) AI_FindTarget();
            }
        }

        if (agent.enabled)
        {
            if (data.unitType == UnitData.UnitType.infantry)
            {
                UIC.AI_Warrior_Action();
            }

            if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery)
            {
                URC.AI_RangeUnit_Action();
            }

            if (data.unitType == UnitData.UnitType.cavalry)
            {
                UCC.AI_Cavalry_Action();
            }
        }
    }


    private void AI_FindTarget()
    {
        UnitTeam targetTeam = UnitTeam.teamB;
        if (unitTeam == UnitTeam.teamB) targetTeam = UnitTeam.teamA;

        if (current_AI_Target == UnitData.AI_State_FindTarget.findClosest)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this);
        }


        if (current_AI_Target == UnitData.AI_State_FindTarget.findWarrior)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.infantry, this);
        }
        if (current_AI_Target == UnitData.AI_State_FindTarget.findArcher)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.archer, this);
        }
        if (current_AI_Target == UnitData.AI_State_FindTarget.findCavalry)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.cavalry, this);
        }
        if (current_AI_Target == UnitData.AI_State_FindTarget.findMonster)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.monster, this);
        }
        if (current_AI_Target == UnitData.AI_State_FindTarget.findArtillery)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.artillery, this);
        }

        if (attackTarget == null)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this);
        }
    }

    public void AddDamage(int dam)
    {
        health -= dam;
        BC.SetValue(health, data.health);
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
    public void AddKnockBack(Transform attacker, float knockBackForce, float waitTime)
    {
        StartCoroutine(_AddKnockBack(attacker, knockBackForce, waitTime));
    }
    private IEnumerator _AddKnockBack(Transform attacker, float knockBackForce, float waitTime)
    {
       
     
        //Debug.Log(knockBackForce);

        if (knockBackForce > (float)data.toughness)
        {
            agent.enabled = false;
            yield return new WaitForSeconds(waitTime);

            if (attacker != null)
            {
                if (agent != null)
                {
                   
                    Vector3 newVector = this.transform.position - attacker.gameObject.transform.position;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(2f * newVector * knockBackForce, ForceMode.Impulse);
                    knockBackTimer = 0.1f + knockBackForce / 15;
                }
            }
        }
    }









    //public void AI_Charge_Single()
    //{
    //    List<Unit> units = AI_Melee_FindAllUnit_InHitBox(1f,1.33f);
    //    Unit targetUnit = null;
    //    if (units != null && units.Count != 0)
    //    {
    //        targetUnit = AI_Melee_FindTargetUnit_InHitBox(units);
    //    }
    //    if (targetUnit != null)
    //    {
    //        float speed = agent.velocity.magnitude;
    //        int knockBackForce = (int)speed;
    //        //[Attack]
    //        attackCD = data.attackCD + knockBackForce / 2;
    //        StartCoroutine(targetUnit.AddKnockBack(this.transform, speed));
          
    //        BattleFunction.Attack(this.gameObject.transform, data.damageMin + (int)speed * 3, data.damageMax + (int)speed * 3, targetUnit,true);
    //        StartCoroutine(targetUnit.AddKnockBack(this.transform, speed* 1.75f));
    //        chargeSpeed = 0f;
    //    }
    //}

    //public void AI_Charge_Mutiple()
    //{
    //    List<Unit> units = AI_Melee_FindAllUnit_InHitBox(1.33f,1.33f);

    //    if (units != null)
    //        if (units.Count != 0)
    //        {
    //            //if (debug_Unit)
    //            //    Debug.Log(speedInLastFrame);
    //            float speed = .6f * agent.velocity.magnitude;

    //            //[Attack]
    //            attackCD = data.attackCD + (int)speed / 2;

    //            foreach (Unit targetUnit in units)
    //            {
    //                StartCoroutine(targetUnit.AddKnockBack(this.transform, speed));
    //                BattleFunction.Attack(this.gameObject.transform, data.damageMin + (int)speed * 2,data.damageMax+ (int)speed * 2, targetUnit,true);
    //            }
    //            chargeSpeed = 0f;
    //        }    
    //}




















    //ArtilleryAI//ArtilleryAI//ArtilleryAI//
    //ArtilleryAI//ArtilleryAI//ArtilleryAI//
    //ArtilleryAI//ArtilleryAI//ArtilleryAI//
    public void AI_ArtilleryShoot(Transform _targetPos)
    {
        attackCD = data.attackCD;

        Unit.UnitTeam targetTeam = Unit.UnitTeam.teamB;
        if (unitTeam == Unit.UnitTeam.teamB) targetTeam = Unit.UnitTeam.teamA;

        //GameObject Arrow = Instantiate(Resources.Load<GameObject>("VFX/ArrowPrefab"), this.transform.position, Quaternion.identity);
        GameObject Arrow = Instantiate(data.ProjectilePrefab, this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();
        Vector3 targetPos = _targetPos.transform.position + new Vector3(Random.Range(-data.arrowOffset, data.arrowOffset), 0, Random.Range(-data.arrowOffset, data.arrowOffset));
        ArrowScript.SetUpArror(targetPos, data.damageMin,data.damageMax, data.arrowSpeed, targetTeam, data.isJavelin);

        chargeSpeed = 0f;

        AddKnockBack(Arrow.transform, 2f,0.1f);
    }


    //public void AI_Monster_Hit()
    //{
    //    Collider[] overlappingItems;
    //    overlappingItems = Physics.OverlapBox(transform.position
    //        + 1 * Vector3.forward, 3 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Item"));
    //}



    //public void SetUpBone()
    //{ 


    //}

    //private Unit AI_Melee_FindTargetUnit_InHitBox(List<Unit> units)
    //{
    //    if (units == null) if (units.Count == 0) return null;

    //    foreach (Unit unit in units)
    //    {
    //        if (unit == attackTarget)
    //        {
    //            return unit;
    //        }
    //    }
    //    return units[Random.Range(0, units.Count)];
    //}
}
