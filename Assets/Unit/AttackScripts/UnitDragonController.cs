using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDragonController : UnitAIController
{
    public AttackTrigger attackTrigger2;
    public override void SetUp()
    {
        this.transform.localPosition = new Vector3(0, 0.1f, -0.278f);
        this.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        this.transform.localEulerAngles = new Vector3(90, -90, 0);
        anim = this.GetComponent<Animator>();
        unit = this.transform.parent.GetComponent<Unit>();
    }
}
