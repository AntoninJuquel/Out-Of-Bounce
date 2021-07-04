using System.Collections;
using Packages.LeanTween;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image fill;

        private void OnEnable()
        {
            DoMove();
            StartCoroutine(ActiveRoutine());
        }

        IEnumerator ActiveRoutine()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            LeanTween.cancel(gameObject);
            gameObject.SetActive(false);
        }

        void DoMove()
        {
            LeanTween.value(gameObject, 0, 1, 3f)
                .setOnUpdate(f => { fill.fillAmount = f; })
                .setOnComplete(() =>
                {
                    fill.fillAmount = 0;
                    DoMove();
                });
        }
    }
}