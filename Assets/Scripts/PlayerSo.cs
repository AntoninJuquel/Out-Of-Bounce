using System;
using System.Collections.Generic;
using Systems.AchievementSystem;
using Systems.UnlockSystem;
using UnityEngine;


[CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
public class PlayerSo : ScriptableObject
{
    [SerializeField] private Vault vault = new Vault();
    [SerializeField] private Achievements achievements = new Achievements();
    public Achievements GetAchievements() => achievements;
}