using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private UnitAttackController USC;
    public bool inAttacking;
    public bool isCharging;

    private bool canAttack;
    private bool canAttack_AOE;
    private int damMin;
    private int damMax;

    public void InputData(UnitAttackController USC, int damMin, int damMax,  bool canAttack_AOE)
    {
        this.canAttack = true;
        this.USC = USC;
        this.canAttack_AOE = canAttack_AOE;
        this.damMin = damMin;
        this.damMax = damMax;
    }


    //Melee Attack Trigger
    //Melee Attack Trigger
    //Melee Attack Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (canAttack == true && inAttacking == true)
        {
            if (other.tag == "Unit")
            {
                Unit unit = other.gameObject.GetComponent<Unit>();

                if (unit.data.unitTeam != this.USC.unit.data.unitTeam)
                {
                    float bonus = 0;
                    if (isCharging) bonus = 3 * USC.unit.currentAgentSpeed;

                    damMax += (int)bonus;
                    damMin += (int)bonus;

                    int dam = BattleFunction.DamageCalculate(damMin, damMax, unit, isCharging,false);
                    //attack;
                    BattleFunction.Attack(this.gameObject.transform, dam, unit);

                    if (isCharging)
                    {
                        StartCoroutine(unit.AddKnockBack(this.transform,  2f + dam/3));
                    }
                    else StartCoroutine(unit.AddKnockBack(this.transform, 0.5f + dam * 0.2f));

                    if (!canAttack_AOE)
                    canAttack = false;
                }
            }
        }
    }
}
