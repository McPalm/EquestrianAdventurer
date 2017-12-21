using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/**
 * This script exist to aid the player character.
 * Softens would be killing blows to deal minimum damage, sometimes saving the character
 * 
 */

namespace Gameplay.DefensiveAbilities
{
	public class LastChance : MyBehaviour
    {
        void OnHurt(DamageData dd)
        {
            if(dd.TotalDamage >= GetComponent<HitPoints>().CurrentHealth)
            {
                dd.SetDamage(dd.minDamage);
            }
        }

        // observer management

        void Subscribe ()
        {
            GetComponent<HitPoints>().EventBeforeHurtFinal.AddListener(OnHurt);
        }

        void UnSubscribe ()
        {
            GetComponent<HitPoints>().EventBeforeHurtFinal.RemoveListener(OnHurt);
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
