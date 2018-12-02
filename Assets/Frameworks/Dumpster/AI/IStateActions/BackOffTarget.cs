using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.AI;
using Dumpster.Core;
using Dumpster.Characteristics;

namespace Dumpster.AI {

	public class BackOffTarget : IStateAction {

		public BackOffTarget( Personality personality ) {
			
			_personality = personality;
			_actor = personality.Logic.Actor;
			_target = personality.Logic.Target;
			_mask = personality.Logic._mask;
			_pathfinding = personality.Logic.Actor.GetCharacteristic<Pathfinding>();
			_minDistance = personality.Logic.MinDistanceFromTarget;
			_maxDistance = personality.Logic.MaxDistanceFromTarget;
		}
	
		bool IStateAction.Complete {
			get { return _completed; }
		}
		void IStateAction.Start () {
			
			_pathfinding.ClearDestination();
			_loop = Game.Instance.StartCoroutine( BackingOff() );
		}
		void IStateAction.Kill () {

			Game.Instance.StopCoroutine( _loop );
			_pathfinding.ClearDestination();
			_personality.Logic.IsWalking = false;
		}
		

		private const float REEVALUATE_TIME = 0.5f;
		private const float ANGLE_RANGE = 90f;
		private const float PATH_LENGTH_SCORRING = 1f;
		private const float DISTANCE_FROM_TARGET_SCORRING = 1f;

		private float _minDistance;
		private float _maxDistance;
		private bool _completed;
		private Actor _actor;
		private Actor _target;
		private Pathfinding _pathfinding;
		private LayerMask _mask;
		private Coroutine _loop;
		private Vector3 _targetPos;
		private Personality _personality;

		private Vector3 _leftDir {
			get{ return Helpers.GetXZDirectionAwayFromObject( _actor.transform.position, _target.transform.position, -ANGLE_RANGE/2f ); }
		}
		private Vector3 _centerDir {
			get{ return Helpers.GetXZDirectionAwayFromObject( _actor.transform.position, _target.transform.position ); }
		}
		private Vector3 _rightDir {
			get{ return Helpers.GetXZDirectionAwayFromObject( _actor.transform.position, _target.transform.position, ANGLE_RANGE/2f ); }
		}

		private IEnumerator BackingOff () {
		
			_pathfinding.GoToDestination( GetNewPosition() );
			_personality.Logic.IsWalking = true;

			while ( _pathfinding.HasDestination ) {
				
				yield return new WaitForSeconds( REEVALUATE_TIME );

				var dest = GetNewPosition();
				if ( _pathfinding.Destination != dest ) {
					_pathfinding.GoToDestination( dest );
				}
			}
			
			_personality.Logic.IsWalking = false;
			_completed = true;
		}

		private Vector3 GetNewPosition () {

			// left scoring
			var leftPosition = GetPosition( _leftDir, _pathfinding.transform.position, _minDistance );
			var leftPathLength = GetPathLength( leftPosition );
			var leftDistFromTarget = Vector3.Distance( leftPosition, _target.transform.position );
			var leftScore = GetScore( leftPathLength, leftDistFromTarget );

			// center scoring
			var centerPosition = GetPosition( _centerDir, _pathfinding.transform.transform.position, _minDistance );
			var centerPathLength = GetPathLength( centerPosition );
			var centerDistFromTarget = Vector3.Distance( centerPosition, _target.transform.position );
			var centerScore = GetScore( centerPathLength, centerDistFromTarget );

			// right scoring
			var rightPosition = GetPosition( _rightDir,  _pathfinding.transform.transform.position, _minDistance );
			var rightPathLength = GetPathLength( rightPosition );
			var rightDistFromTarget = Vector3.Distance( rightPosition, _target.transform.position );
			var rightScore = GetScore( rightPathLength, rightDistFromTarget );


			// left is highest
			if( leftScore > centerScore && leftScore > rightScore ) {
				return leftPosition;
			}
			// center is highest
			else if( centerScore > leftScore && centerScore > rightScore ) {
				return centerPosition;
			}
			// right is highest
			else if( rightScore > centerScore && rightScore > leftScore ) {
				return rightPosition;
			}
			// all the same
			else {
				return centerPosition;
			}
		}
		private Vector3 GetPosition ( Vector3 direction, Vector3 pos, float maxDistance ) {
			
			RaycastHit hit;
			if( Physics.Raycast( pos, direction, out hit, maxDistance, _mask ) ) {
				return hit.point;
			}

			return pos + direction * maxDistance;
		}
		private float? GetPathLength ( Vector3 destination ) {

			return _pathfinding.GetPathLength( Vector3.zero );
		}
		private float GetScore( float? pathLength, float distFromTarget ) {
			
			if ( !pathLength.HasValue ) {
				return 0f;
			}

			var pathScore = (1f - 1f/pathLength.Value) * PATH_LENGTH_SCORRING;
			var distFromTargetScore = 1f/distFromTarget * DISTANCE_FROM_TARGET_SCORRING;

			return pathScore; // + distFromTargetScore;
		}
	}
}