using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackController : MonoBehaviour
{
    public AttackTrigger attackTrigger;

    [Range(0.5f, 4f)]
    public float attackDis;

    [Range(0.5f, 4f)]
    public float offsetDis;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Unit unit;

    public virtual void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        SetUp();
    }

    public virtual bool CheckHitBox()
    {
        List<Unit> units = AI_Melee_FindAllUnit_InHitBox();

        if (units != null && units.Count != 0)
        {
            return true;
        }

        else return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow cube at the transform position
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color32(250,0,0,70);
        Gizmos.DrawCube(new Vector3(0,0,0) + (offsetDis * Vector3.right), new Vector3(attackDis, 1f, 1));
    }
    public virtual void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }

    public virtual void SetUpAttack(int damMin, int damMax, bool causeAOE)
    {
        int ran = Random.Range(0, 2);
        if(ran == 0) anim.SetTrigger("attack");
        if(ran == 1) anim.SetTrigger("attack2");
        attackTrigger.InputData(this, damMin, damMax, causeAOE); 
    }

    public virtual List<Unit> AI_Melee_FindAllUnit_InHitBox()
    {
        Collider[] overlappingItems;
        overlappingItems = Physics.OverlapBox(transform.position + (offsetDis * Vector3.forward), new Vector3(attackDis, 1f, 1), Quaternion.identity, LayerMask.GetMask("Unit"));

        if (overlappingItems == null) return null;

        //Debug.Log(overlappingItems.Length);

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (unit.data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;
        List<Unit> finalList = new List<Unit>();

        foreach (Collider coll in overlappingItems)
        {
            Unit unit = coll.gameObject.GetComponent<Unit>();

            if (unit.data.unitTeam == targetTeam)
            {
                finalList.Add(unit);
            }
        }
        return finalList;
    }
}
