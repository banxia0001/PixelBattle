using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCavalryController : UnitAttackController
{
    private bool canAttack;

    public override void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }



    public void Update()
    {
        if (unit.currentAgentSpeed > 1)
        {
            anim.SetBool("move", true);
        }
        else anim.SetBool("move", false);
    }

    public override void SetUpAttack(int damMin, int damMax, bool causeAOE)
    {
        canAttack = true;
        attackTrigger.InputData(this, damMin, damMax, causeAOE);
    }

    //If cav hit other unit, trigger attack.
    private void OnTriggerEnter(Collider other)
    {
        if (canAttack == true)
        {
            //When collider hit, firstly shock wave the nearby units.
            if (other.tag == "Unit")
            {
                Unit unit = other.gameObject.GetComponent<Unit>();

                if (unit.data.unitTeam != this.unit.data.unitTeam)
                {
                    AI_Charge_Mutiple();

                    //Then unit attack.
                    anim.SetTrigger("attack");
                    canAttack = false;
                }
            }
        }
    }


    public void AI_Charge_Mutiple()
    {
        List<Unit> units = AI_Melee_FindAllUnit_InHitBox_Cav();

        if (units != null)
            if (units.Count != 0)
            {

                float minDam = .6f * unit.currentAgentSpeed;
                float maxDam = 1.2f * unit.currentAgentSpeed;

                foreach (Unit targetUnit in units)
                {
                    Debug.Log("11");
                    StartCoroutine(targetUnit.AddKnockBack_Delay(this.transform, minDam));
                    int dam = BattleFunction.DamageCalculate((int)minDam, (int)maxDam, targetUnit,true, false);
                    BattleFunction.Attack(this.transform, dam, targetUnit);
                }
            }
    }

    public  List<Unit> AI_Melee_FindAllUnit_InHitBox_Cav()
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapBox(transform.position + (offsetDis * Vector3.forward), new Vector3(attackDis, attackDis, attackDis), Quaternion.identity, LayerMask.GetMask("Unit"));

        if (overlappingItems == null) return null;

        //Debug.Log(overlappingItems.Length);

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (unit.data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;
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
}
