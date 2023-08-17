using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    [HideInInspector]
    public TeamController team;

    public void Start()
    {
        team = gameObject.GetComponent<TeamController>();
    }
    public void Action()
    {
        if (team.P < 30)
        {
            for (int i = 0; i < 5; i++)
            {
                RecruitButton RB = team.buttons[i];
                if (RB.buttonState == RecruitButton.ButtonState.TrainingUnit) continue;


                if (RB.buttonState == RecruitButton.ButtonState.WaitingForRecruit)
                {
                    if (team.G >= RB.unit.Gcost)
                    {
                        int num = Random.Range(0, 3);
                        if(num != 2) RB.InputFromButton();
                    }
                }

                if (RB.buttonState == RecruitButton.ButtonState.UnitReady)
                {
                    RB.InputFromButton();
                }
            }
        }

       else
        {
            for (int i = 4; i >= 0; i--)
            {
                RecruitButton RB = team.buttons[i];
                if (RB.buttonState == RecruitButton.ButtonState.TrainingUnit) continue;


                if (RB.buttonState == RecruitButton.ButtonState.WaitingForRecruit)
                {
                    if (team.G >= RB.unit.Gcost)
                    {
                        int num = Random.Range(0, 3);
                        if (num != 2) RB.InputFromButton();
                    }
                }

                if (RB.buttonState == RecruitButton.ButtonState.UnitReady)
                {
                    RB.InputFromButton();
                }
            }
        }

    }
}
