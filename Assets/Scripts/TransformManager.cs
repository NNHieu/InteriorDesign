using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransformManager : MonoBehaviour
{
    // Setting
    public float unitTransform = 0.1f;
    private Vector3 baseDirection;

    // 
        
    private SelectionManager selectionManager;

    // Const
    private Vector3[] vectors = new Vector3[3] { Vector3.right, Vector3.up, Vector3.forward };
    private Coordinate coordinate;


    // State
    public enum OpMode
    {
        None, Translation, Rotation
    }
    private OpMode _operationMode;
    private Ray oldRay;

    private void Awake()
    {
        OperationMode = OpMode.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        coordinate = GameObject.Find("/CoordinateModel").GetComponent<Coordinate>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OperationMode == OpMode.Translation)
            HandleInput();   
    }

    public void init(SelectionManager selectionManager)
    {
        this.selectionManager = selectionManager;
        OperationMode = OpMode.Translation;
        oldRay = selectionManager.ScreenPointToRay();
        if (selectionManager.GetSelected().Count > 0)
            coordinate.bindTo(selectionManager.GetSelected()[0].transform);
    }

    public void stop()
    {
        OperationMode = OpMode.None;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown("x") || Input.GetKeyUp("x"))
        {
            ToggleActiveAxis(0);
        }
        if (Input.GetKeyDown("y") || Input.GetKeyUp("y"))
        {
            ToggleActiveAxis(1);
        }
        if (Input.GetKeyDown("z") || Input.GetKeyUp("z"))
        {
            ToggleActiveAxis(2);
        }
        if (!selectionManager.IsSelectedListEmpty())
        {
            MoveAllSelected();
        }
    }


    void MoveAllSelected()
    {
        if (coordinate.HasActiveAxis())
        {
            List<SelectableScript> listObject = selectionManager.GetSelected();
            Ray ray = selectionManager.ScreenPointToRay();
            Vector3 delta = ray.direction - oldRay.direction;
            oldRay = ray; 
            Vector3 objPos = listObject[listObject.Count - 1].transform.position;
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            
            Vector3 projection = Vector3.zero;
            if (coordinate.SumActive() == 2)
            {
                coordinate.GetProjecttion(objPos, ray, coordinate.GetOnlyNonactive(),ref projection);
                foreach (SelectableScript s in selectionManager.GetSelected())
                {
                    s.GetComponent<Translationable>().HandleTranslation(projection - objPos);
                }
            } else if(coordinate.SumActive() == 1)
            {
                coordinate.GetProjecttion(objPos, ray, coordinate.MaxAbsComponent(Vector3.Scale((objPos - ray.origin), Vector3.one - coordinate.ActiveToVector())),ref projection);
                foreach (SelectableScript s in selectionManager.GetSelected())
                {
                    Debug.Log(Vector3.Scale((projection - objPos), coordinate.ActiveToVector()));
                    s.GetComponent<Translationable>().HandleTranslation( Vector3.Scale((projection - objPos), coordinate.ActiveToVector()));
                }
            }
        }
        
    }

    void ToggleActiveAxis(int id)
    {
        coordinate.ToggleActiveAxis(id);
    }


    // Get Set
    public OpMode OperationMode
    {
        private set
        {
            _operationMode = value;
            Debug.Log("Cur Op: " + value.ToString("G"));
        }
        get
        {
            return _operationMode;
        }
    }
}
