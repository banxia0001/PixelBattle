using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("Dynamic Stats")]
    public int damBonus;
    public float knockbackBonus;
    public int weaponCauseNum;
    public bool canAttack;

    [Header("Static Statis")]
    [HideInInspector] public bool isCharging;
    private UnitAIController AI;
    private Vector2Int damage;
    private bool causeAP;

    public void InputData(UnitAIController AI, Vector2Int damage, bool causeAP)
    {
        this.AI = AI;
        this.damage = damage;
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
                    weaponCauseNum--;

                    float bonus = damBonus;
                    if (isCharging) bonus += AI.unit.currentAgentSpeed;
                    damage.x += (int)bonus;
                    damage.y += (int)bonus;

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

                    if (weaponCauseNum <= 0)
                    canAttack = false;
                }
            }
        }
    }
}
