using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed;
    public float speed = 50.0f;
    public float sensitivity = 0.25f;
    public bool inverted = false;

    private Vector3 lastMouse = new Vector3(225, 225, 225);

    public bool smooth = true;
    public float acceleration = 0.01f;
    private float actSpeed = 0.0f;
    private Vector3 lastDir = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        //TODO: the speed needs to be determined by moog?
        moveSpeed = 50.0f;  
    }

    // Update is called once per frame
    void Update()
    {     
        lastMouse = Input.mousePosition - lastMouse;
        if (!inverted) lastMouse.y = -lastMouse.y;
        lastMouse *= sensitivity;
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.y, transform.eulerAngles.y + lastMouse.x, 0);
        transform.eulerAngles = lastMouse;

        lastMouse = Input.mousePosition;


        //TODO: Read type of input and determine what code to use.
        //TODO: if it is joystick/xbox - use the following code:
        transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
                            0f,
                            moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        //TODO: if it is uncontrolled by the subject, create the given trajectory movement
        /*Vector3 dir = new Vector3();

        if (Input.GetKey(KeyCode.W)) dir.z += 1.0f;
        if (Input.GetKey(KeyCode.S)) dir.z -= 1.0f;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1.0f;
        if (Input.GetKey(KeyCode.D)) dir.x += 1.0f;
        dir.Normalize();

        if(dir != Vector3.zero)
        {
            if (actSpeed < 1)
                actSpeed += acceleration * Time.deltaTime * 40;
            else
                actSpeed = 1.0f;
            lastDir = dir;
        }
        else
        {
            if (actSpeed > 0)
                actSpeed -= acceleration * Time.deltaTime * 20;
            else
                actSpeed = 0.0f;
        }

        if (smooth)
            transform.Translate(lastDir * actSpeed * speed * Time.deltaTime);
        else
            transform.Translate(dir * speed * Time.deltaTime);*/
    }
}
