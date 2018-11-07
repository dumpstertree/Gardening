using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Characteristics;
using Dumpster.Core;

namespace Dumpster.BuiltInModules {

	[CreateAssetMenu(menuName = "Dumpster/Modules/Targeting")]
	public class Targeting : Dumpster.Core.Module {


		// ****************** Public **********************

		public void RegisterTargetable( Targetable targetable ) {

			_thisFrameTargetables.Add( targetable );
		}
		public Actor GetBestTarget ( Vector3 position, Vector3 forward, float maxAngle, float maxDistance ) {
			
			var targetables = new List<Targetable>( _lastFrameTargetables );
			
			GetTargetablesInMaxDistance( targetables, position, maxDistance );
			GetTargetablesInMaxAngle( targetables, position, forward, maxAngle );
			GetTargetablesInView( targetables, position, maxDistance );

			return GetHighestScoringTargetable( targetables, position, forward )?.Actor;
		}


		// ****************** Protected **********************
		
		protected override void OnInit () {

			_lastFrameTargetables = new List<Targetable>();
			_thisFrameTargetables = new List<Targetable>();
		}
		protected override void OnLateUpdate() {

			_lastFrameTargetables = _thisFrameTargetables;
			_thisFrameTargetables = new List<Targetable>();
		}
		protected override void OnDrawGizmos () {

			if ( UnityEngine.Camera.main == null ) {
				return;
			}
			
			var targetables = new List<Targetable>( _lastFrameTargetables );
			var forward = UnityEngine.Camera.main.transform.forward;
			var position = UnityEngine.Camera.main.transform.position;
			var maxDistance = 20f;
			var maxAngle = 3f;

			Gizmos.color = Color.red;
			Gizmos.DrawLine( position, position + forward * maxDistance );

			switch( _visualization ) {
				
				case Visualization.None : 
					targetables = new List<Targetable>();
					break;
				
				case Visualization.InMaxDistance : 
					GetTargetablesInMaxDistance( targetables, position, maxDistance );
					break;
				
				case Visualization.InMaxAngle : 
					GetTargetablesInMaxAngle( targetables, position, forward, maxAngle );
					break;
				
				case Visualization.InView : 
					GetTargetablesInView( targetables, position, Mathf.Infinity );
					break;
				
				case Visualization.Best : 
					GetTargetablesInMaxDistance( targetables, position, maxDistance );
					GetTargetablesInMaxAngle( targetables, position, forward, maxAngle );
					GetTargetablesInView( targetables, position, maxDistance );
					var best = GetHighestScoringTargetable( targetables, position, forward );
					if ( best != null ) {
						targetables = new List<Targetable>() { best };
					}
					
					break;
			}

			foreach( Targetable t in targetables ) {
				
				Gizmos.color = Color.green;
				Gizmos.DrawLine( position, t.transform.position );
			}
		}


		// ****************** Private **********************
		
		[Header( "Targeting" )]
		[SerializeField] private float _angleScore = 10f;
		[SerializeField] private float _distanceScore = 10f;
		[SerializeField] private float _raycastPrecision = 0.1f;

		[Header( "Visualize" )]
		[SerializeField] private Visualization _visualization;
		

		private List<Targetable> _thisFrameTargetables;
		private List<Targetable> _lastFrameTargetables;

		private void GetTargetablesInMaxDistance ( List<Targetable> targetables, Vector3 position, float maxDistance ) {

			for ( int i=targetables.Count-1; i>=0; i-- ) {

				var targetable = targetables[ i ];
				if ( Vector3.Distance( position, targetable.transform.position ) > maxDistance ) {
					targetables.RemoveAt( i );
				}
			}
		}
		private void GetTargetablesInMaxAngle ( List<Targetable> targetables, Vector3 position, Vector3 forward, float maxAngle) {

			for ( int i=targetables.Count-1; i>=0; i-- ) {

				var targetable = targetables[ i ];
				var dirToTargetable = (targetable.transform.position - position).normalized;
				if ( Vector3.Angle( forward, dirToTargetable ) > maxAngle ){
					targetables.RemoveAt( i );
				}
			}
		}
		private void GetTargetablesInView ( List<Targetable> targetables, Vector3 position, float maxDistance ) {

			for ( int i=targetables.Count-1; i>=0; i-- ) {
				
				var targetable = targetables[ i ];
				var dirToTargetable = ( targetable.transform.position - position).normalized ;

				RaycastHit hit;
				if ( Physics.Raycast( position, dirToTargetable, out hit, maxDistance ) ) {

					if ( Vector3.Distance( hit.point, targetable.transform.position ) < _raycastPrecision ) {
						continue;
					}
				}
				
				targetables.RemoveAt( i );
			}
		}
		private Targetable GetHighestScoringTargetable ( List<Targetable> targetables, Vector3 position, Vector3 forward ) {

			float bestScore = 0f;
			Targetable bestTargetable = null;

			for ( int i=0; i<targetables.Count; i++ ) {
				
				var targetable = targetables[ i ];

				var dirToTargetable = (targetable.transform.position - position).normalized;
				var angle = Vector3.Angle( forward, dirToTargetable );
				var angleScore = (1f/angle) * _angleScore;

				var distance = Vector3.Distance( position, targetable.transform.position );
				var distanceScore = (1f/distance) * _distanceScore;

				var score = angleScore + distanceScore;
				if ( score > bestScore ) {
					
					bestScore = score;
					bestTargetable = targetable;
				}
			}

			return bestTargetable;
		}


		private enum Visualization {
			None, InMaxDistance, InMaxAngle, InView, Best
		}
	}
}