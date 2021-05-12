using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UserInterface
{
    public class StatisticMenuController : MonoBehaviour
    {
        [SerializeField] private PlayerSo playerSo;
        [SerializeField] private GameObject statisticItemTemplate;
        [SerializeField] private RectTransform content;

        private void OnEnable()
        {
            foreach (var statistic in playerSo.GetStatistics())
            {
                var statisticGo = Instantiate(statisticItemTemplate, content);
                statisticGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = statistic.Key.ToString().ToUpper();
                statisticGo.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.best.ToString();
                statisticGo.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.last.ToString();
                statisticGo.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.total.ToString();
            }
            statisticItemTemplate.SetActive(false);
        }
    }
}