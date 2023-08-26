using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPoint : MonoBehaviour
{
    private Unit unit;
    public Transform eyeL, eyeM, eyeR, eyeRough;
    private void Start()
    {
        unit = this.transform.GetComponent<Unit>();
    }

    [Header("Circles")]
    [Range(0.1f, 3f)]
    public float radius;
    [Range(0.1f, 6f)]
    public float radius_EyeRough;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(250, 0, 0, 70);
        Gizmos.DrawSphere(eyeL.position, radius);
        Gizmos.DrawSphere(eyeR.position, radius);
        Gizmos.DrawSphere(eyeM.position, radius);
        Gizmos.DrawSphere(eyeRough.position, radius_EyeRough);
    }

    public virtual bool CheckHitShpere(float xRate)
    {
        List<Unit> units = BattleFunction.FindEnemyUnit_InSphere(eyeRough.position, xRate * radius_EyeRough, this.unit);

        if (units != null && units.Count != 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public virtual int CheckSphere(string dir)
    {
        List<Unit> units = null;

        if (dir == "Middle")
            units = BattleFunction.FindEnemyUnit_InSphere(eyeM.transform.position, radius,this.unit);

        if (dir == "Right")
            units = BattleFunction.FindEnemyUnit_InSphere(eyeR.transform.position, radius, this.unit);

        if (dir == "Left")
            units = BattleFunction.FindEnemyUnit_InSphere(eyeL.transform.position, radius, this.unit);


        if (units != null && units.Count != 0)
        {
            return units.Count;
        }
        else return 0;
    }

    public void CopyEyesVectors()
    {
        TransformData.eyeL = this.eyeL.localPosition;
        TransformData.eyeM = this.eyeM.localPosition;
        TransformData.eyeR = this.eyeR.localPosition;
        TransformData.eyeRough = this.eyeRough.localPosition;
        TransformData.radius = this.radius;
        TransformData.radius_Rough = this.radius_EyeRough;

    }
    public void PasteEyesVectors()
    {
        this.eyeL.localPosition = TransformData.eyeL;
        this.eyeM.localPosition = TransformData.eyeM;
        this.eyeR.localPosition = TransformData.eyeR;
        this.eyeRough.localPosition = TransformData.eyeRough;
        this.radius = TransformData.radius;
        this.radius_EyeRough = TransformData.radius_Rough;
    }
}
