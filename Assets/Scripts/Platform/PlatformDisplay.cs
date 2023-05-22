using TMPro;
using UnityEngine;

public class PlatformDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI platformText;

    public void UpdatePlatformText(int counter)
    {
        platformText.text = new string('|', counter);
    }
}