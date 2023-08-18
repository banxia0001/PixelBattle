using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    [HideInInspector]
    public TeamController team;
    public int recruitNum;

    public void Start()
    {
        team = gameObject.GetComponent<TeamController>();
        recruitNum = 0;
    }
    public void Action()
    {
        if (!team.isControl_By_AIPlayer) return;

        for (int i = 0; i < 5; i++)
        {
            RecruitButton RB = team.buttons[i];
            Debug.Log(recruitNum);
            if (RB.buttonState == RecruitButton.ButtonState.UnitReady)
            {
                RB.InputFromButton();
            }

            if (i == recruitNum)
            {
                if (RB.buttonState == RecruitButton.ButtonState.TrainingUnit) continue;

                if (RB.buttonState == RecruitButton.ButtonState.WaitingForRecruit)
                {
                    if (team.G >= RB.unit.Gcost)
                    {
                        //int num = Random.Range(0, 3);
                        //if (num != 2) 
                        recruitNum++;
                        if (recruitNum == 4 || recruitNum == 5) recruitNum = 0;
                        RB.InputFromButton();
                    }
                }
            }
        }

    }

}

    //    if (team.P < 30)
    //    {
    //        for (int i = 0; i < 5; i++)
    //        {
    //            RecruitButton RB = team.buttons[i];
    //            if (RB.buttonState == RecruitButton.ButtonState.TrainingUnit) continue;


    //            if (RB.buttonState == RecruitButton.ButtonState.WaitingForRecruit)
    //            {
    //                if (team.G >= RB.unit.Gcost)
    //                {
    //                    int num = Random.Range(0, 3);
    //                    if(num != 2) RB.InputFromButton();
    //                }
    //            }

    //            if (RB.buttonState == RecruitButton.ButtonState.UnitReady)
    //            {
    //                RB.InputFromButton();
    //            }
    //        }
    //    }

    //   else
    //    {
    //        for (int i = 4; i >= 0; i--)
    //        {
    //            RecruitButton RB = team.buttons[i];
    //            if (RB.buttonState == RecruitButton.ButtonState.TrainingUnit) continue;


    //            if (RB.buttonState == RecruitButton.ButtonState.WaitingForRecruit)
    //            {
    //                if (team.G >= RB.unit.Gcost)
    //                {
    //                    int num = Random.Range(0, 3);
    //                    if (num != 2) RB.InputFromButton();
    //                }
    //            }

    //            if (RB.buttonState == RecruitButton.ButtonState.UnitReady)
    //            {
    //                RB.InputFromButton();
    //            }
    //        }
    //    }

    //}
