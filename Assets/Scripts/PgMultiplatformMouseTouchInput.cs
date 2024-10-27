using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Tools
{
    public class PgMouseOrTouchInput
    {
        private static PgMouseOrTouchInput _instance;
        public static PgMouseOrTouchInput Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PgMouseOrTouchInput();
                return _instance;                
            }
        }
        private EventSystem currEventSys;

        public PgMouseOrTouchInput(EventSystem _curr)
        {
            currEventSys = _curr;
        }

        public PgMouseOrTouchInput()
        {
            currEventSys = EventSystem.current;
        }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    public bool InputControlDown()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            return true;
        return false;
    }
    public bool InputControlUp()
    {
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
            return true;
        return false;
    }
    public bool InputControl()
    {
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary))
            return true;
        return false;
    }

    public bool IsPointerOverUIObject()
    {
        if (Input.touchCount == 0)
            return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(currEventSys);
        eventDataCurrentPosition.position = new Vector2(InputPosition().x, InputPosition().y);
        List<RaycastResult> results = new List<RaycastResult>();
        currEventSys.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    public Vector3 InputPosition()
    {
        return Input.GetTouch(0).position;
    }
        public GameObject GetClickedObject()
        {
            if (Input.touchCount == 0)
                return null;
            PointerEventData eventDataCurrentPosition = new PointerEventData(currEventSys);
            eventDataCurrentPosition.position = new Vector2(InputPosition().x, InputPosition().y);
            List<RaycastResult> results = new List<RaycastResult>();
            currEventSys.RaycastAll(eventDataCurrentPosition, results);
            return results[0].gameObject;
        }
#else


        public bool InputControlDown()
        {
            return Input.GetMouseButtonDown(0);
        }
        public bool InputControlUp()
        {
            if (Input.GetMouseButtonUp(0))
                return true;
            return false;
        }
        public bool InputControl()
        {
            if (Input.GetMouseButton(0))
                return true;
            return false;
        }
        public bool IsPointerOverUIObject()
        {
            return currEventSys.IsPointerOverGameObject();
        }
        public Vector3 InputPosition()
        {
            return Input.mousePosition;
        }
        public GameObject GetClickedObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(currEventSys);
            eventDataCurrentPosition.position = new Vector2(InputPosition().x, InputPosition().y);
            List<RaycastResult> results = new List<RaycastResult>();
            currEventSys.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0 ? results[0].gameObject : null;
            //return currEventSys.currentSelectedGameObject;
        }
#endif

        public bool InputControlDown(out GameObject _obj)
        {
            bool value = InputControlDown();
            _obj = value ? GetClickedObject() : null;
            return value;

        }
        public bool InputControlUp(out GameObject _obj)
        {
            bool value = InputControlUp();
            _obj = value ? GetClickedObject() : null;
            return value;
        }


        public bool InputControl(bool _checkOverUI)
        {
            return _checkOverUI ? (IsPointerOverUIObject() ? false : InputControl()) : InputControl();
        }
        public bool InputControlDown(bool _checkOverUI)
        {
            return _checkOverUI ? (IsPointerOverUIObject() ? false : InputControlDown()) : InputControlDown();
        }
        public bool InputControlUp(bool _checkOverUI)
        {
            return _checkOverUI ? (IsPointerOverUIObject() ? false : InputControlUp()) : InputControlUp();
        }
        public bool InputPosition(out Vector2 _pos)
        {
            if (InputControl())
            {
                _pos = InputPosition();
                return true;
            }
            _pos = Vector2.zero;
            return false;
        }
    }
}
