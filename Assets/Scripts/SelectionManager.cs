using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    // Setting
    [Header("Camera Setting")]
    public Camera freeCamera;
    public FirstPersonAIO player;

    [Header("Highlight Setting")]
    public float outlineWidth = 10f;

    [Header("Operation Scripts")]
    public TransformManager transformManager;

    [Header("UI Setting")]
    public Text uitext_selectMode;



    private Material originalMaterial;
    private SelectableScript selectedObject;
    public string selectableTag = "Selectable";

    // State variable
    private bool hasSelected = false;

    public enum SelMode
    {
        SelectMode, EditMode, NoninteractMode
    }
    private SelMode _selectionMode;


    public enum CamMode
    {
        FreeMode, InGameMode
    }
    private CamMode _cameraMode;


    public enum OpMode
    {
        None, Transform, Rotation
    }
    private OpMode _operationMode;


    // Const
    private Vector3 screenMidPoint = new Vector3(0.5f, 0.5f, 0);



    // Others
    private List<SelectableScript> selectedObjectList = new List<SelectableScript>();
    private List<SelectableScript> allSelectable;

    // Start is called before the first frame update
    void Start()
    {
        SelectionMode = SelMode.SelectMode;
        OperationMode = OpMode.None;
        CameraMode = CamMode.FreeMode;
        allSelectable = new List<SelectableScript>(FindObjectsOfType<SelectableScript>());
    }

    // Update is called once per frame
    void Update()
    {
        trackHighLightObject();
        HandleMouseInput();
        HandleKeyInput();
    }

    public Ray ScreenPointToRay()
    {
        if (CameraMode == CamMode.FreeMode)
        {
            return freeCamera.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            return player.playerCamera.ViewportPointToRay(screenMidPoint);
        }
    }


    void trackHighLightObject()
    {
        if (SelectionMode == SelMode.NoninteractMode) return;

        RaycastHit hit;
        bool isHit = false;
        SelectableScript newPointToObject = null;
        //Debug.Log(freeCamera.ScreenPointToRay(Input.mousePosition));
        if (CameraMode == CamMode.FreeMode)
        {
            isHit = Physics.Raycast(freeCamera.ScreenPointToRay(Input.mousePosition), out hit);
        }
        else
        {
            Debug.Log("Ingame raycast");
            isHit = Physics.Raycast(player.playerCamera.ViewportPointToRay(screenMidPoint), out hit);
        }

        if (isHit)
        {
            // Debug.Log(hit.transform.name);   
            Transform hitTransform = hit.transform;
            if (hitTransform.CompareTag(selectableTag))
            {
                newPointToObject = hitTransform.GetComponent<SelectableScript>();
            }
        }
        if (!Renderer.ReferenceEquals(newPointToObject, selectedObject))
        {
            if (!SelectableScript.ReferenceEquals(selectedObject, null))
            {
                selectedObject.HandleUnpointed();
            }
            if (!SelectableScript.ReferenceEquals(newPointToObject, null))
            {
                // Neu tro toi object moi
                newPointToObject.HandlePointed();
                selectedObject = newPointToObject;
            }
            else
            {
                selectedObject = null;
            }
        }
    }


    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !ScriptableObject.ReferenceEquals(selectedObject, null))
        {
            if (selectedObject.IsSelected())
            {
                selectedObject.HandleDeselect();
                selectedObjectList.Remove(selectedObject);
            }
            else
            {
                selectedObject.HandleSelect();
                selectedObjectList.Add(selectedObject);
            }
        }
    }



    void HandleKeyInput()
    {
        if (Input.GetKeyDown("e"))
        {
            SelectionMode = SelMode.EditMode;
            transformManager.init(this);
            foreach (SelectableScript s in allSelectable)
            {
                Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }
            }
        }
        else if (Input.GetKeyDown("q"))
        {

            SelectionMode = SelMode.SelectMode;
            foreach (SelectableScript s in allSelectable)
            {
                Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                if (rigidbody != null)

                {
                    rigidbody.isKinematic = false;
                }
            }

        }
        uitext_selectMode.text = SelectionMode.ToString("G");

        //if (Input.GetKeyDown("r") && SelectionMode == SelMode.EditMode && selectedObjectList.Count > 0)
        //{
        //    OperationMode = OpMode.Transform;
        //}

        if (Input.GetKeyDown("f"))
        {
            ToggleCamMode();
        }
    }

    public List<SelectableScript> GetSelected()
    {
        return selectedObjectList;
    }

    public bool IsSelectedListEmpty()
    {
        return selectedObjectList.Count == 0;
    }
    // Get Set
    public Vector3 SumPositionSelected
    {
        get;
    }

    #region Set Get State
    public float OutlineWidth
    {
        get; set;
    }

    public SelMode SelectionMode
    {
        private set
        {
            if (value == SelMode.SelectMode)
            {
                _operationMode = OpMode.None;
            }
            _selectionMode = value;
        }
        get
        {
            return _selectionMode;
        }

    }

    public CamMode CameraMode
    {
        private set
        {
            _cameraMode = value;
            Debug.Log("Cur cam: " + value.ToString("G"));
            if (_cameraMode == CamMode.FreeMode)
            {
                player.ControllerPause();
                player.playerCamera.gameObject.SetActive(false);
                freeCamera.gameObject.SetActive(true);
            }
            else if (_cameraMode == CamMode.InGameMode)
            {
                player.ControllerPause();
                player.playerCamera.gameObject.SetActive(true);
                freeCamera.gameObject.SetActive(false);
            }
        }
        get
        {
            return _cameraMode;
        }
    }

    private void ToggleCamMode()
    {
        CameraMode = (CamMode)(1 - (int)CameraMode);
    }

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

    #endregion
}
