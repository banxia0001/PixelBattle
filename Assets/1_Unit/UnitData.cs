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

    [Header("AI Logic")]
    public UnitType unitType;
    public UnitData.AI_State_Tactic current_AI_Tactic;
    public UnitData.AI_State_FindTarget current_AI_Target;
    public UnitData.AI_State_FindTarget current_AI_Target_Secondly;

    [Header("Stats")]
    [Range(50, 2000)] public int health;
    [Range(0, 99)] public int protection;

    [Header("Weapon")]
    [Range(10, 100)] public int damage;
    [Range(1, 15)] public int attackCD;
    [Range(0, 30)] public float knockBackForce = 1;

    [Header("Movement")]
    [Range(0.5f, 5f)] public float moveSpeed;
    [Range(0.5f, 5f)] public float moveStopDis;

    [Header("Mass")]
    [Range(0, 10)] public int toughness;
    [Range(0, 20)] public float mass = 1;
    [Range(0, 1)] public float knockBackDecrease = 1;

    [Header("Archer")]
    public GameObject ProjectilePrefab;
    public bool isJavelin;
    [Range(5f, 35f)] public float shootDis;
    [Range(0.2f, 2f)] public float arrowOffset;
    [Range(0.2f, 0.75f)] public float arrowSpeed;

    [Header("Cavalry")]
    [Range(0f, 10f)] public float chargeSpeed_Accererate;
    [Range(0, 10)] public float chargeSpeed_Max;

    [Header("Traits")]
    public bool isSpear;
    public bool isAP;
    public bool isShielded;

    public  UnitData(UnitData data)
    {
        this.unitType = data.unitType;
        this.current_AI_Tactic = data.current_AI_Tactic;
        this.current_AI_Target_Secondly = data.current_AI_Target_Secondly;
        this.current_AI_Target = data.current_AI_Target;


        this.health = data.health;
        this.protection = data.protection;


        this.damage = data.damage;
        this.attackCD = data.attackCD;
        this.knockBackForce = data.knockBackForce;


        this.moveSpeed = data.moveSpeed;
        this.moveStopDis = data.moveStopDis;


        this.toughness = data.toughness;
        this.mass = data.mass;
        this.knockBackDecrease = data.knockBackDecrease;


        this.ProjectilePrefab = data.ProjectilePrefab;
        this.isJavelin = data.isJavelin;
        this.shootDis = data.shootDis;
        this.arrowOffset = data.arrowOffset;
        this.arrowSpeed = data.arrowSpeed;

        this.chargeSpeed_Accererate = data.chargeSpeed_Accererate;
        this.chargeSpeed_Max = data.chargeSpeed_Max;


        this.isSpear = data.isSpear;
        this.isAP = data.isAP;
        this.isShielded = data.isShielded;
    }
}
