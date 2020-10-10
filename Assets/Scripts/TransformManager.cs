using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransformManager : MonoBehaviour
{
    // Setting

    // 
    private SelectionManager selectionManager;

    // Const
    private Vector3[] vectors = new Vector3[3] { Vector3.right, Vector3.up, Vector3.forward };
    private Coordinate coordinate;


    // State
    public enum State { STOP, IDLE, RUNNING }
    public enum Type { TRANSLATION, ROTATION }

    private Ray oldRay;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = State.STOP;
        coordinate = GameObject.Find("/CoordinateModel").GetComponent<Coordinate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState != State.STOP)
        {
            HandleInput();
            if (CurrentState == State.RUNNING && !selectionManager.IsSelectedListEmpty() && coordinate.HasActiveAxis())
            {
                Ray ray = selectionManager.ScreenPointToRay();
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                if (CurrentType == Type.TRANSLATION) MoveAllSelected(ray, oldRay);
                else if (CurrentType == Type.ROTATION)
                {
                    RotationAllSelected(ray, oldRay);
                }
                oldRay = ray;
            }
        }
    }

    public void init(SelectionManager selectionManager)
    {
        this.selectionManager = selectionManager;
        CurrentState = State.IDLE;
        oldRay = selectionManager.ScreenPointToRay();
        // if (selectionManager.GetSelected().Count > 0)
        //     coordinate.bindTo(selectionManager.GetSelected()[0].transform);
    }

    public void stop()
    {
        CurrentState = State.STOP;
    }

    void HandleKeyToAxis(KeyCode key, int axis)
    {
        if (Input.GetKeyDown(key))
        {
            coordinate.ToggleActiveAxis(axis, true);
        }
        else if (Input.GetKeyUp(key))
        {
            coordinate.ToggleActiveAxis(axis, false);
        }
    }

    void HandleInput()
    {
        HandleKeyToAxis(KeyCode.X, 0);
        HandleKeyToAxis(KeyCode.Y, 1);
        HandleKeyToAxis(KeyCode.Z, 2);
        if (Input.GetKeyDown("r"))
        {
            CurrentType = Type.ROTATION;
        }
        else if (Input.GetKeyDown("t"))
        {
            CurrentType = Type.TRANSLATION;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CurrentState = State.RUNNING;
            PrepairRay();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CurrentState = State.IDLE;
        }
    }

    void PrepairRay()
    {
        oldRay = selectionManager.ScreenPointToRay();
    }


    void MoveAllSelected(Ray currentMouseRay, Ray oldMouseRay)
    {
        List<SelectableScript> listObject = selectionManager.GetSelected();
        Vector3 objPos = listObject[listObject.Count - 1].transform.position;
        Vector3 vectorTransform = Vector3.zero;
        if (coordinate.SumActive() == 2)
        {
            Vector3 rayProjection = coordinate.GetProjecttion(objPos, currentMouseRay, coordinate.GetOnlyNonactive());
            Vector3 oldRayProjection = coordinate.GetProjecttion(objPos, oldMouseRay, coordinate.GetOnlyNonactive());
            // vectorTransform = rayProjection - objPos;
            vectorTransform = rayProjection - oldRayProjection;
        }
        else if (coordinate.SumActive() == 1)
        {
            Vector3 rayProjection = coordinate.GetProjecttion(objPos, currentMouseRay, coordinate.MaxAbsComponentExcept(objPos - currentMouseRay.origin, coordinate.GetOnlyAcitve()));
            Vector3 oldRayProjection = coordinate.GetProjecttion(objPos, oldMouseRay, coordinate.MaxAbsComponentExcept(objPos - currentMouseRay.origin, coordinate.GetOnlyAcitve()));
            vectorTransform = Vector3.Scale(rayProjection - oldRayProjection, coordinate.ActiveToVector());
        }
        foreach (SelectableScript s in listObject)
        {
            s.transform.Translate(vectorTransform, Space.World);
        }
    }

    void RotationAllSelected(Ray currentMouseRay, Ray oldMouseRay)
    {
        List<SelectableScript> listObject = selectionManager.GetSelected();
        Vector3 objPos = listObject[listObject.Count - 1].transform.position;
        Vector3 rayProjection = coordinate.GetProjecttion(objPos, currentMouseRay, coordinate.GetOnlyAcitve());
        Vector3 oldRayProjection = coordinate.GetProjecttion(objPos, oldMouseRay, coordinate.GetOnlyAcitve());
        Vector3 origin = Vector3.zero;
        foreach (SelectableScript s in listObject)
        {
            origin += s.transform.position;
        }
        origin /= listObject.Count;
        float angle = Vector3.SignedAngle(oldRayProjection - origin, rayProjection - origin, coordinate.ActiveToVector());
        foreach (SelectableScript s in listObject)
        {
            s.transform.RotateAround(origin, coordinate.ActiveToVector(), angle);
        }
    }


    public State CurrentState
    {
        get; set;
    }

    private Type _currentType;
    public Type CurrentType
    {
        get
        {
            return _currentType;
        }
        set
        {
            if (value == Type.ROTATION)
            {
                coordinate.MaxActive = Coordinate.Mode.MAX_1_ACTIVE;
            }
            else if (value == Type.TRANSLATION)
            {
                coordinate.MaxActive = Coordinate.Mode.MAX_3_ACTIVE;
            }
            _currentType = value;
            Debug.Log("Cur Op: " + value.ToString("G"));
        }
    }
}
