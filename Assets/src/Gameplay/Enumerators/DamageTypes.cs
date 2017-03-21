using UnityEngine;
using System.Collections;

[System.Flags, System.Serializable]
public enum DamageTypes
{
	untyped = 0x0,
	physical = 0x1,
	slashing = 0x2,
	piercing = 0x4,
	bludgeoning = 0x8,
	magic = 0x10,
	coldiron = 0x20,
	silver = 0x40,
	adamantine = 0x80,
	fire = 0x100,
	cold = 0x200,
	electric = 0x400,
	acid = 0x800,
	sonic = 0x1000
}
