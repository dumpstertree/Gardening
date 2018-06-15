using UnityEngine;
using System.Collections;

namespace Eden.UI.Elements.GunCrafting {

	public class Part : MonoBehaviour, Eden.UI.Subpanels.GunCrafting.IPartGraphSubpanel {

		private UI.Subpanels.GunCrafting.PartGraph _delegate;
		void UI.Subpanels.GunCrafting.IPartGraphSubpanel.Inject (  UI.Subpanels.GunCrafting.PartGraph  subpanel ){
			_delegate = subpanel;
		}
     	
     	// This is bad ^^^^^^^^
		// **************************

		public Model.Gun.Stats Stats;

		[SerializeField] private string _prefabName;
		public string PrefabName { get { return _prefabName; } }

		[SerializeField] private Sprite _sprite;
		public Sprite Sprite { get { return _sprite; } }

		// **************************
		
		public void InitAtLocation () {

			// find components
			_recievers = GetComponentsInChildren<UI.Elements.GunCrafting.Reciever>();
			_projectors = GetComponentsInChildren<UI.Elements.GunCrafting.Projector>();
			_colliders = GetComponentsInChildren<UI.Elements.GunCrafting.Collider>();
			// _animator = GetComponent<ObjectAnimator>();

			// set position
			_targetPos = transform.localPosition;

			// set dragging
			_dragging = false;

			// set state
			Reset();
			if( CanSet ) HandleHasBeenSet();
		}
		public void InitMoving () {
			
			// find components
			_recievers = GetComponentsInChildren<UI.Elements.GunCrafting.Reciever>();
			_projectors = GetComponentsInChildren<UI.Elements.GunCrafting.Projector>();
			_colliders = GetComponentsInChildren<UI.Elements.GunCrafting.Collider>();
			// _animator = GetComponent<ObjectAnimator>();

			// set position
			transform.position = UnityEngine.Input.mousePosition;
			_targetPos = transform.localPosition;

			// set dragging
			_dragging = true;
			
			//set state
			Reset();
			HandleHasBeenUnset();

			// start moving
			_movementCoroutine = StartCoroutine( Move() );
		}
		public void Recieve () {

			if ( !_active ) {
			
				_active = true;

				foreach ( UI.Elements.GunCrafting.Projector p in _projectors ){
					
					var projectorX = _delegate.GetX( p.transform.position );
					var projectorY = _delegate.GetY( p.transform.position );
					_delegate.Project( projectorX, projectorY );
				}

				SetStateVisual();
			}
		}
		public void Reset () {

			_active = false;
			
			SetStateVisual();
		}

		public delegate void HasBeenSetEvent( Part component, UI.Elements.GunCrafting.Collider[] colliders, UI.Elements.GunCrafting.Projector[] projectors, UI.Elements.GunCrafting.Reciever[] recievers );
		public HasBeenSetEvent HasBeenSet;

		public delegate void HasBeenUnsetEvent( UI.Elements.GunCrafting.Collider[] colliders, UI.Elements.GunCrafting.Projector[] projectors, UI.Elements.GunCrafting.Reciever[] recievers );
		public HasBeenUnsetEvent HasBeenUnset;

		// **************************
				
		private const float SNAPPING = 100;
		private const float ROTATION_TIME = 0.1f;
		private const float MOVEMENT_LERP = 0.5f;

		//private ObjectAnimator _animator;
		private UI.Elements.GunCrafting.Reciever[]_recievers;
		private UI.Elements.GunCrafting.Projector[] _projectors;
		private UI.Elements.GunCrafting.Collider[] _colliders;

		private bool _active;
		private bool _set;
		private bool _lockInput;
		private bool _dragging;
		private Vector3 _targetPos;

		private Coroutine _movementCoroutine;

		// **************************

		private bool CanSet {
			
			get {
			
				foreach ( UI.Elements.GunCrafting.Collider c in _colliders ) {
					
					var x = _delegate.GetX( c.transform.position );
					var y = _delegate.GetY( c.transform.position );
					if ( !_delegate.GetAvailable( x, y ) ) {
						return false;
					}
				}

				foreach ( UI.Elements.GunCrafting.Projector p in _projectors ) {

					var x = _delegate.GetX( p.transform.position );
					var y = _delegate.GetY( p.transform.position );
					if ( !_delegate.GetAvailable( x, y ) ) {
						return false;
					}
				}

				foreach ( UI.Elements.GunCrafting.Reciever r in _recievers ) {
					
					var x = _delegate.GetX( r.transform.position );
					var y = _delegate.GetY( r.transform.position );
					if ( !_delegate.GetAvailable( x, y ) ) {
						return false;
					}
				}

				return true;
			}
		}
		private bool MouseIsOver {
			
			get {
				foreach ( UI.Elements.GunCrafting.Collider c in _colliders ) {
					 if ( c.MouseIsOver ) { return true; }
				}

				return false;
			}
		}

