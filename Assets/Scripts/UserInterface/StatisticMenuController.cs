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
            var i = 0;
            foreach (var statistic in playerSo.GetStatistics())
            {
                var statisticGo = i < content.childCount ? content.GetChild(i) : Instantiate(statisticItemTemplate, content).transform;
                statisticGo.GetChild(0).GetComponent<TextMeshProUGUI>().text = statistic.Key.ToString().ToUpper();
                statisticGo.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.best.ToString();
                statisticGo.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.last.ToString();
                statisticGo.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = statistic.Value.total.ToString();
                i++;
            }
        }
    }
}