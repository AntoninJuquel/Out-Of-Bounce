using System.Collections;
using Systems.Event.Scripts.Channels;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private float moveSpeed = 7f, zoomSpeed = 7f, zoomStep = 3f;
    [SerializeField] private FloatEventChannelSo xAxisChannelSo, yAxisChannelSo;
    private Camera _camera;
    private Transform _transform;
    private Rigidbody2D _target;
    private float _targetZoom;
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
        if (!_target) return;
        _position = _transform.position;
        _targetVelocity = _target.velocity;
        _targetPos = _target.position + (Vector2) _targetVelocity.normalized;

        if (!_zooming)
        {
            _targetZoom = Mathf.Clamp(_targetVelocity.magnitude, 10f, 20f);
        }
        
        if(GameManager.GameStatus == GameStatus.GameOver) return;
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * zoomSpeed);
        xAxisChannelSo.RaiseEvent(_position.x);
        yAxisChannelSo.RaiseEvent(_position.y);
    }

    private void LateUpdate()
    {
        if (!_target || Input.GetMouseButton(0)) return;
        _transform.position = Vector3.SmoothDamp(_position, _targetPos + Vector3.back, ref _moveVelocity, 1f / moveSpeed);
    }

    private IEnumerator ZoomRoutine()
    {
        _targetZoom = _camera.orthographicSize - zoomStep;
        zoomSpeed *= 5;
        _zooming = true;
        yield return new WaitForSeconds(1f / zoomSpeed);
        zoomSpeed /= 5;
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