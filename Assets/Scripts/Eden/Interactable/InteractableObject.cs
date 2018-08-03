using UnityEngine;

namespace Eden.Interactable {

	public class InteractableObject : MonoBehaviour {


		// ***************** PUBLIC *******************

		public bool Active {
			get { return _active; }
			set { _active = value; }
		}


		public Transform InteractablePivot {
			get { return _interactablePivot ? _interactablePivot : transform; }
		}
		public bool Hitable { 
			get{ return _hitDelegate != null; } 
		}
		public bool Actionable { 
			get{ return _actionDelegate != null; }
		}

		public Hitable HitDelegate {
			get { return _hitDelegate; }
		}
		public Actionable ActionDelegate {
			get{ return _actionDelegate; }
		}

		// ***************** PRIVATE ********************

		[Header( "Interactable Properties" )]
		[SerializeField] private bool _active = true;
		[SerializeField] private Transform _interactablePivot;

		private Hitable _hitDelegate;
		private Actionable _actionDelegate;


		// ***********************************************

		protected virtual void Awake () {

			_hitDelegate = GetComponent<Hitable>();
			_actionDelegate = GetComponent<Actionable>();
		}
	}
}