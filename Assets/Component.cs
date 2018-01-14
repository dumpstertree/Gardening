using UnityEngine;
using System.Collections;

namespace Gun {

	public class Component : UiPanel {

		public string PrefabName {
			get { return _prefabName; }
		}
		public void Recieve () {

			if ( !_active ) {
			
				_active = true;
				SetActiveVisual();

				var panel = GetComponentInParent<GunCraftingPanel>();
				foreach ( Projector p in _projectors ){
					
					var projectorX = panel.GetX( p.transform.position );
					var projectorY = panel.GetY( p.transform.position );
					panel.Project( projectorX, projectorY );
				}
			}
		}
		public void Reset () {

			_active = false;
			SetActiveVisual();
		}

		// **************************
				
		[SerializeField] private string _prefabName;
		[SerializeField] private bool _active;
		[SerializeField] private GameObject _outline;
		[SerializeField] private CanvasGroup _canvasGroup;

		private const float SNAPPING = 100;
		private const float ROTATION_TIME = 0.1f;
		private const float MOVEMENT_LERP = 0.5f;
		
		private GunCraftingPanel _panel;
		private Reciever[]_recievers;
		private Projector[] _projectors;
		private Collider[] _colliders;
		private bool _set;
		private bool _lockInput;
		private Vector3 _targetPos;


		private bool CanSet {
			get {

				var panel = GetComponentInParent<GunCraftingPanel>();

				foreach ( Collider c in _colliders ) {
					
					var x = panel.GetX( c.transform.position );
					var y = panel.GetY( c.transform.position );
					if ( !panel.GetAvailable( x, y ) ) {
						return false;
					}
				}

				foreach ( Projector p in _projectors ) {

					var x = panel.GetX( p.transform.position );
					var y = panel.GetY( p.transform.position );
					if ( !panel.GetAvailable( x, y ) ) {
						return false;
					}
				}

				foreach ( Reciever r in _recievers ) {
					
					var x = panel.GetX( r.transform.position );
					var y = panel.GetY( r.transform.position );
					if ( !panel.GetAvailable( x, y ) ) {
						return false;
					}
				}

				return true;
			}
		}
		private bool MouseIsOver {
			get {
				
				var panel = GetComponentInParent<GunCraftingPanel>();

				foreach ( Collider c in _colliders ) {
					 if ( c.MouseIsOver ) { return true; }
				}

				return false;
			}
		}
		
		// ************************** 

		private void Set () {

			if( !_set ) {

				_set = true;
				SetStateVisual();

				// remove colliders from graph
				foreach ( Collider c in _colliders ) {
					
					var x = _panel.GetX( c.transform.position );
					var y = _panel.GetY( c.transform.position );
					_panel.SetComponent( x, y, this );
				}

				// remove projectors from graph
				foreach ( Projector p in _projectors ) {
					_panel.AddProjector( p );
				}

				// remove recievers from graph
				foreach ( Reciever r in _recievers ) {
					_panel.AddReciever( r );
				}

				// Recalculate the path
				_panel.SetNeedsRecalulatePath();
			}
		}
		private void UnSet () {

			if ( _set ) {

				_set = false;
				SetStateVisual();
				
				// remove colliders from graph
				foreach ( Collider c in _colliders ) {
					
					var x = _panel.GetX( c.transform.position );
					var y = _panel.GetY( c.transform.position );
					_panel.SetComponent( x, y, null );
				}

				// remove projectors from graph
				foreach ( Projector p in _projectors ) {
					_panel.RemoveProjector( p );
				}

				// remove recievers from graph
				foreach ( Reciever r in _recievers ) {
					_panel.RemoveReciever( r );
				}

				// Recalculate the path
				_panel.SetNeedsRecalulatePath();
			}
		}
		private void Move ( Vector3 clickOffset ) {
			
			var inputPos = Input.mousePosition;
			var x = Mathf.Round( transform.parent.InverseTransformPoint( inputPos ).x / SNAPPING ) * SNAPPING + 50;
			var y = Mathf.Round( transform.parent.InverseTransformPoint( inputPos ).y / SNAPPING ) * SNAPPING + 50;
			var z = transform.localPosition.z;
			
			_targetPos = new Vector3( x, y, z ); 

			SetStateVisual();
		}

		// **************************
		
		private void SetStateVisual () {

			if ( _set ){
				SetSetVisual();
			} else {
				if ( CanSet ) {
					SetUnsetValidVisual();
				} else {
					SetUnsetInvalidVisual();
				}
			}
		}
		private void SetSetVisual () {

			_canvasGroup.alpha = 1.0f;
		}
		private void SetUnsetValidVisual () {

			_canvasGroup.alpha = 0.8f;
		}
		private void SetUnsetInvalidVisual () {

			_canvasGroup.alpha = 0.2f;
		}
		private void SetActiveVisual () {

			_outline.SetActive( _active );
		}

		// **************************

		private void Awake () {
			
			_panel = GetComponentInParent<GunCraftingPanel>();
			_recievers = GetComponentsInChildren<Reciever>();
			_projectors = GetComponentsInChildren<Projector>();
			_colliders = GetComponentsInChildren<Collider>();

			var clickOffset = Vector3.zero;
			// input commands
			OnPointerDownEvent += () => { 
				clickOffset = transform.position - Input.mousePosition;
				UnSet();
			};
			OnPointerIsStillDownEvent += () => {
				Move( clickOffset );
			}; 
			OnPointerUpEvent += () => {
				if ( CanSet ){
					Set(); 
				}
			};
		}
		private void Start () {

			Reset();
			_targetPos = transform.localPosition;
			Set();
		}
		protected override void Update() {
		
			base.Update();

			if( Input.GetKeyDown( KeyCode.LeftArrow ) && MouseIsOver && !_lockInput ) {
				
				StartCoroutine( RotateLeft() );
			}

			if( Input.GetKeyDown( KeyCode.RightArrow ) && MouseIsOver && !_lockInput ) {
				
				StartCoroutine( RotateRight() );
			}

			transform.localPosition = Vector3.Lerp( transform.localPosition, _targetPos, MOVEMENT_LERP );
		}

		// **************************

		private IEnumerator RotateLeft(){
			
			_lockInput = true;

			UnSet();

			var targetRotation = transform.rotation * Quaternion.AngleAxis( 90, Vector3.forward );
			for (float i = 0; i < ROTATION_TIME; i += Time.deltaTime ){
				var frac = i/ROTATION_TIME;

				transform.rotation =  Quaternion.Slerp( transform.rotation, targetRotation, frac);
				yield return null;
			}

			transform.rotation = targetRotation;

			if ( CanSet ) {
				Set();
			}

			_lockInput = false;
		}
		private IEnumerator RotateRight(){

			_lockInput = true;

			UnSet();

			var targetRotation = transform.rotation * Quaternion.AngleAxis( -90, Vector3.forward );
			for (float i = 0; i < ROTATION_TIME; i += Time.deltaTime ){
				var frac = i/ROTATION_TIME;

				transform.rotation =  Quaternion.Slerp( transform.rotation, targetRotation, frac);
				yield return null;
			}

			transform.rotation = targetRotation;

			if ( CanSet ) {
				Set();
			}

			_lockInput = false;
		}
	}
}