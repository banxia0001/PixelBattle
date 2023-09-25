using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    private UnitAIController USC;
    public bool inAttacking;
    public bool isCharging;
    public int damBonus;
    public float knockbackBonus;
    public int weaponCauseNum;

    private bool canAttack;
    private int damMin;
    private int damMax;
    private bool causeAP;


    public void InputData(UnitAIController USC, int damMin, int damMax,  int weaponCauseNum, bool causeAP)
    {
        weaponCauseNum = 1;
        this.canAttack = true;
        this.USC = USC;
        this.weaponCauseNum = weaponCauseNum;
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
                    weaponCauseNum--;

                    float bonus = 0;
                    if (isCharging) bonus = USC.unit.currentAgentSpeed;

                    damMax += (int)bonus;
                    damMin += (int)bonus;

                    int dam = BattleFunction.DamageCalculate(damMin + damBonus, damMax + damBonus, unit, isCharging,false,causeAP, false);

                    BattleFunction.Attack(this.gameObject.transform, dam, unit);

                    if (isCharging)
                    {
                        unit.AddKnockBack(USC.unit.transform, (float)USC.unit.data.knockBackForce + this.knockbackBonus, 0.1f,false);
                    }

                    else
                    {
                        unit.AddKnockBack(USC.unit.transform, (float)USC.unit.data.knockBackForce + this.knockbackBonus, 0.1f,false);
                    }

                    if (weaponCauseNum <= 0)
                    canAttack = false;
                }
            }
        }
    }
}
