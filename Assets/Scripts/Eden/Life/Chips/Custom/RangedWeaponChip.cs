using Eden.Interactors.Ranged;
using UnityEngine;

namespace Eden.Life.Chips {
	
	public class RangedWeaponChip : Dumpster.Core.Life.Chip<Eden.Model.Life.Visual>, ICanUseRangedWeapon {

		
		// ************** Data ***************
		
		private enum ForwardDirecton {
			Local,
			Camera
		}


		// ************** Public ***************

		
		public float GetAimAssistRange  () {
			return _aimAssistRange;
		}
		public Vector3 GetSpawnLocation () {
			return _projectileSpawner.position;
		}
		public Collider[] GetForbiddenColliders () {
			return BlackBox.GetComponentsInChildren<Collider>();
		}
		public Vector3 GetLookingDirection () {

			switch( _forwardDirection ) {
				case ForwardDirecton.Local: return _projectileSpawner.forward;
				case ForwardDirecton.Camera: return Camera.main.transform.forward;
			}

			return Vector3.zero;
		}


		// ************** Private ***************


		[SerializeField] private float _aimAssistRange = 3f;
		[SerializeField] private Transform _projectileSpawner;
		[SerializeField] private ForwardDirecton _forwardDirection;
	}
}