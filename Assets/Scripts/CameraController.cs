using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mainSpeed = 0.02f;
    public float xRot = 3f;
    public float yRot = 3f;
    public float shiftSpeed = 20f;
    public GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!StaticDatas.editMode){
            float shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? shiftSpeed: 1f;
            Transform　trans = transform;
            trans.position += trans.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") * mainSpeed * shift;
            trans.position += trans.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal") * mainSpeed * shift;
            trans.position += trans.TransformDirection(Vector3.up) * Input.GetAxis("UpDown") * mainSpeed * shift;
            transform.position = trans.position;

            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");
                x = x * xRot;
                y = y * yRot;
                this.transform.Rotate(0,x,0);
                MainCamera.transform.Rotate(-y,0,0);
            }
        }
    }
}
