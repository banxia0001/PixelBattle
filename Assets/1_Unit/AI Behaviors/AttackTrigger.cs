using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("Dynamic Stats")]
    public int damBonus;
    public float knockbackBonus;
    public int attackNum;
    public bool canAttack;


    [Header("Static Statis")]
    [HideInInspector] public bool isCharging;
    private UnitAIController AI;
    private int damage;
    private bool causeAP;


    public void InputData(UnitAIController AI, int damage, bool causeAP)
    {
        this.AI = AI;
        this.damage = damage;
        if (this.attackNum == 0) this.attackNum = 1;
        this.causeAP = causeAP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canAttack == true)
        {
            if (other.tag == "Unit")
            {
                Unit unit = other.gameObject.GetComponent<Unit>();
                if (unit.unitTeam != this.AI.unit.unitTeam)
                {
                    attackNum--;

                    float bonus = damBonus;
                    if (isCharging) bonus += 2 * AI.unit.currentAgentSpeed;
                    damage += (int)bonus;

                    int dam = BattleFunction.GetDamage(damage, unit, isCharging,false,causeAP, false);
                    BattleFunction.Attack(this.gameObject.transform, dam, unit);

                    //[Knock Back]
                    if (isCharging)
                    {
                        unit.AddKnockBack(AI.unit.transform, (float)AI.unit.data.knockBackForce + this.knockbackBonus, 0.1f,false);
                    }
                    else
                    {
                        unit.AddKnockBack(AI.unit.transform, (float)AI.unit.data.knockBackForce + this.knockbackBonus, 0.1f,false);
                    }

                    if (attackNum <= 0)
                    canAttack = false;
                }
            }
        }
    }
}
