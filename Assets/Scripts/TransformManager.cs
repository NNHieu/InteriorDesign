using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformManager : MonoBehaviour
{
    // Setting
    public SelectionManager selectionManager;
    public float unitTransform = 0.1f;
    private bool[] activeAxis = new bool[3];
    private Vector3 baseDirection;
    [SerializeField] private Toggle uitoggle_XAxis, uitoggle_YAxis, uitoggle_ZAxis;
    private Toggle[] uitoggle_Axises;

    // Const
    private Vector3[] vectors = new Vector3[3] { Vector3.right, Vector3.up, Vector3.forward };

    private void Awake()
    {
        uitoggle_Axises = new Toggle[3] { uitoggle_XAxis, uitoggle_YAxis, uitoggle_ZAxis };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();   
    }

    void HandleInput()
    {
        if (selectionManager.OperationMode == SelectionManager.OpMode.Translation) 
        {
            Debug.Log("Translation Manager Running");
            if (Input.GetKeyDown("x"))
            {
                ToggleActiveAxis(0);
            }
            if (Input.GetKeyDown("y"))
            {
                ToggleActiveAxis(1);
            }
            if (Input.GetKeyDown("z"))
            {
                ToggleActiveAxis(2);
            }
            Vector2 mScrollDelta = Input.mouseScrollDelta;
            if(mScrollDelta.y != 0)
            {
                MoveAllSelected(mScrollDelta.y);
            }
        }
    }

    void MoveAllSelected(float delta)
    {
        foreach(SelectableScript s in selectionManager.GetSelected())
        {
            s.GetComponent<Translationable>().HandleTranslation(baseDirection *unitTransform* delta);
        }
    }

    void ToggleActiveAxis(int id)
    {
        activeAxis[id] = !activeAxis[id];
        uitoggle_Axises[id].isOn = activeAxis[id];
        baseDirection += (activeAxis[id] ? 1 : -1) * vectors[id];
    }
}
