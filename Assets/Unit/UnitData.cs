using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public enum UnitTeam { teamA, teamB }
    public enum UnitType { warrior, archer, cavalry, monster }
    public enum AI_State_Tactic { attack, hold_n_attack, shoot, shoot_n_advance, shoot_n_flee }
    public enum AI_State_FindTarget { findClosest,findWarrior, findArcher, findCavalry, findMonster, findRearmost }

    public UnitType unitType;
    public UnitTeam unitTeam;
    public int health;
    public int armor;
    public int damage;  
    public int attackCD;
    public float attackDis;
    public float moveSpeed;

    


    [Header("UnitAI")]
    public AI_State_Tactic current_AI_Tactic;
    public AI_State_FindTarget current_AI_Target;
}
