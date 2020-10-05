using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    #region Look Settings
    public bool enableCameraMovement = true;
    public enum InvertMouseInput { None, X, Y, Both }
    public InvertMouseInput mouseInputInversion = InvertMouseInput.None;

    public enum CameraInputMethod { Traditional, TraditionalWithConstraints, Retro }
    public CameraInputMethod cameraInputMethod = CameraInputMethod.Traditional;

    public float verticalRotationRange = 170;
    public float mouseSensitivity = 10;
    public float fOVToMouseSensitivity = 1;
    public float cameraSmoothing = 5f;
    public bool lockAndHideCursor = false;
    public Camera playerCamera;
    public bool enableCameraShake = false;
    internal Vector3 cameraStartingPosition;
    float baseCamFOV;


    public bool autoCrosshair = false;
    public bool drawStaminaMeter = true;
    float smoothRef;
    Image StaminaMeter;
    Image StaminaMeterBG;
    public Sprite Crosshair;
    public Vector3 targetAngles;
    private Vector3 followAngles;
    private Vector3 followVelocity;
    private Vector3 originalRotation;
    #endregion


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
