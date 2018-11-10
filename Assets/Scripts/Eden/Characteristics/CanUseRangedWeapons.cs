using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {

	public class CanUseRangedWeapons : Characteristic {

			
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
			return _actor.GetComponentsInChildren<Collider>();
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