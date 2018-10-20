using UnityEngine;

public interface ICanUseMeleeWeapon {

	Vector3 GetSpawnLocation ();
	Vector3 GetLookingDirection ();
	Collider[] GetForbiddenColliders ();
	void SetSwingActive ( bool active, int comboNumber );
	void SetSwingProgress ( float progress, int comboNumber );
}
