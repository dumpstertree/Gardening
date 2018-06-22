using System.Collections.Generic;
using UnityEngine;

namespace Eden.Properties {
	
	public class Magnetic : MonoBehaviour {

	
		// ************** Public ******************
		
		public delegate void ReachedTargetEvent ( Magnet magnet );
		public ReachedTargetEvent OnReachedTarget;

		public void ReachedTarget( Magnet magnet ) {

			FireReachedTargetEvent( magnet );
		}


		// ************** Private ******************
		
		[SerializeField] private float _speed = 1f;
		
		private List<Magnet> _activeMagnets;
		

		private void Awake () {

			_activeMagnets = new List<Magnet>();
		}
		private void Update () {

			if ( _activeMagnets.Count > 0 ) {
			
				var magnet = GetClosest();
				var distance = Vector3.Distance( magnet.transform.position, transform.position );

				if ( distance < magnet.ReachRange ){
					FireReachedTargetEvent( magnet );
				} else {
					var speed = _speed/distance;
					transform.position = Vector3.Lerp( transform.position, magnet.transform.position, speed );
				}
			}
		}
		private void OnTriggerEnter ( Collider collision ){

			var magnet = collision.GetComponentInChildren<Magnet>();

			if ( magnet != null ) {
				_activeMagnets.Add( magnet );
			}
		}
		private void OnTriggerExit ( Collider collision ){

			var magnet = collision.GetComponentInChildren<Magnet>();
				
			if ( magnet != null && _activeMagnets.Contains( magnet )) {
				_activeMagnets.Remove( magnet );
			}
		}
		
		private Magnet GetClosest () {
			
			Magnet best = null;
			float bestDist = Mathf.Infinity;
			
			foreach ( Magnet m in _activeMagnets ) {
				
				var dist = Vector3.Distance( m.transform.position, transform.position );
				if ( dist < bestDist ) {
					best = m;
					bestDist = dist;
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
