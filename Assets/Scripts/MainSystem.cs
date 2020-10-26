using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{

    // Setting
    [Header("Camera Setting")]
    public Camera freeCamera;
    public FirstPersonAIO player;

    //[Header("Highlight Setting")]
    //public float outlineWidth = 10f;

    [Header("Operation Scripts")]
    public TransformManager transformManager;

    //[Header("UI Setting")]
    //public Text uitext_selectMode;



    private List<SelectableScript> selectedObjectList = new List<SelectableScript>();
    // Start is called before the first frame update
    void Start()
    {
        CameraMode = CamMode.FreeMode;
        allSelectable = new List<SelectableScript>(FindObjectsOfType<SelectableScript>());
        gvrControllerInputDevice = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyInput();
    }

    Vector3 prePos, curPos;

    void HandleKeyInput()
    {
        if (Input.GetKeyDown("e"))
        {
            //transformManager.init(this);
            foreach (SelectableScript s in allSelectable)
            {
                Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    //rigidbody.isKinematic = true;
                    rigidbody.useGravity = false;

                }
            }
        }
        else if (Input.GetKeyDown("q"))
        {
            foreach (SelectableScript s in allSelectable)
            {
                Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                if (rigidbody != null)

                {
                    rigidbody.isKinematic = false;
                }
            }

        }
        if (CameraMode == CamMode.FreeMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (SelectableScript s in selectedObjectList)
                {
                    Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.velocity = Vector3.zero;
                    }

                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = ScreenPointToRay();
                prePos = curPos;
                curPos = ray.GetPoint(3);
                foreach (SelectableScript s in selectedObjectList)
                {
                    Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        //rigidbody.AddForce( 4 * (p - rigidbody.transform.position));
                        rigidbody.transform.position = curPos;
                    }

                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 throwVel = curPos - prePos;
                float speed = throwVel.magnitude / Time.deltaTime;
                Debug.Log("Speed " + speed.ToString());
                Debug.Log("Thr: " + throwVel.ToString());
                foreach (SelectableScript s in selectedObjectList)
                {
                    Rigidbody rigidbody = s.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.velocity = speed * throwVel.normalized;
                    }

                }
            }
        }

        if (Input.GetKeyDown("f"))
        {
            ToggleCamMode();
        }
    }

    // Lien quan den list object duoc chon
    private List<SelectableScript> allSelectable;
    public void registationSelectable(SelectableScript s)
    {
        allSelectable.Add(s);
    }
    public void addToSelectedObjects(SelectableScript s)
    {
        selectedObjectList.Add(s);
        Debug.Log("Add to selected " + s.ToString());
    }

    public void removeFromSelectedObjects(SelectableScript s)
    {
        selectedObjectList.Remove(s);
        Debug.Log("Remove from selected " + s.ToString());
    }

    public List<SelectableScript> GetSelected()
    {
        return selectedObjectList;
    }

    public bool IsSelectedListEmpty()
    {
        return selectedObjectList.Count == 0;
    }

    // Lien quan den camera
    private Vector3 SCREEN_MID_POINT = new Vector3(0.5f, 0.5f, 0);
    public enum CamMode
    {
        FreeMode, InGameMode
    }
    private CamMode _cameraMode;
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

    public Ray ScreenPointToRay()
    {
        if (CameraMode == CamMode.FreeMode)
        {
            return freeCamera.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            GvrBasePointer.PointerRay pointerRay = player.playerCamera.GetComponent<GvrPointerPhysicsRaycaster>().GetLastRay();
            return pointerRay.ray;
        }
    }

    GvrControllerInputDevice gvrControllerInputDevice;
    bool buttonState = false;
    public bool ButtonDown()
    {
        return Input.GetMouseButtonDown(0);
        
    }

    public bool ButtonUp()
    {
        return Input.GetMouseButtonUp(0); 
    }
}
