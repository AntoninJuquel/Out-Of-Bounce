using UnityEngine;

namespace Environment
{
    public class Background : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1;
        private Camera _camera;
        private Transform _transform;
        private Material _material;


        private void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
            _material = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            var cameraPosition = _camera.transform.position;
            _transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 10);
            _transform.rotation = Quaternion.identity;
            _material.mainTextureOffset = cameraPosition * scrollSpeed;
        }
    }
}