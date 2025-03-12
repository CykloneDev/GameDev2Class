using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;


    float rotX; //Rotation X

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;



        //Clamp the Camera on the X Axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);


        //Rotate the Camera on the X Axis to look up and down
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //Rotate the Player on the Y Axis to look right and left
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
