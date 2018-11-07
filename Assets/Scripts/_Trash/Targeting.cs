// using System.Collections.Generic;
// using UnityEngine;
// using Eden.Properties;

// namespace Eden.Modules {

// 	[CreateAssetMenu( menuName = "Eden/Module/Targeting" )]
// 	public class Targeting : Dumpster.Core.Module {


// 		//  ********************* Public *********************


// 		public void RegisterTargetableForFrame( Targetable targetable ) {

// 			_targetableObjects.Add( targetable );
// 		}
// 		public Targetable GetTargetable ( Vector3 position, Vector3 forward, float maxRange ) {

// 			var targetables = new List<Targetable>( _lastFrameTargetableObjects );

// 			GetTargetablesInRange( targetables, position, forward, maxRange );
// 			GetVisibleTargetables( targetables, position );

// 			var best = GetBestTarget( targetables, position, forward );

// 			return best;
// 		}


// 		//  ********************* Protected *********************


// 		protected override void OnInit () {

// 			_lastFrameTargetableObjects = new List<Targetable>();
// 			_targetableObjects = new List<Targetable>();
// 		}


// 		//  ********************* Private *********************


// 		public LayerMask _layermask;
		
// 		private List<Targetable> _lastFrameTargetableObjects;
// 		private List<Targetable> _targetableObjects;
// 		private float _depthWeight = 1.0f;
// 		private float _angleWeight = 8.0f;
 		 


// 		private void LateUpdate () {

// 			ClearFramesTargetables ();
// 		}

		
// 		private void ClearFramesTargetables () {

// 			_lastFrameTargetableObjects = _targetableObjects;
// 			_targetableObjects = new List<Targetable>();
// 		}
// 		private void GetTargetablesInRange ( List<Targetable> targetables, Vector3 position, Vector3 forward, float maxRange ) {

// 			for ( int i=targetables.Count-1; i>=0; i-- ) {
				
// 				var targetable = targetables[ i ];
// 				var directionToTargetable = (targetable.transform.position - position).normalized;
// 				var angle = Vector3.Angle( forward, directionToTargetable );

// 				if ( angle > maxRange ) {
// 					targetables.Remove( targetable );
// 				}
// 			}
// 		}
// 		private void GetVisibleTargetables ( List<Targetable> targetables, Vector3 position ) {

// 			for ( int i=targetables.Count-1; i>=0; i-- ) {
				
// 				var targetable = targetables[ i ];
// 				var directionToTargetable = (targetable.transform.position - position).normalized;

// 				RaycastHit hit;
// 				if ( Physics.Raycast( position, directionToTargetable, out hit, Mathf.Infinity, _layermask ) ) {
					
// 					var distanceFromTarget = Vector3.Distance( hit.transform.position, targetable.transform.position );
// 					if( distanceFromTarget > 1f ) {

// 						targetables.Remove( targetable );
// 					}
// 				}
// 			}
// 		}
// 		private Targetable GetBestTarget ( List<Targetable> targetables, Vector3 position, Vector3 forward ) {

// 			Targetable bestTargetable = null;
// 			float smallestOffset = Mathf.Infinity;

// 			foreach ( Targetable targetable in targetables ) {

// 				var distance = Vector3.Distance( targetable.transform.position, position );
// 				var directionToTargetable = targetable.transform.position - position;
// 				var angle = Vector3.Angle( forward, directionToTargetable );

// 				var offset = (distance * _depthWeight) + (angle * _angleWeight);

// 				if ( offset < smallestOffset ) {
					
// 					bestTargetable = targetable;
// 					smallestOffset = offset;
// 				}
// 			}

// 			//Debug.Log( bestTargetable );
// 			return bestTargetable;
// 		}
// 	}
// }