using System.Collections.Generic;
using UnityEngine;

namespace UI.GunCrafting.Subpanel {
	
	public class PartGraph : MonoBehaviour {

		
		// ************ PUBLIC ***************

		public delegate void CraftedGunChangeEvent ( CraftedGun newGun );
		public CraftedGunChangeEvent CraftedGunChanged;
		

		// from panel
		public void Clear () {

			// remove all existing parts from the graph
			for ( int i = _partsOnGraph.Count-1; i >= 0; i-- ) {
				RemovePartFromGraph( _partsOnGraph[ i ] );
			}

			// clear the graph model
			_partGraph.ClearGraph();

			// reset the lists
			_gunComponents.Clear();
			_partsOnGraph.Clear();

			// re-add all the base projectors
			AddBaseProjectors();
		}
		public void AddPartToGraph ( CraftedGun.Component part, bool moving = false ) {
		
			// create an instance based on the model name
			var prefab = Resources.Load( part.PrefabName ) as GameObject;
			var instance = Instantiate( prefab );
				
			// set transform of new part controller
			instance.transform.SetParent( _content, false );
			instance.transform.rotation = Quaternion.Euler( part.Rotation );
			instance.transform.position = part.Position;

			// add new part to the object graph
			var component = instance.GetComponent<Gun.Component>();
			_partsOnGraph.Add( component );

			// inject
			((IPartGraphSubpanel)component).Inject( this );

			// set actions
			component.HasBeenSet += SetPart;
			component.HasBeenUnset += UnsetPart;

			// if moving start
			if ( moving ){ component.InitMoving();
			} else { component.InitAtLocation(); }

			// calculate new path
			SetNeedToRecalulatePath();
		}
		public void RemovePartFromGraph ( Gun.Component part ) {
			
			// remove from graph
			_partsOnGraph.Remove( part );

			// if part is on the graph but not part of the gun add it back to the parts list
			if ( !_gunComponents.Contains( part ) ){
				Game.Area.LoadedPlayer.GunParts.Add( new CraftedGun.Component( part ) );
			}

			// destroy the part controller
			Destroy( part.gameObject );

			// calculate new path
			SetNeedToRecalulatePath();
		}


		// from subpanel
		public int GetX ( Vector3 pos ){

			var contentLocalX = _content.InverseTransformPoint( pos ).x - SNAPPING/2;
			var snappedX = Mathf.RoundToInt( (contentLocalX+_content.sizeDelta.x/2) / SNAPPING);
			
			return snappedX;
		}
		public int GetY ( Vector3 pos ){
			
			var contentLocalY = _content.InverseTransformPoint( pos ).y - SNAPPING/2;
			var snappedY = Mathf.RoundToInt( (contentLocalY+_content.sizeDelta.y/2) / SNAPPING);
			
			return snappedY;
		}
		public bool GetAvailable ( int x, int y ) {

			return _partGraph.GetAvailable( x, y );
		} 		
		public void Project( int x, int y ) {

			foreach ( Gun.Projector projector in _partGraph.GetProjectors( x, y ) ) {		

				// if projector exists project forward
				var projectedPos = projector.transform.position + ( projector.transform.up * PROJECTOR_LENGTH );
				var projectedX = GetX( projectedPos );
				var projectedY = GetY( projectedPos );			
				
				foreach ( Gun.Reciever reciever in _partGraph.GetRecievers( projectedX, projectedY) ) {
					
					if ( reciever && projector.Connection == reciever.Connection ) {
						
						// if reciever exists reciever forward
						var recivedPos = reciever.transform.position + (reciever.transform.up * PROJECTOR_LENGTH);
						var recivedX = GetX( recivedPos );
						var recivedY = GetY( recivedPos );

						// if projector and reciever match activate the component
						if ( recivedX == x && recivedY == y ) {

							var component = _partGraph.GetComponent( projectedX, projectedY );
							component.Recieve();
							AddToPath( component );
						}
					}
				}
			}
		}


		// ************ PRIVATE ***************

