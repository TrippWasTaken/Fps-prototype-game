using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public float mouseSens = 5f;
    public Transform playerBody;
    public Transform playerHead;
    public float cameraSpeed = 120f;
    public GameObject cameraFollowObj;
    public float smoothing = 3f;
    float xAxisClamp;
    float camX;
    float camY;
    //float xRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        MouseLook();
        CameraUpdate();
        CameraFollow();
    }

    void MouseLook(){
        //This whole func handles controls the models head and not the mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime * 200;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime * 200;

        xAxisClamp -= mouseY;
        
       //xRotation = Mathf.Clamp(xRotation, -90f, 75f);
        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 90f);
        //playerBody.Rotate(Vector3.up * mouseX);
        Vector3 playerRotate = playerBody.transform.rotation.eulerAngles;
        Vector3 headRotate = playerHead.transform.rotation.eulerAngles;

        playerRotate.y += mouseX;
        headRotate.y += mouseX;
        headRotate.x -= mouseY;
        headRotate.z = -90f;

        if (xAxisClamp > 60)
        {
            xAxisClamp = 60;
            headRotate.x = 60;
        }
        else if(xAxisClamp < -60)
        {
            xAxisClamp = -60;
            headRotate.x = 300;
        }
        playerBody.rotation = Quaternion.Euler(playerRotate);
        playerHead.rotation = Quaternion.Euler(headRotate);
    }

    void CameraUpdate(){
        //This func is for mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime * 200;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime * 200;
        camX += mouseY;
        camY += mouseX;
        camX = Mathf.Clamp(camX, -60, 60);

        Quaternion localRotation = Quaternion.Euler(-camX, camY, 0f);
        transform.rotation = localRotation;
    }
    void CameraFollow(){

        Transform target = cameraFollowObj.transform;

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, target.position, 10f);

    }
}
