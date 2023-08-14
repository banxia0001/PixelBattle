using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private UnitAIController USC;
    public bool inAttacking;
    public bool isCharging;
    public int damBonus;

    private bool canAttack;
    private bool canAttack_AOE;
    private int damMin;
    private int damMax;
    private bool causeAP;

    public void InputData(UnitAIController USC, int damMin, int damMax,  bool canAttack_AOE, bool causeAP)
    {
        this.canAttack = true;
        this.USC = USC;
        this.canAttack_AOE = canAttack_AOE;
        this.damMin = damMin;
        this.damMax = damMax;
        this.causeAP = causeAP;
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
                    if (isCharging) bonus = USC.unit.currentAgentSpeed;

                    damMax += (int)bonus;
                    damMin += (int)bonus;

                    int dam = BattleFunction.DamageCalculate(damMin + damBonus, damMax + damBonus, unit, isCharging,false,causeAP);

                    BattleFunction.Attack(this.gameObject.transform, dam, unit);

                    if (isCharging)
                    {
                        unit.AddKnockBack(USC.unit.transform, damMax / 2 + damMin / 2 + bonus/2 + damBonus/2, 0.1f);
                    }

                    else
                    {
                        unit.AddKnockBack(USC.unit.transform, damMax / 2 + damMin / 2 + damBonus/2, 0.1f);
                    }

                    if (!canAttack_AOE)
                    canAttack = false;
                }
            }
        }
    }
}
