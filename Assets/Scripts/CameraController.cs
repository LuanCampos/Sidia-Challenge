using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("The speed at which the camera zooms in and out.")]
    [SerializeField] private float zoomSpeed = 5f;
    [Tooltip("The minimum zoom of the camera.")]
    [SerializeField] private float minZoom = 40f;
    [Tooltip("The maximum zoom of the camera.")]
    [SerializeField] private float maxZoom = 100f;
    [Tooltip("The speed at which the camera follows the player.")]
    [SerializeField] private float followSpeed = 1f;
    [Tooltip("The speed at which the camera moves when the player is dragging it.")]
    [SerializeField] private float dragSpeed = 2f;

    [Header("References")]
    [Tooltip("The game manager.")]
    [SerializeField] private GameManager gameManager;

    private Camera cam;
    private Vector3 target;
    private Vector3 lastMousePos;

    private void Start()
    {
        cam = GetComponent<Camera>();
        target = gameManager.GetPositionOfCurrentPlayer();
        target.y = transform.position.y;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0f)
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);

        if (Input.GetMouseButtonDown(1))
            lastMousePos = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 move = new Vector3(-delta.x * dragSpeed / 100f, -delta.y * dragSpeed / 100f, 0);
            transform.Translate(move, Space.Self);
            lastMousePos = Input.mousePosition;
        }
        else
            transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        target = gameManager.GetPositionOfCurrentPlayer();
        target.y = transform.position.y;
    }
}