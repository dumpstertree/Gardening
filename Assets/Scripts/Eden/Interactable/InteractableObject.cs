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
		public Transform UIAnchor {
			get { return _uiAnchor ? _uiAnchor : transform; }
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

		[Header( "Properties" )]
		[SerializeField] private bool _active = true;
		[SerializeField] private Transform _interactablePivot;
		[SerializeField] private Transform _uiAnchor;

		[Header( "Interactable" )]
		[SerializeField] private Hitable _hitDelegate;
		[SerializeField] private Actionable _actionDelegate;
	}
}
public interface IInteractable {
	
	void Update ();
}