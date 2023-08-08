using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 localMousePos;
    private Vector3 planeLocalPos;
    private RectTransform targetObject;
    private RectTransform parentRectTransform;
    private RectTransform targetRectTransform;

    void Awake()
    {
        targetObject = this.transform.GetComponent<RectTransform>();
        targetRectTransform = targetObject;
        parentRectTransform = targetObject.parent as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        planeLocalPos = targetRectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localMousePos);
        targetObject.gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        //屏幕点到矩形中的局部点
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - localMousePos;
            targetObject.localPosition = planeLocalPos + offsetToOriginal;
        }
    }
}
