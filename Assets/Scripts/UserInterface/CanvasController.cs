using System;
using UnityEngine;

namespace UserInterface
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject[] menus;

        private void Start()
        {
            SetActiveMenu(menus[0]);
        }

        public void SetActiveMenu(GameObject menuTarget)
        {
            foreach (var menu in menus)
            {
                menu.SetActive(menuTarget.name == menu.name);
            }
        }
    }
}