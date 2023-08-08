using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraitButtonPanel : MonoBehaviour
{
    public TMP_Text traitName;
    public Image image;

    public Trait trait;

    public void InputTrait(Trait trait)
    {
        if (trait == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        this.trait = trait;

        traitName.text = trait.traitName;
        image.sprite = trait.icon;
    }

}
