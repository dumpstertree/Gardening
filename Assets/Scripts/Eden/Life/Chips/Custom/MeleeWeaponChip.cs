using UnityEngine;
using Dumpster.Controllers;

namespace Eden.Life.Chips {
	
	public class MeleeWeaponChip : Dumpster.Core.Life.Chip<Eden.Model.Life.Visual>, ICanUseMeleeWeapon {


		// ************** Public ***************

		
		public Vector3 GetSpawnLocation () {
			return _meleeSpawner.position;
		}
		public Collider[] GetForbiddenColliders () {
			return BlackBox.GetComponentsInChildren<Collider>();
		}
		public Vector3 GetLookingDirection () {
			return _meleeSpawner.forward;
		}
		public void SetSwingActive ( bool active, int combo ) {

			if ( combo == 0 ) { _characterController.IsSwingingCombo1 = active; }
			if ( combo == 1 ) { _characterController.IsSwingingCombo2 = active; }

		}
		public void SetSwingProgress ( float progress, int combo ) {

			if ( combo == 0 ) { _characterController.SwingProgressCombo1 = progress; }
			if ( combo == 1 ) { _characterController.SwingProgressCombo2 = progress; }
		}


		// ************** Private ***************


		[SerializeField] private Transform _meleeSpawner;
		[SerializeField] private ThirdPersonCharacterController _characterController;
	}
}