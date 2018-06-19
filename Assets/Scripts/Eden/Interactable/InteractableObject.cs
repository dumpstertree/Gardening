using UnityEngine;

namespace Eden.Interactable {

	// [RequireComponent(typeof(Animator))]
	// [RequireComponent(typeof(Rigidbody))]
	public class InteractableObject : MonoBehaviour {

		// ***************** PUBLIC *******************

		public Transform InteractablePivot {
			get { return _interactablePivot ? _interactablePivot : transform; }
		}
		public bool Hitable { 
			get{ return _hitDelegate != null; } 
		}
		public bool Plantable {
			get{ return false; }
		}
		public bool Feedable { 
			get{ return false; } 
		}
		public bool Interactable { 
			get{ return false; }
		}

		public Interactable.Hitable HitDelegate {
			get { return _hitDelegate; }
		}
		// public Interactable.Component.Plantable PlantDelegate {
		// 	get { return _plantDelegate; }
		// }
		// public Interactable.Component.Feedable FeedDelegate {
		// 	get { return _feedDelegate; }
		// }
		// public Interactable.Component.Interactable InteractDelegate {
		// 	get { return _interactDelegate; }
		// }
			

		// ***************** PRIVATE ********************

		[Header( "Interactable Properties" )]
		[SerializeField] private Transform _interactablePivot;

		private Hitable _hitDelegate;
		// private Component.Plantable _plantDelegate;
		// private Component.Feedable _feedDelegate;
		// private Component.Interactable _interactDelegate;

		// ***********************************************

		protected virtual void Awake () {

			_hitDelegate = GetComponent<Hitable>();
			// _plantDelegate = GetComponent<Component.Plantable>();
			// _feedDelegate = GetComponent<Component.Feedable>();
			// _interactDelegate = GetComponent<Component.Interactable>();
		}
	}
}