		// **************************

		private void Update () {
		
			// rotate left
			if( UnityEngine.Input.GetKeyDown( KeyCode.LeftArrow ) ){
				LeftKeyDown();
			}

			// rotate right
			if( UnityEngine.Input.GetKeyDown( KeyCode.RightArrow ) ) {
				RightKeyDown();
			}

			// on mouse down
			if ( UnityEngine.Input.GetMouseButtonDown( 0 ) ) {
				MouseDown();
			} 

			// on mouse up
			if ( UnityEngine.Input.GetMouseButtonUp( 0 ) ) {
				MouseUp();
			}

			// move controller
			transform.localPosition = Vector3.Lerp( transform.localPosition, _targetPos, MOVEMENT_LERP );
		}
		private void MouseDown () {

			if ( MouseIsOver ) { 
					
				_dragging = true;
				
				HandleHasBeenUnset();
				_movementCoroutine = StartCoroutine( Move() );
			}
		}
		private void MouseUp () {
			
			if ( _dragging ) {

				_dragging = false;

				if ( _movementCoroutine != null ){ 
					StopCoroutine( _movementCoroutine ); 
				}
				
				if ( CanSet ) {
					HandleHasBeenSet();
				} else {
					_delegate.RemovePartFromGraph( this );
				}
			}
		}
		private void LeftKeyDown () {
			
			if( MouseIsOver && !_lockInput ) {
				StartCoroutine( RotateLeft() );
			}
		}
		private void RightKeyDown () {

			if( MouseIsOver && !_lockInput ) {
				StartCoroutine( RotateRight() );
			}
		}

		// ************************** 

		private void HandleHasBeenSet () {
			
			_set = true;
			SetStateVisual();
			
			if ( HasBeenSet != null ){

				HasBeenSet( this, _colliders, _projectors, _recievers );
			}
		}
		private void HandleHasBeenUnset () {

			_set = false;
			SetStateVisual();

			if ( HasBeenUnset != null ){

				HasBeenUnset( _colliders, _projectors, _recievers );
			}
		}

		// **************************
		
		private void SetStateVisual () {

			// if ( _set ){
			// 	_animator.Animate( "SET" );
			// } else {
			// 	if ( CanSet ) {
			// 		_animator.Animate( "UNSET_VALID" );
			// 	} else {
			// 		_animator.Animate( "UNSET_INVALID" );
			// 	}
			// }

			// if ( _active ) {
			// 	_animator.Animate( "ACTIVE" );
			// }
			// else {
			// 	_animator.Animate( "INACTIVE" );
			// }
		}

		// **************************

		private IEnumerator RotateLeft () {
			
			_lockInput = true;

			HandleHasBeenUnset();

			var targetRotation = transform.rotation * Quaternion.AngleAxis( 90, Vector3.forward );
			for (float i = 0; i < ROTATION_TIME; i += Time.deltaTime ){
				var frac = i/ROTATION_TIME;

				transform.rotation =  Quaternion.Slerp( transform.rotation, targetRotation, frac);
				yield return null;
			}

			transform.rotation = targetRotation;

			if ( CanSet ) {
				HandleHasBeenSet();
			}

			_lockInput = false;
		}
		private IEnumerator RotateRight () {

			_lockInput = true;

			HandleHasBeenUnset();

			var targetRotation = transform.rotation * Quaternion.AngleAxis( -90, Vector3.forward );
			for (float i = 0; i < ROTATION_TIME; i += Time.deltaTime ){
				var frac = i/ROTATION_TIME;

				transform.rotation =  Quaternion.Slerp( transform.rotation, targetRotation, frac);
				yield return null;
			}

			transform.rotation = targetRotation;

			if ( CanSet ) {
				HandleHasBeenSet();
			}

			_lockInput = false;
		}
		private IEnumerator Move () {

			while ( true ) {

				var inputPos = UnityEngine.Input.mousePosition;
				var x = Mathf.Round( transform.parent.InverseTransformPoint( inputPos ).x / SNAPPING ) * SNAPPING + 50;
				var y = Mathf.Round( transform.parent.InverseTransformPoint( inputPos ).y / SNAPPING ) * SNAPPING + 50;
				var z = transform.localPosition.z;
				
				_targetPos = new Vector3( x, y, z ); 

				SetStateVisual();
				yield return null;
			}
		}
	}
}