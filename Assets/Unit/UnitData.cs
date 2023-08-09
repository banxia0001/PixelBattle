using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public enum UnitListID
    {
        Militia,
        LightInfantry,
        LightArcher,
        JavelinInfantry,
        HeavyInfantry,
        Knight,
        Barbarian,
    }

    public UnitListID ID;
    public enum UnitTeam { teamA, teamB}
    public enum UnitType { infantry, archer, cavalry, monster, artillery }
    public enum AI_State_Wait { advance, hold5s, hold10S, hold15S }
    public enum AI_State_Tactic { attack, guard, shoot, shoot_n_keepDis }
    public enum AI_State_FindTarget { findClosest, findWarrior, findArcher, findCavalry, findMonster, findArtillery }

    public UnitType unitType;
    public UnitTeam unitTeam;
    [Range(10, 75)]
    public int health;
    [Range(1, 10)]
    public int armor;

    public int damageMin;
    public int damageMax;
    public bool weaponCauseAOE;

    [Range(1, 30)]
    public int attackCD;

    [Range(0.5f, 4f)]
    public float moveSpeed;


    [Header("Archer")]
    public GameObject ProjectilePrefab;
    public bool isJavelin;


    [Range(5f, 35f)]
    public float shootDis;
    [Range(0.2f, 2f)]
    public float arrowOffset;
    [Range(0.2f, 0.75f)]
    public float arrowSpeed;

    [Header("Cavalry")]

    [Range(0f, 10f)]
    public float chargeSpeed_Accererate;
    [Range(0, 10)]
    public float chargeSpeed_Max;

    [Header("Traits")]
    public bool antiCharge;


    [Header("UnitAI")]
    public AI_State_Tactic current_AI_Tactic;
    public AI_State_FindTarget current_AI_Target;
    public AI_State_Wait current_AI_Wait;
}
