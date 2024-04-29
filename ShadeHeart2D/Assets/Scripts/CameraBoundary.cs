using UnityEngine;
using Cinemachine;

public class CameraBoundary : MonoBehaviour
{
    public GameObject borderTop;
    public GameObject borderBottom;
    public GameObject borderLeft;
    public GameObject borderRight;

    private CinemachineVirtualCamera virtualCamera;
    private float cameraHalfHeight;
    private float cameraHalfWidth;

    void Start()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        // Calculate half of the camera's height and width
        cameraHalfHeight = virtualCamera.m_Lens.OrthographicSize;
        cameraHalfWidth = cameraHalfHeight * virtualCamera.m_Lens.Aspect;
    }

    void Update()
    {
        // Calculate the boundaries based on the border game objects' positions
        float minX = Mathf.Abs(borderLeft.transform.position.x) + cameraHalfWidth;
        float maxX = Mathf.Abs(borderRight.transform.position.x) - cameraHalfWidth;
        float minY = Mathf.Abs(borderBottom.transform.position.y) + cameraHalfHeight;
        float maxY = Mathf.Abs(borderTop.transform.position.y) - cameraHalfHeight;

        // Get the current camera position from the Camera GameObject
        Vector3 cameraPosition = transform.position;

        // Clamp the camera's position within the boundaries
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minX, maxX);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, minY, maxY);

        // Update the Camera GameObject's position
        transform.position = cameraPosition;
    }
}
