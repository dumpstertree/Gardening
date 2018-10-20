using System.Collections.Generic;
using UnityEngine;

namespace Eden.Properties {
	
	[RequireComponent( typeof( SphereCollider ) )]
	public class Magnetic : MonoBehaviour, IProperty {

	
		// ************** Public ******************
		
		public delegate void ReachedTargetEvent ( Magnet magnet );
		public ReachedTargetEvent OnReachedTarget;

		public void ReachedTarget( Magnet magnet ) {

			FireReachedTargetEvent( magnet );
		}

		
		// ************** IProperty ******************
		
		void IProperty.Update () {

			if ( _activeMagnets.Count > 0 ) {
					
				var magnet = GetClosest ();
				var distance = Vector3.Distance( magnet.transform.position, transform.position );

				if ( distance < magnet.ReachRange ){
					FireReachedTargetEvent( magnet );
				} else {
					var speed = _speed/distance;
					_root.position = Vector3.Lerp( _root.position, magnet.transform.position, speed );
				}
			}
		}

		
		// ************** Private ******************
		
		[SerializeField] private Transform _root;
		[SerializeField] private float _speed = 1f;
		
		private List<PropertiesObject> _activeMagnets;
		

		private void Awake () {

			_activeMagnets = new List<PropertiesObject>();
		}
		private void OnTriggerEnter ( Collider collision ){

			var propertiesObject = collision.GetComponent<PropertiesObject>();

			if ( propertiesObject != null && propertiesObject.IsMagnet ) {
				_activeMagnets.Add( propertiesObject );
			}
		}
		private void OnTriggerExit ( Collider collision ){

			var propertiesObject = collision.GetComponent<PropertiesObject>();
				
			if ( propertiesObject != null && propertiesObject.IsMagnet && _activeMagnets.Contains( propertiesObject )) {
				_activeMagnets.Remove( propertiesObject );
			}
		}
		
		private Magnet GetClosest () {
			
			Magnet best = null;
			float bestDist = Mathf.Infinity;
			
			foreach ( PropertiesObject p in _activeMagnets ) {
				
				if ( !p.Active ) {
					continue;
				}
					
				if ( p.MagnetDelegate != null ) {
					
					var dist = Vector3.Distance( p.MagnetDelegate.transform.position, transform.position );
					if ( dist < bestDist ) {
						best = p.MagnetDelegate;
						bestDist = dist;
					}
				}
			}

			return best;
		}
		private void FireReachedTargetEvent ( Magnet magnet ) {

			if ( OnReachedTarget != null ) {
				OnReachedTarget( magnet );
			}
		}
	}
}
