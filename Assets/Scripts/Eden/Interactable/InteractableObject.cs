using UnityEngine;

namespace Eden.Interactable {

	// [RequireComponent(typeof(Animator))]
	// [RequireComponent(typeof(Rigidbody))]
	public class InteractableObject : MonoBehaviour {


		// ***************** PUBLIC *******************

		public bool Active {
			set{ gameObject.SetActive( value ); }
			get{ return gameObject.activeSelf; }
		}


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
		public bool Actionable { 
			get{ return _actionDelegate != null; }
		}

		public Interactable.Hitable HitDelegate {
			get { return _hitDelegate; }
		}
		public Interactable.Actionable ActionDelegate {
			get{ return _actionDelegate; }
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
		private Actionable _actionDelegate;
		// private Component.Plantable _plantDelegate;
		// private Component.Feedable _feedDelegate;
		// private Component.Interactable _interactDelegate;

		// ***********************************************

		protected virtual void Awake () {

			_hitDelegate = GetComponent<Hitable>();
			_actionDelegate = GetComponent<Actionable>();
			// _plantDelegate = GetComponent<Component.Plantable>();
			// _feedDelegate = GetComponent<Component.Feedable>();
			// _interactDelegate = GetComponent<Component.Interactable>();
		}
	}
}