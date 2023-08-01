using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public enum UnitTeam { teamA, teamB }
    public enum UnitType { warrior, archer, cavalry, monster }
    public enum AI_State_Tactic { attack, hold_n_attack, guard, shoot, hold_n_shoot, shoot_n_keepDis }
    public enum AI_State_FindTarget { findClosest, findWarrior, findArcher, findCavalry, findMonster, findRearmost }

    public UnitType unitType;
    public UnitTeam unitTeam;
    [Range(10, 75)]
    public int health;
    [Range(1,10)]
    public int armor;
    [Range(2,20 )]
    public int damage;
    [Range(1, 30)]
    public int attackCD;
    [Range(.4f, 3f)]
    public float attackDis;
    [Range(0.5f, 2f)]
    public float moveSpeed;


    [Header("Arrow")]
    public bool isJavelin;
    [Range(5f, 35f)]
    public float shootDis;
    [Range(0.2f, 2f)]
    public float arrowOffset;
    [Range(0.2f, 0.75f)]
    public float arrowSpeed;

    [Header("Charge")]
    [Range(0f, 6f)]
    public float chargeSpeed_Accererate;
    [Range(0,4)]
    public float chargeSpeed_Max;


    [Header("UnitAI")]
    public AI_State_Tactic current_AI_Tactic;
    public AI_State_FindTarget current_AI_Target;
}
