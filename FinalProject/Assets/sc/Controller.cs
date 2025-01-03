using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    float inputX;
    float inputY;
    public Transform Model;

    Animator Anim;
    Vector3 StickDirection;
    Camera MainCam;
    public float damp;
    [Range(1,20)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        MainCam=Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        StickDirection=new Vector3(inputX,0,inputY);
        InputMove();
        InputRotation();
    }

    void InputMove()
    {
        Anim.SetFloat("speed", Vector3.ClampMagnitude(StickDirection, 1).magnitude, damp, Time.deltaTime * 10);
    }

    void InputRotation()
    {
        Vector3 rotOfset = MainCam.transform.TransformDirection(StickDirection);
        rotOfset.y = 0;
        Model.forward=Vector3.Slerp(Model.forward, rotOfset,Time.deltaTime* rotationSpeed);
    }
}
