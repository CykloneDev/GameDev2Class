using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockvertMax;
    [SerializeField] bool invertY;

    float rotx;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        if(invertY)
        {
            rotx += mouseX;
        }
        else
        {
            rotx -= mouseY;
        }

        rotx = Mathf.Clamp(rotx, lockVertMin, lockvertMax);

        transform.localRotation = Quaternion.Euler(rotx, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
