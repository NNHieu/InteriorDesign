using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    // Setting
    public Camera playerCamera;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material selectedMaterial;

    private Material originalMaterial;
    private SelectableScript selectedObject;
    public string selectableTag = "Selectable";

    // State variable
    private bool hasSelected = false;
    private SelectionMode currentMode;

    // Const
    private Vector3 screenMidPoint = new Vector3(0.5f, 0.5f, 0);
    public enum SelectionMode
    {
        SelectMode, EditMode
    }

    // Others
    private List<SelectableScript> selectedObjectList = new List<SelectableScript>();

    // Start is called before the first frame update
    void Start()
    {
        currentMode = SelectionMode.SelectMode;
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

        RaycastHit hit;
        SelectableScript newPointToObject = null;
        if (Physics.Raycast(playerCamera.ViewportPointToRay(screenMidPoint), out hit))
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

    public bool IsInMode(SelectionMode mode)
    {
        return currentMode == mode;
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
        if (Input.GetKey("e"))
        {
            currentMode = SelectionMode.EditMode;
            Debug.Log("Change to EditMode");
        }
        else if (Input.GetKey("q"))
        {
            currentMode = SelectionMode.SelectMode;
            Debug.Log("Change to SelectMode");
        }
    }
}
