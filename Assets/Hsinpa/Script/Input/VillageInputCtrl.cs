using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using JAM.Village;

public class VillageInputCtrl
{
    public delegate void OnVillageObjectClickEvent(Village village);
    private Camera _camera;

    private Collider2D[] cacheCollder = new Collider2D[10];
    private List<RaycastResult> UIObjectHits = new List<RaycastResult>();

    private enum InputState
    {
        Normal,
        Dragging,
        Pause
    }

    private InputState inputState;
    private Vector2 lastMousePos = Vector2.zero;
    private Vector2 currentMousePos = Vector2.zero;
    private Vector2 mouseDownPos = Vector2.zero;
    public OnVillageObjectClickEvent OnVillageObjectClick;


    public VillageInputCtrl(Camera camera)
    {
        _camera = camera;
    }

    public void OnUpdate()
    {
        if (_camera == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        currentMousePos = MouseToWorldPos(mousePosition);

        Vector2 velocity = currentMousePos - lastMousePos;

        HandleMouseClick(currentMousePos, mousePosition, velocity);

        if (Input.GetMouseButtonUp(0))
            inputState = InputState.Normal;

        lastMousePos = currentMousePos;
    }

    private void CheckRaycastConstraint(Vector2 worldPos, Vector3 mousePosition)
    {
        PointerEventData cursor = new PointerEventData(EventSystem.current);
        cursor.position = mousePosition;

        EventSystem.current.RaycastAll(cursor, UIObjectHits);

        //UI Object has higher priority
        if (UIObjectHits.Count > 0)
        {
            inputState = InputState.Pause;
        }
    }

    private void CastRaycast(Vector2 worldPos, Vector3 mousePosition)
    {
        int result = Physics2D.OverlapBoxNonAlloc(worldPos, Vector2.one, 0, cacheCollder);

        if (result > 0) {

            if (OnVillageObjectClick != null)
                OnVillageObjectClick(cacheCollder[0].GetComponent<Village>());
        }
            //InGameApp.Instance.Notify(EventFlag.InGame.OnClickObject, cacheCollder[0]);
        //else
        //    InGameApp.Instance.Notify(EventFlag.InGame.OnClickEmpty);
    }

    private void HandleMouseClick(Vector2 worldPosition, Vector3 mousePosition, Vector2 velocity)
    {
        if (inputState == InputState.Normal)
        {

            float mouseDistance = Vector2.Distance(mouseDownPos, lastMousePos);

            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = worldPosition;
                CheckRaycastConstraint(worldPosition, mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                inputState = InputState.Normal;

                CastRaycast(worldPosition, mousePosition);
            }
        }
    }

    private Vector2 MouseToWorldPos(Vector2 mousePosition)
    {
        return _camera.ScreenToWorldPoint(mousePosition);
    }
}
