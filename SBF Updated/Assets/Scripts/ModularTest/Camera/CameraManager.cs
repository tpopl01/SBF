using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CameraState cameraState = CameraState.ThirdPerson;
    FollowTransform followTransform;
    RotateCamera rotateCamera;
    CameraCollision cameraCollision;
    [SerializeField] Transform player = null;
    [SerializeField] float thirdPersonCamZ = 0;
    Transform camTP;
    public Transform cameraMain()
    {
        if (cameraState == CameraState.FirstPerson)
            return firstPersonCam;
        return camTP;
    }
    Transform pivot;
    Vector3 defaultPivot;

    [SerializeField] Transform firstPersonCam = null;
    [SerializeField] Sway sway = null;
    [SerializeField] Lean lean = null;
   // [SerializeField] Kickback kickback = null;
    [SerializeField] FPMovementAnims movementAnims = null;
    [SerializeField] bool debugAim;
    Rigidbody playerRB;

    public static CameraManager instance;
    private void Awake()
    {
        pivot = transform.GetChild(0);
        camTP = GetComponentInChildren<Camera>().transform;
        instance = this;
        if (firstPersonCam.gameObject.activeSelf)
            firstPersonCam.gameObject.SetActive(true);
        if (!pivot.gameObject.activeSelf)
            pivot.gameObject.SetActive(false);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
        this.playerRB = player.GetComponent<Rigidbody>();
    }

    float camOffset = -2.5f;
    public void SetCameraOffset(Vector3 pivotOffset, float offset = -2.5f)
    {
        if(pivotOffset == Vector3.zero)
        {
            pivot.transform.localPosition = new Vector3(0.17f, 1.8f, 0);
        }
        else
        {
            pivot.transform.localPosition = pivotOffset;
        }
        camOffset = offset;
        camTP.transform.localPosition = new Vector3(0, 0, offset);
    }

    public void SetState(Transform t, CameraState state)
    {
        cameraState = state;
        player = t;
    }

    private void Start()
    {
        defaultPivot = pivot.localPosition;
        followTransform = GetComponent<FollowTransform>();
        rotateCamera = GetComponent<RotateCamera>();
        cameraCollision = GetComponent<CameraCollision>();
        playerRB = player.GetComponent<Rigidbody>();
        if (firstPersonCam.gameObject.activeSelf)
            firstPersonCam.gameObject.SetActive(false);
        if (!pivot.gameObject.activeSelf)
            pivot.gameObject.SetActive(true);
    }

    public void ChangeState(CameraState state)
    {
        cameraState = state;
    }

    private void Update()
    {
        switch (cameraState)
        {
            case CameraState.ThirdPerson:
                if (firstPersonCam.gameObject.activeSelf)
                {
                    movementAnims.StopAnims();
                    firstPersonCam.gameObject.SetActive(false);
                }
                if (!pivot.gameObject.activeSelf)
                    pivot.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Mouse1) || debugAim)
                {
                    cameraState = CameraState.ThirdPersonAim;
                }
                if(Input.GetKeyDown(KeyCode.F))
                {
                    cameraState = CameraState.FirstPerson;
                }
                break;
            case CameraState.FirstPerson:
                if (pivot.gameObject.activeSelf)
                {
                    movementAnims.StartAnims();
                    pivot.gameObject.SetActive(false);
                }
                if (!firstPersonCam.gameObject.activeSelf)
                    firstPersonCam.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    cameraState = CameraState.ThirdPerson;
                }
                followTransform.Follow(player, Vector3.up * 1.8f);
                if(!PauseMenu.singleton.InMenu)
                rotateCamera.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), firstPersonCam);
                sway.WeaponSway();
                lean.HandleLean(player.position, Vector3.up * 1.8f, Input.GetKey(KeyCode.Mouse1));
                if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                    movementAnims.MoveRunAnims();
                else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    movementAnims.MoveWalkAnims(playerRB.velocity.magnitude);
                else
                    movementAnims.MoveAimAnims();
                break;
            case CameraState.Lock:
                break;
            case CameraState.ThirdPersonAim:
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    cameraState = CameraState.ThirdPerson;
                }
                break;
            case CameraState.vehicle:
                followTransform.Follow(player.position, 100);
                rotateCamera.Rotate(0, Input.GetAxis("Mouse Y"), pivot);
                Vector3 rot = transform.eulerAngles;
                rot.y = player.transform.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime *10);
                break;
            case CameraState.spaceShip:
                followTransform.Follow(player.position, 100);
                //   followTransform.Follow(player, Vector3.zero);
                //  transform.forward = player.forward;
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (cameraState)
        {
            case CameraState.vehicle:
                followTransform.Follow(player.position, 100);
                break;
            case CameraState.spaceShip:
                followTransform.Follow(player.position, 100);
                break;
        }
    }

    private void FixedUpdate()
    {
        switch(cameraState)
        {
            case CameraState.ThirdPerson:
                followTransform.Follow(player);
                rotateCamera.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), pivot);
                pivot.localPosition = defaultPivot;
                cameraCollision.CollisionCamera();
                pivot.localPosition = Vector3.Lerp(pivot.localPosition, defaultPivot, Time.deltaTime * 5);
                break;
            case CameraState.ThirdPersonAim:
                Vector3 pos = camTP.localPosition;
                pos.z = thirdPersonCamZ;
                camTP.localPosition = Vector3.Lerp(camTP.localPosition, pos, Time.deltaTime * 5);
                pivot.localPosition = Vector3.Lerp(pivot.localPosition, Vector3.up*defaultPivot.y + Vector3.right * 0.5f, Time.deltaTime * 5);
                followTransform.Follow(player);
                rotateCamera.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), pivot);
                break;
            case CameraState.spaceShip:
                followTransform.Follow(player);
                rotateCamera.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), pivot, -80, 80);
                // rotateCamera.Rotate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                break;
            case CameraState.FirstPerson:
                break;
            case CameraState.Lock:
                break;

            case CameraState.vehicle:
                cameraCollision.CollisionCamera(camOffset);
                break;
        }
    }


}

public enum CameraState
{
    ThirdPerson,
    FirstPerson,
    Lock,
    ThirdPersonAim,
    vehicle,
    spaceShip
}
