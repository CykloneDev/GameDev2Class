using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens = 300; 
    [SerializeField] int lockVertMin = -90, lockVertMax = 90;
    [SerializeField] bool invertY = false;
    [SerializeField] Vector3 normalOffset = new Vector3(0f, 1.8f, 0.2f); //Default Camera Position
    [SerializeField] Vector3 crouchOffset = new Vector3(0f, 0.5f, 0f);//How much the camera drops when crouching.
    [SerializeField] float offsetSpeed = 5f;   
    [SerializeField] Transform head;
    [SerializeField] CharacterController Charactercontroller;
    [SerializeField] Transform player;

    float rotX; //Rotation X

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;//Locking the Cursor while playing.

        Charactercontroller = GetComponentInParent<CharacterController>();
        player = transform.parent;

    }

    void Update()
    {
        HandleRotation();
        UpdateCameraPosition();
    }

    void HandleRotation() //Handles the mouse and camera rotation.
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    void UpdateCameraPosition()
    {
        Vector3 targetPosition = normalOffset; // Start with normal position
        if (Charactercontroller.height < 2f) // Crouching
        {
            //Subtracts the crouching offset from the normal offset.
            targetPosition = normalOffset - crouchOffset;
        }

        //Smooth transition using Lerp.
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * offsetSpeed);
    }
}