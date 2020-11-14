using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
   public enum ADSType
    {
        HoldToAim,
        ClickToAim
    }
    // Start is called before the first frame update
    [Header("Camera target position")]
    public Transform Target;

    [Header("Camera Aim Settings")]
    public float VerticalSensitivity;
    public float HorizontalSensitivity;
    public float MaxLookUpAngle;
    public float MinLookUpAngle;
    public float FOV;
    [Header("ADS")]
    public float ADSVerticalSensitivity;
    public float ADSHorizontalSensitivity;
    public ADSType Aimingmode;
    public float ADSFOV;
    public float ADSspeed;
    Vector2 mouseInput;
    float rotX=0, rotY=0;
    bool aimingDownSights = false;
    float Xsensitivity;
    float Ysensitivity;
    Camera main;
    public PlayerInputController playerInputController;
    

    void Start()
    {
        main = GetComponent<Camera>();
       
    }

    private void Update()
    {
        #region AimDownSights
        ADS();
        #endregion
        #region CameraMovement
        mouseInput = playerInputController.inputActions.Player.Look.ReadValue<Vector2>();
        rotX += mouseInput.x * Xsensitivity;
        rotY += mouseInput.y * Ysensitivity;
        rotY = Mathf.Clamp(rotY, MinLookUpAngle, MaxLookUpAngle);
        transform.localRotation = Quaternion.Euler(-rotY, rotX, 0f);
        #endregion

       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
      
        transform.position = Target.position;
    }

 
    void ADS()
    {
        switch (Aimingmode)
        {
            case ADSType.ClickToAim:
                if(playerInputController.inputActions.Player.ADS.triggered)
                aimingDownSights = !aimingDownSights;
                break;

            case ADSType.HoldToAim:
                playerInputController.inputActions.Player.ADS.performed += aim => aimingDownSights = true;
                playerInputController.inputActions.Player.ADS.canceled += aim => aimingDownSights = false;
                break;
        }

        if (aimingDownSights)
        {
            main.fieldOfView = Mathf.Lerp(main.fieldOfView, ADSFOV, ADSspeed);
            Xsensitivity = ADSHorizontalSensitivity;
            Ysensitivity = ADSVerticalSensitivity;
        }
        else
        {
            main.fieldOfView = Mathf.Lerp(main.fieldOfView, FOV, ADSspeed);
            Xsensitivity = HorizontalSensitivity;
            Ysensitivity = VerticalSensitivity;
        }
    }
}
