using UnityEngine;

namespace UserInterface
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private GameObject ambient;
        private Material _material;
        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            _material.mainTextureOffset = transform.position / 100f;
        }

        public void SetColor(Color color) => _material.color = color;

        public void SetActive(bool enable)
        {
            PlayerPrefs.SetInt("Background",enable ? 1 : 0);
            gameObject.SetActive(enable);
        }
        public void SetAmbientActive(bool enable)
        {
            PlayerPrefs.SetInt("Ambient",enable ? 1 : 0);
            ambient.SetActive(enable);
        }
    }
}
