using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    public bool debug_Unit;
    //public enum AI_State_Combat { normal, beKnockBack, chargeToTarget }
    //public AI_State_Combat state;

    [Header("UnitData")]
    public UnitData data;
    public int health;
    public int attackCD;

    [Header("Movement")]
    public Unit attackTarget;
    public GameObject unitSprite;
    private UnityEngine.AI.NavMeshAgent agent;
    private Rigidbody rb;

    private float chargeSpeed;
    private float knockBackTimer;
    public bool canMeleeAttack;
    public bool triggerUpdatePerTurn;



    [Header("UI")]
    public BarController BC;
    public TMP_Text text;
    public SpriteRenderer render;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        attackTarget = null;

        if (data.unitTeam == UnitData.UnitTeam.teamA) text.text = "A";
        if (data.unitTeam == UnitData.UnitTeam.teamB) text.text = "B";

        if (data.unitType == UnitData.UnitType.cavalry) { render.color = Color.blue; agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.archer) { render.color = Color.green; agent.avoidancePriority = 10; }
        if (data.unitType == UnitData.UnitType.monster) { render.color = Color.red; agent.avoidancePriority = 200; }
        if (data.unitType == UnitData.UnitType.warrior) { render.color = Color.white; agent.avoidancePriority = 30; }

        InputData(data);
    }

    public void InputData(UnitData data)
    {
        this.data = data;
        health = data.health;
        attackCD = 0;
        BC.SetValue_Initial(health, data.health);
        canMeleeAttack = false;
    }

  
    void FixedUpdate()
    {

        float speed = data.moveSpeed;

        if (data.unitType == UnitData.UnitType.cavalry)
        {
            if (chargeSpeed <= data.chargeSpeed_Max)
                chargeSpeed += Time.deltaTime * data.chargeSpeed_Accererate;
        }
        if (data.unitType == UnitData.UnitType.archer)
        {
            if (attackCD <= data.attackCD * 0.2f) speed = speed / 2;
        }

        agent.speed = speed + chargeSpeed;
        //agent.angularSpeed = 300 - (60 * chargeSpeed);


        knockBackTimer -= 4 * Time.fixedDeltaTime;
        if (knockBackTimer < 0)
        {
            agent.enabled = true;
            //if (state == AI_State_Combat.beKnockBack)
            //{
            //    agent.enabled = true;
            //    state = AI_State_Combat.normal;
            //}
        } 
    }
    void LateUpdate()
    {
        unitSprite.transform.eulerAngles = new Vector3(0, 0, 0);
        //if (debug_Unit)
        //    Debug.Log(agent.velocity.magnitude);
    }

    public void AI_DecideAction()
    {
        agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        triggerUpdatePerTurn = true;

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
            if (data.unitType == UnitData.UnitType.warrior)
            {
                if (dis > data.attackDis * 2f) AI_FindTarget();
            }
            else if (data.unitType == UnitData.UnitType.archer)
            {
                if (dis > data.shootDis * 1.2f) AI_FindTarget();
            } 
            else if (data.unitType == UnitData.UnitType.cavalry)
            {
                if (dis > data.attackDis * 5f) AI_FindTarget();
            }
            else if (data.unitType == UnitData.UnitType.monster)
            {
                if (dis > data.attackDis * 1.5f) AI_FindTarget();
            }
        }

        //[Check Unit Action]

        //if (data.unitType == UnitData.UnitType.warrior)
        //{
        //    AI_Warrior_Action();
        //}

        //if (data.unitType == UnitData.UnitType.archer)
        //{
        //    AI_Archer_Action();
        //}

        //if (data.unitType == UnitData.UnitType.cavalry)
        //{
        //    AI_CavalryAttack();
        //}

        if (agent.enabled)
        {
            if (data.unitType == UnitData.UnitType.warrior)
            {
                AI_Warrior_Action();
            }

            if (data.unitType == UnitData.UnitType.archer)
            {
                AI_Archer_Action();
            }

            if (data.unitType == UnitData.UnitType.cavalry)
            {
                AI_CavalryAttack();
            }
        }
    }






    private void AI_FindTarget()
    {
        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findClosest)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findWarrior)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.warrior, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findArcher)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.archer, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findCavalry)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.cavalry, this);
        }
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findMonster)
        {
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.monster, this);
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
        yield return new WaitForSeconds(0.33f);
        Destroy(this.gameObject);
    }

    public IEnumerator AddKnockBack(Transform attacker, float knockBackForce)
    {
        yield return new WaitForSeconds(0.01f);
        agent.enabled = false;
        Vector3 newVector = this.transform.position - attacker.gameObject.transform.position;
        rb.velocity = Vector3.zero;
        rb.AddForce(4f * newVector * knockBackForce, ForceMode.Impulse);
        knockBackTimer = knockBackForce / 3;
    }

    private void AI_Stay()
    {
        //Debug.Log("Stay");
        if (agent.enabled)
            agent.SetDestination(this.transform.position + new Vector3(Random.Range(-0.13f, 0.13f), 0, Random.Range(-0.13f, 0.13f)));
    }
    private void AI_MoveToward(Transform trans)
    {
        if(agent.enabled)
        agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    

    private Unit AI_Melee_FindTargetUnit_InHitBox(List<Unit> units)
    {
        if (units == null) if (units.Count == 0) return null;

        foreach (Unit unit in units)
        {
            if (unit == attackTarget)
            {
                return unit;
            }
        }
        return units[Random.Range(0, units.Count)];
    }

    private List<Unit> AI_Melee_FindAllUnit_InHitBox(float ratio, float forwardExtend)
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapSphere(transform.position + (data.attackDis * Vector3.forward) * forwardExtend, data.attackDis * ratio, LayerMask.GetMask("Unit"));

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;
        List<Unit> finalList = new List<Unit>();

        foreach (Collider coll in overlappingItems)
        {
            Unit unit = coll.gameObject.GetComponent<Unit>();
            
            if (unit.data.unitTeam == targetTeam)
            {
                finalList.Add(unit);
            }
        }
        return finalList;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(data.attackDis / 4 * Vector3.forward, data.attackDis);
    }









    /// <summary>
    //WarriorAI//WarriorAI//WarriorAI//
    //WarriorAI//WarriorAI//WarriorAI//
    //WarriorAI//WarriorAI//WarriorAI//
    /// </summary>
    private void AI_Warrior_Action()
    {
        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay();
            return;
        }

        //[Attack]
        if (attackCD <= 0 && canMeleeAttack)
        {
            AI_WarriorAttack();
            canMeleeAttack = false;
        }

        //[Find Enemy]
       if(attackCD != 0)
        {
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.hold_n_attack)
            {
                if (GameController.holdStage) AI_Stay();

                else AI_MoveToward(attackTarget.gameObject.transform);
            }
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                AI_MoveToward(attackTarget.gameObject.transform);
            }
        }
    }
    public void AI_WarriorAttack()
    {
        Unit attackUnit = null;
        //[Check Attack Range]
        List<Unit> units = AI_Melee_FindAllUnit_InHitBox(1f, 0.2f);
        //Debug.Log(units);
        if (units != null && units.Count != 0) attackUnit = AI_Melee_FindTargetUnit_InHitBox(units);
        if (attackUnit != null)
        {
            //Debug.Log(units.Count);
            //[Attack]
            attackCD = data.attackCD;
            BattleFunction.Attack(this.gameObject.transform, data.damage, attackUnit);
            chargeSpeed = 0f;
            //[Set Pos]
            AI_Stay();
        }
    }













    /// <summary>
    //ArcherAI//ArcherAI//ArcherAI//
    //ArcherAI//ArcherAI//ArcherAI//
    //ArcherAI//ArcherAI//ArcherAI//
    /// </summary>
    private void AI_Archer_Action()
    {
        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay();
            return;
        }

        //[Attack]
        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);

        if (dis < data.shootDis && attackCD <= 0) AI_Shoot();

        //[Find Enemy]
        else
        {
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot_n_keepDis)
            {
                bool shouldFlee = AI_CheckIfShouldFlee();
                if (dis < data.shootDis * 0.5f || shouldFlee)
                    AI_Flee();

                else AI_MoveToward(attackTarget.transform);
            }

            else if (data.current_AI_Tactic == UnitData.AI_State_Tactic.hold_n_shoot)
            {
                if (GameController.holdStage) AI_Stay();

                else AI_MoveToward(attackTarget.gameObject.transform);
            }

            else if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot)
            {
                AI_MoveToward(attackTarget.gameObject.transform);
            }
        }

    }
    public void AI_Shoot()
    {
        //[Shoot Arrow]
        attackCD = data.attackCD;

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        GameObject Arrow = Instantiate(Resources.Load<GameObject>("VFX/ArrowPrefab"), this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();
        Vector3 targetPos = attackTarget.transform.position + new Vector3(Random.Range(-data.arrowOffset, data.arrowOffset), 0, Random.Range(-data.arrowOffset, data.arrowOffset));
        ArrowScript.SetUpArror(targetPos, data.damage, data.arrowSpeed, targetTeam);
        chargeSpeed = 0f;

        //[Set Pos]
        AI_MoveToward(attackTarget.transform);
    }
    private void AI_Flee()
    {
        if (agent.enabled)
        { 
         float xValue = 10f;
        if(data.unitTeam == UnitData.UnitTeam.teamB) xValue = 10f;
        if(data.unitTeam == UnitData.UnitTeam.teamA) xValue = -10f;
        Vector3 newPos = this.transform.position + new Vector3(xValue + Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        agent.SetDestination(newPos);
        }           
    }

    private bool AI_CheckIfShouldFlee()
    {
        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        List<Unit> unitList = BattleFunction.Find_UnitsInRange(targetTeam, data.shootDis * 0.2f, this.transform);
        if (unitList != null)
        {
            if (unitList.Count != 0)
            {
                attackTarget = BattleFunction.Find_ClosestUnitInList(unitList, this.transform);
                //Debug.Log("moveTarget:" + attackTarget.name);
                return true;
            }
            else return false;
        }
        else return false;
    }


















    /// <summary>
    //CavalryAI//CavalryAI//CavalryAI//
    //CavalryAI//CavalryAI//CavalryAI//
    //CavalryAI//CavalryAI//CavalryAI//
    /// </summary>
    private void AI_CavalryAttack()
    {
        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay();
            return;
        }

        //[Attack]
        if (attackCD <= 0 && canMeleeAttack)
        {
            canMeleeAttack = false;
            AI_Charge_Mutiple();
            //if (agent.velocity.magnitude > 1.12f)
            //{
            //    AI_Charge_Mutiple();
            //}

            //else
            //{
            //    AI_Charge_Mutiple();
            //}
        }

        //[Freeze after attack]
        if (attackCD >= data.attackCD * 0.2f)
        {
            AI_MoveForward();
        }

        //[Find Enemy]
        else
        {
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.hold_n_attack)
            {
                if (GameController.holdStage) AI_Stay();

                else AI_MoveTowardWithoutStopping(attackTarget.gameObject.transform);
            }
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                AI_MoveTowardWithoutStopping(attackTarget.gameObject.transform);
            }
        }
    }

    private void AI_MoveForward()
    {
        //Debug.Log("AI_MoveForward");
        if (agent.enabled)
            agent.SetDestination(this.transform.position + transform.forward * 3f);
    }

    private void AI_MoveTowardWithoutStopping(Transform trans)
    {
        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
        if (agent.enabled)
        {
            if (dis < 5f)
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
            }

            if (dis < 1f)
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                agent.SetDestination(this.transform.position + transform.forward * 6f);
            }

            else agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
        }
            
        //Vector3 dir = BattleFunction.DistantPoint(this.transform.position, trans.position);
        //agent.SetDestination(dir + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    public void AI_Charge_Simgle()
    {
        List<Unit> units = AI_Melee_FindAllUnit_InHitBox(1f,1f);
        Unit targetUnit = null;
        if (units != null && units.Count != 0)
        {
            targetUnit = AI_Melee_FindTargetUnit_InHitBox(units);
        }
        if (targetUnit != null)
        {
            float speed = agent.velocity.magnitude;
            int knockBackForce = (int)speed;
            //[Attack]
            attackCD = data.attackCD + knockBackForce / 2;
            StartCoroutine(targetUnit.AddKnockBack(this.transform, speed));
          
            BattleFunction.Attack(this.gameObject.transform, data.damage + knockBackForce, targetUnit);
            chargeSpeed = 0f;
        }
    }

    public void AI_Charge_Mutiple()
    {
        List<Unit> units = AI_Melee_FindAllUnit_InHitBox(2f,1.2f);

        if (units != null)
            if (units.Count != 0)
            {
                //if (debug_Unit)
                //    Debug.Log(speedInLastFrame);
                float speed = .6f * agent.velocity.magnitude;

                //[Attack]
                attackCD = data.attackCD + (int)speed / 2;

                foreach (Unit targetUnit in units)
                {
                    StartCoroutine(targetUnit.AddKnockBack(this.transform, speed));
                    BattleFunction.Attack(this.gameObject.transform, data.damage + (int)speed * 2, targetUnit);
                }
                chargeSpeed = 0f;
            }    
    }

    public void AI_Monster_Hit()
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapBox(transform.position
            + 1 * Vector3.forward, 3 * Vector3.one, Quaternion.identity, LayerMask.GetMask("Item"));
    }


    private void OnTriggerStay(Collider other)
    {
        if (triggerUpdatePerTurn == true)
        {
            if (other.tag == "Unit")
            {
                Unit unit = other.gameObject.GetComponent<Unit>();
                if (unit.data.unitTeam != this.data.unitTeam)
                {
                    triggerUpdatePerTurn = false;
                    canMeleeAttack = true;
                }
            }
        }
    }

}
