using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    public UnitPanel unitPanel;
    public Canvas canvas;

    [Header("GraphicRaycaster")]
    public GraphicRaycaster myRaycaster;
    public EventSystem myEventSystem;
    PointerEventData myPointerEventData;

    [Header("StartPanel")]
    public GameObject panel;
    public UnitRecruitList Rlist;

    [Header("Team")]
    public TMP_Text[] G;
    public TMP_Text[] G_Per;
    public TMP_Text[] Pop;

    public BarController goldBar;
    public BarController scoreBar;

    private void Start()
    {
        goldBar.SetValue_Initial(0.5f,1);
        scoreBar.SetValue_Initial(0.5f,1);
    }

    public void GameStart()
    {
        FindObjectOfType<GameController>().GameStart();
        panel.SetActive(false);
    }

    #region UI Updates
    public void UpdateGBar(float ratio)
    {
        goldBar.SetValue(ratio);
    }
    public void UpdateScoreBar(int Score, int ScoreMax)
    {
        float ratio = (float)Score / (float)ScoreMax;
        scoreBar.SetValue(ratio);
    }

    public void Update_GPer(int G_Per, int i)
    {
        this.G_Per[i].text = G_Per.ToString() + "G/S";
    }
    public void Update_G(int G, int i)
    {
        this.G[i].text = G.ToString();
    }
    public void Update_Population(int Pop, int i)
    {
        this.Pop[i].text = Pop.ToString() + "/50";
    }
    #endregion


    #region RayCast
    public void Update()
    {
        CheckUIRaycast();
    }

    public static Vector2 ScreenToRectPos(Vector2 screen_pos, Canvas canvas)
    {
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
        {
            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.gameObject.GetComponent<RectTransform>(), screen_pos, canvas.worldCamera, out anchorPos);
            return anchorPos;
        }
        else
        {
            //Canvas is in Overlay mode
            Vector2 anchorPos = screen_pos - new Vector2(canvas.gameObject.GetComponent<RectTransform>().position.x, canvas.gameObject.GetComponent<RectTransform>().position.y);
            anchorPos = new Vector2(anchorPos.x / canvas.gameObject.GetComponent<RectTransform>().lossyScale.x, anchorPos.y / canvas.gameObject.GetComponent<RectTransform>().lossyScale.y);
            return anchorPos;
        }
    }

    private void CheckUIRaycast()
    {
        myPointerEventData = new PointerEventData(myEventSystem);
        myPointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        myRaycaster.Raycast(myPointerEventData, results);

        Vector3 screenPos = GameUI.ScreenToRectPos(Input.mousePosition, canvas);
        unitPanel.gameObject.SetActive(false);

        //if have result
        if (results.Count > 0)
        {
            GameController.mouseOnUI = true;
            foreach (RaycastResult RR in results)
            {
                //Debug.Log(RR.gameObject.name);
                if (RR.gameObject.tag == "UnitButton")
                {
                    RecruitButton rc = RR.gameObject.transform.parent.GetComponent<RecruitButton>();
                    //rc.anim.SetBool("selected", true);
                    unitPanel.gameObject.SetActive(true);
                    unitPanel.InputUnit(rc.unit);

                    if (Input.GetMouseButtonDown(0))
                    {
                        rc.InputFromButton();
                    }

                }
            }
        }
        else GameController.mouseOnUI = false;
    }
    #endregion
}
