using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private UnitAIController USC;
    public bool inAttacking;
    public bool isCharging;

    private bool canAttack;
    private bool canAttack_AOE;
    private int damMin;
    private int damMax;

    public void InputData(UnitAIController USC, int damMin, int damMax,  bool canAttack_AOE)
    {
        this.canAttack = true;
        this.USC = USC;
        this.canAttack_AOE = canAttack_AOE;
        this.damMin = damMin;
        this.damMax = damMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canAttack == true && inAttacking == true)
        {
            if (other.tag == "Unit")
            {
                Unit unit = other.gameObject.GetComponent<Unit>();

                if (unit.unitTeam != this.USC.unit.unitTeam)
                {
                    float bonus = 0;
                    if (isCharging) bonus = 2 * USC.unit.currentAgentSpeed;

                    damMax += (int)bonus;
                    damMin += (int)bonus;

                    int dam = BattleFunction.DamageCalculate(damMin, damMax, unit, isCharging,false);

                    BattleFunction.Attack(this.gameObject.transform, dam, unit);

                    if (isCharging)
                    {
                        unit.AddKnockBack(USC.unit.transform, bonus, 0.1f);
                    }

                    else
                    {
                        unit.AddKnockBack(USC.unit.transform, 1f + dam / 4, 0.1f);
                    }

                    if (!canAttack_AOE)
                    canAttack = false;
                }
            }
        }
    }
}
