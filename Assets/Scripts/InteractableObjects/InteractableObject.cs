using UnityEngine;

namespace Interactable {

	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody))]
	public class InteractableObject : MonoBehaviour {

		// ***************** PUBLIC *******************

		public Transform InteractablePivot {
			get { return _interactablePivot ? _interactablePivot : transform; }
		}
		public bool Hitable { 
			get{ return _hitDelegate != null; } 
		}
		public bool Plantable {
			get{ return _plantDelegate != null; }
		}
		public bool Feedable { 
			get{ return _feedDelegate != null; } 
		}
		public bool Interactable { 
			get{ return _interactDelegate != null; }
		}

		public Component.Hitable HitDelegate {
			get { return _hitDelegate; }
		}
		public Component.Plantable PlantDelegate {
			get { return _plantDelegate; }
		}
		public Component.Feedable FeedDelegate {
			get { return _feedDelegate; }
		}
		public Component.Interactable InteractDelegate {
			get { return _interactDelegate; }
		}
			

		// ***************** PRIVATE ********************

		[Header( "Interactable Properties" )]
		[SerializeField] private Transform _interactablePivot;

		private Component.Hitable _hitDelegate;
		private Component.Plantable _plantDelegate;
		private Component.Feedable _feedDelegate;
		private Component.Interactable _interactDelegate;

		// ***********************************************

		protected virtual void Awake () {

			_hitDelegate = GetComponent<Component.Hitable>();
			_plantDelegate = GetComponent<Component.Plantable>();
			_feedDelegate = GetComponent<Component.Feedable>();
			_interactDelegate = GetComponent<Component.Interactable>();
		}
	}
}