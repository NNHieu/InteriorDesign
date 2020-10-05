using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    // Setting
    public Camera freeCamera;
    public FirstPersonAIO player;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material selectedMaterial;

    public float outlineWidth = 10f;
    public float OutlineWidth
    {
        get; set;
    }

    // Setting UI
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
        None, Translation, Rotation
    }
    private OpMode _operationMode;


    // Const
    private Vector3 screenMidPoint = new Vector3(0.5f, 0.5f, 0);
    


    // Others
    private List<SelectableScript> selectedObjectList = new List<SelectableScript>();

    // Start is called before the first frame update
    void Start()
    {
        SelectionMode = SelMode.SelectMode;
        OperationMode = OpMode.None;
        CameraMode = CamMode.FreeMode;
    }

    // Update is called once per frame
    void Update()
    {
        trackHighLightObject();
        HandleMouseInput();
        HandleKeyInput();
    }


    void trackHighLightObject()
    {
        if (SelectionMode == SelMode.NoninteractMode) return;

        RaycastHit hit;
        bool isHit = false;
        SelectableScript newPointToObject = null;
        Debug.Log(freeCamera.ScreenPointToRay(Input.mousePosition));
        if(CameraMode == CamMode.FreeMode)
        {
            isHit = Physics.Raycast(freeCamera.ScreenPointToRay(Input.mousePosition), out hit);
        } else
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
                newPointToObject.HandlePointed(highlightMaterial);
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

                selectedObject.HandleDeselect(highlightMaterial);
                selectedObjectList.Remove(selectedObject);
            }
            else
            {
                selectedObject.HandleSelect(selectedMaterial);
                selectedObjectList.Add(selectedObject);
            }
        }
    }

    void HandleKeyInput()
    {
        if (Input.GetKeyDown("e"))
        {
            SelectionMode = SelMode.EditMode;
        }
        else if (Input.GetKeyDown("q")) { 

            SelectionMode = SelMode.SelectMode;

        }
            uitext_selectMode.text = SelectionMode.ToString("G");

        if (Input.GetKeyDown("r") && SelectionMode == SelMode.EditMode && selectedObjectList.Count > 0)
        {
            OperationMode = OpMode.Translation;
        }

        if (Input.GetKeyDown("f"))
        {
            ToggleCamMode();
        }
    }

    public List<SelectableScript> GetSelected()
    {
        return selectedObjectList;
    }

    #region Set Get State
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
            if(_cameraMode == CamMode.FreeMode)
            {
                player.ControllerPause();
                player.playerCamera.gameObject.SetActive(false);
                freeCamera.gameObject.SetActive(true);
            } else if(_cameraMode == CamMode.InGameMode)
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
