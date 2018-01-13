using UnityEngine;
using UnityEngine.EventSystems;

namespace Gun {

	public class Component : UiPanel {


		// **************************

		public void Reset () {

			_active = false;
			SetActiveVisual();
		}
		public void Set () {

			if( !_set ) {

				_set = true;
				SetStateVisual();

				var panel = GetComponentInParent<GunCraftingPanel>();

				// add colliders to graph
				foreach ( GunCrafting.Collider c in _colliders ) {
					var slot = panel.GetSlot( c.X, c.Y );
					if (slot != null){ slot.Occupied = true; }
					if (slot != null){ slot.Component = this; }
				}
				
				// add projectors to graph
				foreach ( Projector p in _projectors ) {
					var slot = panel.GetSlot( p.Row, p.Collumn );
					if (slot != null){ slot.Projector = p; }
				}
				
				// add recievers to graph
				foreach ( Reciever r in _recievers ) {
					var slot = panel.GetSlot( r.Row, r.Collumn );
					if (slot != null){ slot.Reciever = r; }
				}

				// Recalculate the path
				panel.SetNeedsRecalulatePath();
			}
		}
		public void UnSet () {

			if ( _set ) {

				_set = false;
				SetStateVisual();

				var panel = GetComponentInParent<GunCraftingPanel>();
				
				// remove colliders from graph
				foreach ( GunCrafting.Collider c in _colliders ) {
					var slot = panel.GetSlot( c.X, c.Y );
					if (slot != null){ slot.Occupied = false; }
					if (slot != null){ slot.Component = null; }
				}

				// remove projectors from graph
				foreach ( Projector p in _projectors ) {
					var slot = panel.GetSlot( p.Row, p.Collumn );
					if (slot != null){ slot.Projector = null; }
				}

				// remove recievers from graph
				foreach ( Reciever r in _recievers ) {
					var slot = panel.GetSlot( r.Row, r.Collumn );
					if (slot != null){ slot.Reciever = null; }
				}

				// Recalculate the path
				panel.SetNeedsRecalulatePath();
			}
		}
		public void Move () {

			var x = Mathf.Round( transform.parent.InverseTransformPoint( Input.mousePosition ).x / SNAPPING ) * SNAPPING + 50;
			var y = Mathf.Round( transform.parent.InverseTransformPoint( Input.mousePosition ).y / SNAPPING ) * SNAPPING + 50;
			var z = transform.localPosition.z;
			
			transform.localPosition = new Vector3( x, y, z );

			SetStateVisual();
		}
		public bool CanSet {
			get{
				var panel = GetComponentInParent<GunCraftingPanel>();
				foreach ( GunCrafting.Collider c in _colliders ) {
				
					var slot = panel.GetSlot( c.X, c.Y );
						
					if ( slot == null ) {
						return false;
					}

					if ( slot.Occupied ) {
						return false;
					}
				}

				return true;
			}
		}


		// **************************
				
		[SerializeField] private bool _active;
		[SerializeField] private GameObject _outline;
		[SerializeField] private CanvasGroup _canvasGroup;

		private const float SNAPPING = 100;
		
		private bool _set;
		private Reciever[]_recievers;
		private Projector[] _projectors;
		private GunCrafting.Collider[] _colliders;
		
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

		private void HandleOnRecieve () {

			if ( !_active ) {
			
				_active = true;
				SetActiveVisual();

				foreach ( Projector p in _projectors ){
					p.Project();
				}
			}
		}
		private void HandleOnProject () {
		}

		// **************************

		private void Awake () {
			
			_recievers = GetComponentsInChildren<Reciever>();
			_projectors = GetComponentsInChildren<Projector>();
			_colliders = GetComponentsInChildren<GunCrafting.Collider>();

			foreach ( Projector p in _projectors ){
				p.OnProject += HandleOnProject;
			}

			foreach ( Reciever r in _recievers ){
				r.OnRecieve += HandleOnRecieve;
			}
		}
		private void Start () {

			Reset();
			Set();
		}

		public enum Connection {
			None,
			Circle,
			Square,
		}
	}
}