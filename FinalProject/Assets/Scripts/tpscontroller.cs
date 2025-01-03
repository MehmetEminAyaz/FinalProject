using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(CharacterController))]
public class tpscontroller : MonoBehaviour
{
    [HideInInspector] public InputManager inputManager;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Vector3 motionVector, gravityVector;
    [HideInInspector] public Animator animator;

    public float gravityPower = -9.8f;
    public float jumpValue = -9.8f;
    [Range(1,4)]public float movementSpeed = 2;
    [Range(0,1)] public float groundClearance;
    [Range(0, 1)] public float groundDistance;

    private float gravityForce = -9.18f;
    private bool cursorLocked;
    private Vector3 relativeVector;
    public GameObject focusPoint;
    private float turnDirection;
    public float turnMulitpler;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        movement();
        mouseLook();
    }

    void mouseLook()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cursorLocked = cursorLocked ? false : true;
        }
        if (!cursorLocked) return;

        relativeVector = transform.InverseTransformPoint(focusPoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        turnDirection = (relativeVector.x / relativeVector.magnitude);

        //vertical
        focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + Input.GetAxis("Mouse Y"), focusPoint.transform.eulerAngles.y, 0);
        //horizontal
        focusPoint.transform.parent.Rotate(transform.up * Input.GetAxis("Mouse X")* 100 * Time.deltaTime);
    }

    void movement()
    {
        animator.SetFloat("vertical", inputManager.vertical);
        animator.SetFloat("horizontal", inputManager.horizontal);
        animator.SetBool("grounded", isGrounded());
        animator.SetFloat("jump", inputManager.jump);
        if (isGrounded() && gravityVector.y < 0)
        {
            gravityVector.y = -2;
        }
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        gravityVector.y += gravityPower * Time.deltaTime;
        characterController.Move(gravityVector * Time.deltaTime);
        if (isGrounded())
        {
            motionVector = transform.right * inputManager.horizontal + transform.forward * inputManager.vertical;
            characterController.Move(motionVector * movementSpeed * Time.deltaTime);
        }

        if (inputManager.vertical > 0)
        {
            characterController.Move(motionVector * movementSpeed * Time.deltaTime);
            transform.Rotate(transform.up *  turnDirection * turnMulitpler * Time.deltaTime );
            focusPoint.transform.parent.Rotate(transform.up * -turnDirection * turnMulitpler * Time.deltaTime);
        }

        if (inputManager.jump != 0)
        {
            jump();
        }
    }
    void jump()
    {
        if(isGrounded())
        characterController.Move(transform.up * (jumpValue * -2 * gravityForce * Time.deltaTime));
    }
    bool isGrounded()
    {
        return Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z), groundClearance);
    }

    void OnGUI()
    {
        float rectPos = 50;
        GUI.Label(new Rect(20, rectPos, 200, 20), "Is Grounded" + isGrounded());
        rectPos += 30f;
        GUI.Label(new Rect(20, rectPos, 200, 20), "cursorLocked" + cursorLocked);
        rectPos += 30f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z), groundClearance);
    }
}
