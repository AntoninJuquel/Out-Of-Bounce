using System;
using Statistics;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerSave
    {
        [field: SerializeField] public int Coins { get; private set; }

        public PlayerSave(int coins)
        {
            Coins = coins;
        }
    }
}