		[SerializeField] private RectTransform _content;
		[SerializeField] private List<Gun.Projector> _baseProjectors;

		private const float SNAPPING = 100;	
		private const int PROJECTOR_LENGTH = 100;

		private bool _recalculatePath;
		private List<Gun.Component> _gunComponents;	
		private List<Gun.Component> _partsOnGraph;
		private Model.PartGraph _partGraph;

		// **********************************
 		
 		private void Awake () {

			_partGraph = new Model.PartGraph( 10 );
			_gunComponents = new List<Gun.Component>();
			_partsOnGraph = new List<Gun.Component>();
		}
 		private void AddBaseProjectors () {
 			
 			foreach( Gun.Projector p in _baseProjectors ) {
			
				var x = GetX( p.transform.position );
				var y = GetY( p.transform.position );
				
				_partGraph.AddProjector( x, y, p );
			}
 		}
		private void AddToPath ( Gun.Component component ) {

			if ( !_gunComponents.Contains( component ) ) {
				_gunComponents.Add( component );
			}
		}
		private void ClearPath () {
			
			foreach ( Gun.Component c in _gunComponents ) {
				c.Reset();
			}

			_gunComponents.Clear();
		}
		private void SetPart( Gun.Component component, Gun.Collider[] colliders, Gun.Projector[] projectors, Gun.Reciever[] recievers ){

			// remove colliders from graph
			foreach ( Gun.Collider c in colliders ) {
				
				var x = GetX( c.transform.position );
				var y = GetY( c.transform.position );
				
				_partGraph.SetComponent( x, y, component );
			}

			// remove projectors from graph
			foreach ( Gun.Projector p in projectors ) {
				var x = GetX( p.transform.position );
				var y = GetY( p.transform.position );
				
				_partGraph.AddProjector( x, y, p );
			}

			// remove recievers from graph
			foreach ( Gun.Reciever r in recievers ) {

				var x = GetX( r.transform.position );
				var y = GetY( r.transform.position );

				_partGraph.AddReciever( x, y, r );
			}

			SetNeedToRecalulatePath();
		}
		private void UnsetPart( Gun.Collider[] colliders, Gun.Projector[] projectors, Gun.Reciever[] recievers ){

			// remove colliders from graph
			foreach ( Gun.Collider c in colliders ) {
				
				var x = GetX( c.transform.position );
				var y = GetY( c.transform.position );

				_partGraph.SetComponent( x, y, null );
			}

			// remove projectors from graph
			foreach ( Gun.Projector p in projectors ) {

				var x = GetX( p.transform.position );
				var y = GetY( p.transform.position );

				_partGraph.RemoveProjector( x, y,  p );
			}

			// remove recievers from graph
			foreach ( Gun.Reciever r in recievers ) {

				var x = GetX( r.transform.position );
				var y = GetY( r.transform.position );

				_partGraph.RemoveReciever( x, y, r );
			}

			SetNeedToRecalulatePath();
		}
		private  void SetNeedToRecalulatePath () {

			// if not already waiting to recalculate start wating
			if ( !_recalculatePath ){
				_recalculatePath = true;
				
				// wait for the end of frame so it only needs to be done once
				Game.Async.WaitForEndOfFrame( () => {
					
					// clear the old path
					ClearPath();

					// project from the base projectors to chain the activation
					foreach ( Gun.Projector p in _baseProjectors ){
						
						var projectorX = GetX( p.transform.position );
						var projectorY = GetY( p.transform.position );
						Project( projectorX, projectorY );
					}

					_recalculatePath = false;

					HandleCraftedGunChanged();
				});
			}
		}

		// **********************************

		private void HandleCraftedGunChanged () {

			if ( CraftedGunChanged != null ) {

				var components =  new List<CraftedGun.Component>();
				foreach ( Gun.Component c in _gunComponents ) {
					components.Add( new CraftedGun.Component( c ) );
				}

				CraftedGunChanged( new CraftedGun( components ) );
			}
		}
	}

	public interface IPartGraphSubpanel {

		void Inject ( UI.GunCrafting.Subpanel.PartGraph subpanel );
	}
}