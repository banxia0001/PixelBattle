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

        //if have result
        if (results.Count > 0)
        {
            GameController.isOnUI = true;
            foreach (RaycastResult RR in results)
            {
                //Debug.Log(RR.gameObject.name);
                if (RR.gameObject.tag == "UnitButton")
                {
                    RecruitButton rc = RR.gameObject.transform.parent.GetComponent<RecruitButton>();
                    rc.anim.SetBool("selected", true);
                }
            }
        }
        else GameController.isOnUI = false;
    }

}
