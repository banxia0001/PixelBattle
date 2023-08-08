using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridNode 
{
    public int x;
    public int y;
    public int id;

    public UnitData.AI_State_FindTarget target;
    public UnitData.AI_State_Tactic tactic;
    public UnitData.AI_State_Wait wait;
}
