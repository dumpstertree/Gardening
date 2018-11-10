using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {

	public class CanUseMeleeItems : Characteristic {

		
		// ************** Public ***************
		
		public Vector3 GetSpawnLocation () {
			return _meleeSpawner.position;
		}
		public Collider[] GetForbiddenColliders () {
			return _actor.GetComponentsInChildren<Collider>();
		}
		public Vector3 GetLookingDirection () {
			return _meleeSpawner.forward;
		}
		public void SetSwingActive ( bool active, int combo ) {

			// if ( combo == 0 ) { _characterController.IsSwingingCombo1 = active; }
			// if ( combo == 1 ) { _characterController.IsSwingingCombo2 = active; }

		}
		public void SetSwingProgress ( float progress, int combo ) {

			// if ( combo == 0 ) { _characterController.SwingProgressCombo1 = progress; }
			// if ( combo == 1 ) { _characterController.SwingProgressCombo2 = progress; }
		}


		[SerializeField] private Transform _meleeSpawner;
		// [SerializeField] private ThirdPersonCharacterController _characterController;
	}
}