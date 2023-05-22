using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Image fill;

        private void OnEnable()
        {
            fill.DOKill();
            fill.fillAmount = 0;
            fill.DOFillAmount(1, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            StartCoroutine(ActiveRoutine());
        }

        IEnumerator ActiveRoutine()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            gameObject.SetActive(false);
        }
    }
}