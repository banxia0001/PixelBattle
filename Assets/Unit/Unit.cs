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

    private UnitAttackController UAC;
    private UnitRangerController URC;
    private UnitCavalryController UCC;

    private Rigidbody rb;

    private float chargeSpeed;
    private float knockBackTimer;
    public float currentAgentSpeed;
    //public bool triggerUpdatePerTurn;


    [Header("UI")]
    public BarController BC;
    public TMP_Text text;
    public SpriteRenderer render;

    void Start()
    {
        UAC = this.transform.GetChild(2).GetComponent<UnitAttackController>();
        URC = this.transform.GetChild(2).GetComponent<UnitRangerController>();
        UCC = this.transform.GetChild(2).GetComponent<UnitCavalryController>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        attackTarget = null;

        if (data.unitTeam == UnitData.UnitTeam.teamA) text.text = "A";
        if (data.unitTeam == UnitData.UnitTeam.teamB) text.text = "B";
        if (data.unitType == UnitData.UnitType.cavalry) { render.color = Color.blue; agent.avoidancePriority = 100; }
        if (data.unitType == UnitData.UnitType.archer) { render.color = Color.green; agent.avoidancePriority = 10; }
        if (data.unitType == UnitData.UnitType.monster) { render.color = Color.red; agent.avoidancePriority = 200; }
        if (data.unitType == UnitData.UnitType.infantry) { render.color = Color.white; agent.avoidancePriority = 30; }


        InputData(data);
    }

    public void InputData(UnitData data)
    {
        this.data = data;
        health = data.health;
        attackCD = 0;
        BC.SetValue_Initial(health, data.health);
        //canMeleeAttack = false;

        agent.speed = data.moveSpeed;

        if (data.unitTeam == UnitData.UnitTeam.teamA)
            BC.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 99, 0, 255);
        else
            BC.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(99, 255, 0, 255);
    }

  
    void FixedUpdate()
    {
        currentAgentSpeed = agent.velocity.magnitude;
        float speed = data.moveSpeed;

        if (data.unitType == UnitData.UnitType.cavalry)
        {
            if (chargeSpeed <= data.chargeSpeed_Max)
                chargeSpeed += Time.deltaTime * data.chargeSpeed_Accererate;

            agent.speed = speed + chargeSpeed;
        }
        if (data.unitType == UnitData.UnitType.archer)
        {
            if (attackCD <= data.attackCD * 0.4f) agent.speed = 0.1f;
            else agent.speed = speed;
        }
        else
        {
            agent.speed = speed;
        }
     


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
        //triggerUpdatePerTurn = true;

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
                if (dis > 10f) AI_FindTarget();
            }
            else if (data.unitType == UnitData.UnitType.monster)
            {
                if (dis > 5f) AI_FindTarget();
            }
        }

        if (agent.enabled)
        {
            if (data.unitType == UnitData.UnitType.infantry)
            {
                AI_Warrior_Action();
            }

            if (data.unitType == UnitData.UnitType.archer || data.unitType == UnitData.UnitType.artillery)
            {
                AI_RangeUnit_Action();
            }

            if (data.unitType == UnitData.UnitType.cavalry)
            {
                AI_Cavalry_Action();
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
            attackTarget = AIFunctions.AI_Find_ClosestUnit(targetTeam, UnitData.UnitType.infantry, this);
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
        if (data.current_AI_Target == UnitData.AI_State_FindTarget.findArtillery)
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
        yield return new WaitForSeconds(0.33f);
        Destroy(this.gameObject);
    }

    public IEnumerator AddKnockBack(Transform attacker, float knockBackForce)
    {
        yield return new WaitForSeconds(0.01f);
        if (attacker != null)
        {
            if (agent != null)
            {
                agent.enabled = false;
                Vector3 newVector = this.transform.position - attacker.gameObject.transform.position;
                rb.velocity = Vector3.zero;
                rb.AddForce(4f * newVector * knockBackForce, ForceMode.Impulse);
                knockBackTimer = 0.1f + knockBackForce / 15;
            }
        }
    }

    public IEnumerator AddKnockBack_Delay(Transform attacker, float knockBackForce)
    {
        yield return new WaitForSeconds(0.1f);
        if (attacker != null)
        {
            agent.enabled = false;
            Vector3 newVector = this.transform.position - attacker.gameObject.transform.position;
            rb.velocity = Vector3.zero;
            rb.AddForce(4f * newVector * knockBackForce, ForceMode.Impulse);
            knockBackTimer = knockBackForce / 3;
        }
    }

    private void AI_Stay(bool minorMove)
    {
        //Debug.Log("Stay");
        if (agent.enabled)
        {
            if (minorMove)
                agent.SetDestination(this.transform.position + new Vector3(Random.Range(-0.13f, 0.13f), 0, Random.Range(-0.13f, 0.13f)));

            else
                agent.SetDestination(this.transform.position);
        }
            
    }
    private void AI_MoveToward(Transform trans)
    {
        if(agent.enabled)
        agent.SetDestination(trans.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
    }
    



   




    private bool CheckHoldStage()
    {
        int stateNum = (int)GameController.waitState;
      
        if (this.data.current_AI_Wait == UnitData.AI_State_Wait.advance) return true;

        if (this.data.current_AI_Wait == UnitData.AI_State_Wait.hold5s && stateNum >= 1) return true;

        if (this.data.current_AI_Wait == UnitData.AI_State_Wait.hold10S && stateNum >= 2) return true;

        if (this.data.current_AI_Wait == UnitData.AI_State_Wait.hold15S && stateNum >= 3) return true;

        //Debug.Log(stateNum);

        return false;
    }


   


    /// <summary>
    //WarriorAI//WarriorAI//WarriorAI//
    //WarriorAI//WarriorAI//WarriorAI//
    //WarriorAI//WarriorAI//WarriorAI//
    /// </summary>
    private void AI_Warrior_Action()
    {
        agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
        bool canAttack = false;
        if (dis < UAC.attackDis * 2f)
        {
            //triggerUpdatePerTurn = false;
            canAttack = UAC.CheckHitBox();
        }

        //[Set Attack]
        if (attackCD <= 0 && canAttack)
        {
            attackCD = data.attackCD + Random.Range(-1,1);
            UAC.SetUpAttack(data.damageMin, data.damageMax, false);
            chargeSpeed = 0f;
            AI_Stay(true);
        }

        if (!CheckHoldStage()) { AI_Stay(true); return; }

        //[Find Enemy]
        if (attackCD != 0)
        {
            //if (data.current_AI_Tactic == UnitData.AI_State_Tactic.hold_n_attack)
            //{
            //    if (GameController.holdStage) AI_Stay();

            //    else AI_MoveToward(attackTarget.gameObject.transform);
            //}
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                AI_MoveToward(attackTarget.gameObject.transform);
            }
        }
    }






    /// <summary>
    //CavalryAI//CavalryAI//CavalryAI//
    //CavalryAI//CavalryAI//CavalryAI//
    //CavalryAI//CavalryAI//CavalryAI//
    /// </summary>
    private void AI_Cavalry_Action()
    {
        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        if (!CheckHoldStage()) { AI_Stay(true); return; }

        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);
        bool canAttack = false;
        if (attackCD <= data.attackCD * 0.75f && agent.velocity.magnitude > 2f) canAttack = true;
        bool canAttack_InHitBox = false;

        if (dis < UAC.attackDis * 1f)
        {
            canAttack_InHitBox = UAC.CheckHitBox();
        }

        //[Set Attack]
        if (attackCD <= 0 && canAttack && canAttack_InHitBox)
        {
            agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
            attackCD = data.attackCD + Random.Range(-1, 1);
            if (data.weaponCauseAOE) UCC.SetUpAttack(data.damageMin, data.damageMax, true);
            else UCC.SetUpAttack(data.damageMin, data.damageMax, false);
        }

        //[Find Enemy]
        else
        {
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.attack)
            {
                //[Freeze after attack]
                if (attackCD > data.attackCD * 0.35f)
                {
                    agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                    AI_MoveForward();
                }

                else
                {
                    agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                    AI_MoveTowardWithoutStopping(attackTarget.transform);
                }
                    
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
            if (dis > 10f)
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
                agent.SetDestination(trans.position);
            }

            else
            {
                agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
                agent.SetDestination(this.transform.position + transform.forward * 6f);
            }
        }
    }













    /// <summary>
    //ArcherAI//ArcherAI//ArcherAI//
    //ArcherAI//ArcherAI//ArcherAI//
    //ArcherAI//ArcherAI//ArcherAI//
    /// </summary>
    private void AI_RangeUnit_Action()
    {
        agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
        //[Stay]
        if (attackTarget == null)
        {
            AI_Stay(true);
            return;
        }

        //[Attack]
        float dis = Vector3.Distance(this.transform.position, attackTarget.transform.position);

        if (dis < data.shootDis && attackCD <= 0)
        {
            if (data.unitType == UnitData.UnitType.archer)
            {
                attackCD = data.attackCD + Random.Range(-2,2);
                URC.SetUpAttack(attackTarget.transform, data.damageMin, data.damageMax, false);
                AI_Stay(false);
                chargeSpeed = 0;
            }
               

            //if (data.unitType == UnitData.UnitType.artillery)
            //    AI_ArtilleryShoot(attackTarget.transform);
        }

        if (!CheckHoldStage()) { AI_Stay(false);  return; }

        //[Find Enemy]
        else
        {
            if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot_n_keepDis)
            {
                bool shouldFlee = AI_CheckIfShouldFlee();

                if (dis < data.shootDis * 0.7f) AI_Stay(false);

                else if (dis < data.shootDis * 0.35f || shouldFlee) AI_Flee();

                else AI_MoveToward(attackTarget.transform);
            }


            else if (data.current_AI_Tactic == UnitData.AI_State_Tactic.shoot)
            {
                if (dis < data.shootDis * 0.7f) AI_Stay(false);

                else if (dis < data.shootDis * 0.35f) AI_Flee();

                else AI_MoveToward(attackTarget.gameObject.transform);
            }
        }
    }

    public void AI_LookAt(Transform pos)
    {
        agent.enabled = false;
       
        transform.LookAt(pos, Vector3.up);
        agent.enabled = true;
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

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        //GameObject Arrow = Instantiate(Resources.Load<GameObject>("VFX/ArrowPrefab"), this.transform.position, Quaternion.identity);
        GameObject Arrow = Instantiate(data.ProjectilePrefab, this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();
        Vector3 targetPos = _targetPos.transform.position + new Vector3(Random.Range(-data.arrowOffset, data.arrowOffset), 0, Random.Range(-data.arrowOffset, data.arrowOffset));
        ArrowScript.SetUpArror(targetPos, data.damageMin,data.damageMax, data.arrowSpeed, targetTeam, data.isJavelin);

        chargeSpeed = 0f;

        StartCoroutine(AddKnockBack(Arrow.transform, 2f));
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
