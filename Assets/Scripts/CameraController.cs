using System.Collections;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private float moveSpeed = 7f, zoomSpeed = 7f, zoomStep = 3f;
    private Camera _camera;
    private Transform _transform;
    private Rigidbody2D _target;
    private float _targetZoom, _zoomVelocity;
    private Vector3 _moveVelocity, _position, _targetPos, _targetVelocity;
    private bool _zooming;

    private void Awake()
    {
        Instance = this;
        _camera = GetComponent<Camera>();
        _transform = transform;
    }

    private void Update()
    {
        _position = _transform.position;
        _targetVelocity = _target.velocity;
        _targetPos = _target.position + (Vector2) _targetVelocity.normalized;

        if(!_zooming)
        {
            _targetZoom = Mathf.Clamp(_targetVelocity.magnitude, 10f, 20f);
        }
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoom, ref _zoomVelocity, 1f / zoomSpeed);
    }

    private void LateUpdate()
    {
        if (!_target || Input.GetMouseButton(0)) return;
        _transform.position = Vector3.SmoothDamp(_position, _targetPos + Vector3.back, ref _moveVelocity, 1f / moveSpeed);
    }

    private IEnumerator ZoomRoutine()
    {
        _targetZoom = _camera.orthographicSize - zoomStep;
        zoomSpeed *= 20;
        _zooming = true;
        yield return new WaitForSeconds(zoomStep / zoomSpeed);
        zoomSpeed /= 20;
        _zooming = false;
    }

    public void SetTarget(Rigidbody2D target) => _target = target;
    public void SetTarget(GameObject target) => _target = target.GetComponent<Rigidbody2D>();

    public void Zoom()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomRoutine());
    }
}