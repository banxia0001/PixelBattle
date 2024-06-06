using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptaleObjects/Trait")]
public class Trait : ScriptableObject
{
    public Sprite icon;
    public string traitName;
    [TextArea (9,3)]
    public string traitDetail;
}
