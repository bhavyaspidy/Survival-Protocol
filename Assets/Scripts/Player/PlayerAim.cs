using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    private void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(
            UnityEngine.InputSystem.Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}