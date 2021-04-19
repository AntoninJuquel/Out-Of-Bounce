using System;
using UnityEngine;

namespace UserInterface
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject[] menus;

        private void Start()
        {
            SetActiveMenu(0);
        }

        public void SetActiveMenu(int index)
        {
            for (var i = 0; i < menus.Length; i++)
            {
                menus[i].SetActive(i == index);
            }
        }
    }
}