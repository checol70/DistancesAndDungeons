using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedManaPotionScript : DroppedConsumableScript {

	// Use this for initialization
	void Start () {
        ConsumableType = ConsumableEnum.ManaPotion;
	}
}
