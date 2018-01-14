using UnityEngine;

namespace Gun {

	public class Component : UiPanel {

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
				
		[SerializeField] private bool _active;
		[SerializeField] private GameObject _outline;
		[SerializeField] private CanvasGroup _canvasGroup;

		private const float SNAPPING = 100;
		
		private GunCraftingPanel _panel;
		private bool _set;
		private Reciever[]_recievers;
		private Projector[] _projectors;
		private Collider[] _colliders;

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
		private void Move () {

			var x = Mathf.Round( transform.parent.InverseTransformPoint( Input.mousePosition ).x / SNAPPING ) * SNAPPING + 50;
			var y = Mathf.Round( transform.parent.InverseTransformPoint( Input.mousePosition ).y / SNAPPING ) * SNAPPING + 50;
			var z = transform.localPosition.z;
			
			transform.localPosition = new Vector3( x, y, z );

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

			// input commands
			OnPointerDownEvent += UnSet;
			OnPointerIsStillDownEvent += Move;
			OnPointerUpEvent += () => {
				if ( CanSet ){
					Set(); 
				}
			};
		}
		private void Start () {

			Reset();
			Set();
		}
		protected override void Update() {
		
			base.Update();

			if( Input.GetKeyDown( KeyCode.LeftArrow ) && MouseIsOver ) {
				
				UnSet();
				transform.Rotate( Vector3.forward, 90f );

				if ( CanSet ) {
					Set();
				}
			}

			if( Input.GetKeyDown( KeyCode.RightArrow ) && MouseIsOver ) {
				
				UnSet();
				transform.Rotate( Vector3.forward, -90f );

				if ( CanSet ) {
					Set();
				}
			}
		}
	}
}