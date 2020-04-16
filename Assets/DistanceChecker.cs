using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour {

    //{WeaponType.Ranged, 7 },
    //{WeaponType.Dagger, 5 },
    //{WeaponType.Fisticuffs, 7},
    //{WeaponType.GreatAxe, 15},
    //{WeaponType.SwordAndShield, 10 },
    //{WeaponType.Wand, 5 },
    //{WeaponType.Magic, 8 }
    private CharacterScript chara;
    private List<GameObject> contains = new List<GameObject>();

    private void Start()
    {
        chara = gameObject.transform.root.gameObject.GetComponent<CharacterScript>();
        chara.distanceChecker = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        contains.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        contains.Remove(other.gameObject);
    }
    public bool Contains(GameObject target)
    {
        if (contains.Contains(target))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
