using UnityEngine;

namespace UserInterface
{
    public class BackgroundController : MonoBehaviour
    {
        private Material _material;
        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            _material.mainTextureOffset = transform.position / 100f;
        }
    }
}
