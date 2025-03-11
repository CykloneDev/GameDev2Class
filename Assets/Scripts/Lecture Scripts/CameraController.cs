using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int _sensitivity;
    [SerializeField] int _lockVertMin, _lockVertMax;
    [SerializeField] bool _invertY;

    private float _rotX;

    private void Awake()
    {
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get input
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        if (_invertY)
            _rotX += mouseY;
        else
            _rotX -= mouseY;

        // Clamp the camera on the x axis
        _rotX = Mathf.Clamp(_rotX, _lockVertMin, _lockVertMax);

        // rotate the camera on the x axis to look up and down
        transform.localRotation = Quaternion.Euler(_rotX, 0, 0);

        // rotate the player on the y axis to look left and right
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}