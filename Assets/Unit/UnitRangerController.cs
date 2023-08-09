using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangerController : UnitAttackController
{
    private bool canAttack;
    private Transform target;
    public override void Start()
    {
        unit = this.transform.parent.GetComponent<Unit>();
        canAttack = false;
        SetUp();
    }

    public override void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }

    public void SetUpAttack(Transform target, int damMin, int damMax, bool causeAOE)
    {
        unit.AI_LookAt(target);
        anim.SetTrigger("shoot");
        canAttack = true;
        this.target = target;
    }

    public void Update()
    {
        if (canAttack)
        {
            if (target == null) return;

            if (attackTrigger.inAttacking)
            {
                canAttack = false;
                Shoot();
            }

        }
     
    }

    public void Shoot()
    {
        if (target == null) return;

     

        UnitData.UnitTeam targetTeam = UnitData.UnitTeam.teamB;
        if (unit.data.unitTeam == UnitData.UnitTeam.teamB) targetTeam = UnitData.UnitTeam.teamA;

        int damMin = unit.data.damageMin;
        int damMax = unit.data.damageMax;

       
        GameObject Arrow = Instantiate(unit.data.ProjectilePrefab, this.transform.position, Quaternion.identity);
        Projectile ArrowScript = Arrow.GetComponent<Projectile>();

        float offset = unit.data.arrowOffset;
        float dis = Vector3.Distance(this.transform.position, target.transform.position);
        offset = offset * dis / 15;

        Vector3 targetPos = target.transform.position + new Vector3(Random.Range(-offset, offset), 0, Random.Range(-offset, offset));
        ArrowScript.SetUpArror(targetPos, damMin, damMax, unit.data.arrowSpeed, targetTeam, unit.data.isJavelin);
    }
}
