using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FurnitureInteractionManager : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private bool _isScaling = false;
    private float _initialDistance;
    private Vector3 _initialScale;

    private bool _isRotating = false;
    private float _initialRotationAngle;

    private bool _isMoving = false;
    private Vector3 _initialPosition;
    private Transform _objectTransform;
    private Camera _camera;
    private float moveSpeed = 0.01f;
    void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _objectTransform = transform;
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1 && !_isScaling && !_isRotating)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _isMoving = true;
                _initialPosition = _objectTransform.position;
            }
            else if (touch.phase == TouchPhase.Moved && _isMoving)
            {
                Vector2 touchDelta = touch.deltaPosition;
                float moveAmount = touchDelta.y * moveSpeed;

                Vector3 moveDirection = _camera.transform.forward;
                moveDirection.y = 0;
                moveDirection.Normalize();

                _objectTransform.position += moveDirection * moveAmount;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isMoving = false;
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (!_isScaling && !_isRotating)
            {
                _isScaling = true;
                _initialDistance = currentDistance;
                _initialScale = _objectTransform.localScale;

                _isRotating = true;
                _initialRotationAngle = GetAngleBetweenTouches(touch0, touch1);
            }
            else
            {
                float scaleFactor = currentDistance / _initialDistance;
                _objectTransform.localScale = _initialScale * scaleFactor;

                float currentAngle = GetAngleBetweenTouches(touch0, touch1);
                float angleDifference = currentAngle - _initialRotationAngle;
                _objectTransform.Rotate(Vector3.up, -angleDifference, Space.World);
                _initialRotationAngle = currentAngle;
            }
        }
        else
        {
            _isScaling = false;
            _isRotating = false;
        }
    }

    private float GetAngleBetweenTouches(Touch touch0, Touch touch1)
    {
        Vector2 direction = touch1.position - touch0.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
