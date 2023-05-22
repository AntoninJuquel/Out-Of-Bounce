using UnityEngine;

namespace Environment
{
    public class Lava : MonoBehaviour
    {
        [SerializeField] private float speed = 1f, speed2 = 1f;
        private Camera _mainCamera;
        private Material _material;
        private static readonly int Shape2Tex = Shader.PropertyToID("_Shape2Tex");
        private Transform _transform;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _transform = transform;

            if (!_mainCamera)
            {
                enabled = false;
            }
        }


        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            var position = new Vector3(_mainCamera.transform.position.x, _transform.position.y, 0);
            _transform.position = position;
            _material.mainTextureOffset = new Vector2(position.x * speed, 0);
            _material.SetTextureOffset(Shape2Tex, new Vector2(position.x * speed2, 0));
        }
    }
}