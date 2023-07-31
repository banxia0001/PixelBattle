using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int dam;
    public float flySpeed;
    public Vector3 arrowDropLocation;
    private UnitData.UnitTeam targetAttackTeam;
    private bool isDead = false;

    void Start()
    {
        
    }

    public void SetUpArror(Vector3 arrowDropLocation, int dam, float flySpeed, UnitData.UnitTeam targetAttackTeam)
    {
        this.arrowDropLocation = arrowDropLocation;
        this.flySpeed = flySpeed;
        this.dam = dam;
        transform.LookAt(arrowDropLocation);
        this.targetAttackTeam = targetAttackTeam;
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead) return;

        float dist = Vector3.Distance(transform.position, arrowDropLocation);
        if (dist < 0.01f)
        {
            StartCoroutine(Death());
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, arrowDropLocation,  flySpeed);
        }
    }

    public IEnumerator Death()
    {
        isDead = true;

        Unit targetUnit = BattleFunction.Find_ClosestUnitInList(BattleFunction.Find_UnitsInRange(targetAttackTeam, 0.9f, this.transform), this.transform);
        if (targetUnit != null)
        {
            Debug.Log("!!");
            BattleFunction.Attack(this.transform, dam, targetUnit);
        }

        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }
}
