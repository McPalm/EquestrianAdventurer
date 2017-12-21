using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/**
 * This script exist to aid the player character.
 * It primarly exist to make the game easier for the player, but also prevent the players from suffering from frustration.
 * 
 * Last Chance: When a character would suffer a killing blow, reduce the damage of that blow to minimum damage, this may save the player.
 * Improved Accuracy: When the players is missing attacks, increase chance ot hit the next attack.
 * Improved Evasion: When the players is hit enough, make them automatically dodge.
 * Dire Straights: When the players is low on health, sometimes grant automatical critical hits
 * 
 */

namespace Gameplay
{
	public class BadluckProtection : MyBehaviour
    {
        public bool lastChance = true;
        public bool impAcc = true;
        public bool impEva = true;
        public bool direStraighs = true;

        void LastChance(DamageData dd)
        {
            if(lastChance && dd.TotalDamage >= GetComponent<HitPoints>().CurrentHealth)
            {
                Debug.Log(string.Format("Last Chancre Triggered! {0} -> {1}", dd.damage, dd.minDamage));
                dd.SetDamage(dd.minDamage);
            }
        }


        // observer management

        void Subscribe ()
        {
            GetComponent<HitPoints>().EventBeforeHurtFinal.AddListener(LastChance);
        }

        void UnSubscribe ()
        {
            GetComponent<HitPoints>().EventBeforeHurtFinal.RemoveListener(LastChance);
        }

        private void Awake()
        {
            EventDisable.AddListener(UnSubscribe);
        }

        private void OnEnable()
        {
            Subscribe();
        }
    }
}
