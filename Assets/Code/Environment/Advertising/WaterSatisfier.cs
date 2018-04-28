using System.Collections;
using System.Collections.Generic;
using Code.Characters.Doods;
using Code.Characters.Doods.Needs;
using Code.Environment.Advertising;
using UnityEngine;

public class WaterSatisfier : Satisfier {
	
	public override void InteractWith (Dood dood) {
		dood.Comps.Status.GetNeedOfType(Satisfies()).Satisfy();
	}
	public override NeedType Satisfies () { return NeedType.Water;}
}
