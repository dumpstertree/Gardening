using UnityEngine;

namespace Eden.Interactors.Ranged {
	
	public interface ICanUseRangedWeapon {

		Vector3 GetSpawnLocation ();
		Vector3 GetLookingDirection ();
		Collider[] GetForbiddenColliders ();
		float GetAimAssistRange  ();
	}
}