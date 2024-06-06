using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectScript : MonoBehaviour
{
    //public bool isEnable;
    ////private GameController GC;

    //public Unit.UnitTeam selectUnitType;
    //public Camera cam;
    //public LayerMask unit_GrounLayer;

    //public RectTransform boxVisual;

    ////[HideInInspector]
    //public List<Unit> selectedUnits = new List<Unit>();



    //Rect selectionBox;
    //Vector2 startPos, endPos;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //GC = FindObjectOfType<GameController>();
    //    selectedUnits = new List<Unit>();
    //    startPos = Vector2.zero;
    //    endPos = Vector2.zero;
    //    DrawVisual();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (!isEnable) return;

    //    if (GameController.state == GameController.GameState.none || GameController.state == GameController.GameState.gameFrozen)
    //    {
    //        return;
    //    }

    //    if (GameController.state == GameController.GameState.gameActive)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            UnitDeselected();
    //        }
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            startPos = Input.mousePosition;
    //            UnitDeselected();
    //        }

    //        if (Input.GetMouseButton(0))
    //        {
    //            endPos = Input.mousePosition;
    //            DrawVisual();
    //            DrawSelection();
    //        }

    //        if (Input.GetMouseButtonUp(0))
    //        {
    //            startPos = Vector2.zero;
    //            endPos = Vector2.zero;
    //            DrawVisual();
    //            SelectUnit();
    //        }

    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            RaycastHit hit; 
    //            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unit_GrounLayer))
    //            {
    //                List<Unit> unit = BattleFunction.FindAllUnit_InSphere(hit.point, 1.2f, selectUnitType);
    //                Debug.Log("1Count:"+unit.Count);
    //                if (unit != null && unit.Count > 0)
    //                {
    //                    Command_UnitAttack(hit, unit);
    //                }

    //                else
    //                {


    //                }
    //            }
    //        }
    //    }
    //}




    //private void Command_UnitAttack(RaycastHit hit, List<Unit> list)
    //{
    //    GameObject VFX = Instantiate(Resources.Load<GameObject>("VFX/VFX_Attack"), transform.position, Quaternion.identity);
    //    VFX.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
    //    VFX.transform.eulerAngles = new Vector3(90, 0, 0);

    //    if (selectedUnits == null) return;
    //    if (selectedUnits.Count == 0) return;

    //    if (list != null && list.Count != 0)
    //    {
    //        Unit target = list[Random.Range(0, list.Count)];
    //        Debug.Log("1Count:" + list.Count);
    //        Debug.Log("12Count:" + selectedUnits.Count);
    //        foreach (Unit unit in selectedUnits)
    //        {
    //            Debug.Log("2Count:" + target.name);
    //            //unit.UnitAddTargetUnit(target);
    //        }
    //    }
    //}


    //private void DrawVisual()
    //{
    //    Vector2 boxStart = startPos;
    //    Vector2 boxEnd = endPos;
    //    Vector2 boxCenter = (boxStart + boxEnd) / 2;
     
    //    Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

    //    boxVisual.position = boxCenter;
    //    boxVisual.sizeDelta = boxSize;
    //}
    //private void DrawSelection()
    //{
    //    if (Input.mousePosition.x < startPos.x)
    //    {
    //        selectionBox.xMin = Input.mousePosition.x;
    //        selectionBox.xMax = startPos.x;
    //    }

    //    else
    //    {
    //        selectionBox.xMax = Input.mousePosition.x;
    //        selectionBox.xMin = startPos.x;
    //    }

    //    if (Input.mousePosition.y < startPos.y)
    //    {
    //        selectionBox.yMin = Input.mousePosition.y;
    //        selectionBox.yMax = startPos.y;
    //    }

    //    else
    //    {
    //        selectionBox.yMax = Input.mousePosition.y;
    //        selectionBox.yMin = startPos.y;
    //    }
    //    Vector2 boxStart = startPos;
    //    Vector2 boxEnd = endPos;
    //    Vector2 boxCenter = (boxStart + boxEnd) / 2;

    //    Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

    //    boxVisual.position = boxCenter;
    //    boxVisual.sizeDelta = boxSize;
    //}
    //private void SelectUnit()
    //{
    //    GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
    //    List<Unit> unitsSelected = new List<Unit>();

    //    foreach (GameObject unitOB in allUnits)
    //    {
    //        Unit unit = unitOB.GetComponent<Unit>();
    //        if (selectUnitType != unit.unitTeam) continue;

    //        if (selectionBox.Contains(cam.WorldToScreenPoint(unitOB.transform.position)))
    //        {
    //            unitsSelected.Add(unit);
    //        }
    //    }

    //    UnitSelected(unitsSelected);
    //}

    //public void UnitSelected(List<Unit> unitsSelected)
    //{
    //    if (unitsSelected == null) return;
    //    if (unitsSelected.Count == 0) return;

    //    selectedUnits = unitsSelected;

    //    foreach (Unit unit in unitsSelected)
    //    {
    //        //unit.UnitSelect();
    //    }
    //}

    //public void UnitDeselected()
    //{
    //    if (selectedUnits == null) return;s
    //    if (selectedUnits.Count == 0) return;

    //    foreach (Unit unit in selectedUnits)
    //    {
    //        //unit.UnitDeselect();

    //    }

    //    selectedUnits = new List<Unit>();
    //}




    //private void Click()
    //{

    //    RaycastHit hit;
    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
    //    {


    //    }

    //    else
    //    {


    //    }

    //}

}
