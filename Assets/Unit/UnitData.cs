using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public enum UnitListID
    {
        Militia = 0,
        LightInfantry = 1,
        LightArcher = 2,
        Spearman = 3,
        LightCavalry = 4,
        HeavyCavalry = 5,
        Sobek = 6,
        SpearKnight = 7,
        FootKnight = 8,
    }
 
    public enum UnitType { infantry, archer, cavalry, monster, artillery }
    public enum AI_State_Tactic { attack, guard, shoot, shoot_n_keepDis }
    public enum AI_State_FindTarget 
    { 
        findClosest,
        findClosestWarrior,
        findClosestTarget_InFrontline, 

        findValueableTarget_InDistance,
        findValueableTarget_InFrontline,

        findClosestArcher,
        findClosestCavalry,
        findClosestMonster,
        findClosestArtillery
    }

    public UnitType unitType;
    public UnitData.AI_State_Tactic current_AI_Tactic;
    public UnitData.AI_State_FindTarget current_AI_Target;
    public UnitData.AI_State_FindTarget current_AI_Target_Secondly;
    //public UnitData.AI_State_Wait current_AI_Wait;

    [Range(10, 150)]
    public int health;
    [Range(0, 25)]
    public int armor;

    [Range(1, 30)]
    public int damageMin;
    [Range(2, 30)]
    public int damageMax;
    [Range(1, 30)]
    public int knockBackForce = 1;
    public int weaponAOENum;


    [Range(1, 30)]
    public int attackCD;

    [Range(0.5f, 5f)]
    public float moveSpeed;
    public float moveStopDis;

    [Header("Mass")]
    [Range(0, 10)]
    public int toughness;

    [Range(0, 20)]
    public float mass = 1;

    [Range(0, 1)]
    public float knockBackBonus = 1;

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
    public bool isSpear;
    public bool isAP;
    public bool isShielded;
}
