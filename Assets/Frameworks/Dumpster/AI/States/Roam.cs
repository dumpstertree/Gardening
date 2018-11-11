using Dumpster.BuiltInModules;
using Dumpster.Core;
using Dumpster.Characteristics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.AI {
	
	[CreateAssetMenu( menuName = "Dumpster/AI/States/Roam" )]
	public class Roam: State {

		// ************* Public ******************
		
		public Eyes Eyes {
			get{ return _eyes; }
		}
		public Pathfinding Pathfinding {
			get{ return _pathFinding; }
		}
	
		public void GetInstance ( Personality personality ) {

			_personality = personality;
			_eyes = _personality.Actor.GetCharacteristic<Eyes>( true );
			_pathFinding = _personality.Actor.GetCharacteristic<Pathfinding>( true );
		}

		public override void TryToLeaveState () {

			LookForTarget ();

			if ( _personality.Target != null ) {
				ChangeState ( _stateOnSeeEnemy );
			}
		}
		public override void EnterState () {

			_pathFinding.MovementSpeed = _movementSpeed;
		}
		public override void UpdateState () {

			if ( _roaming.ActionIsFinished ) {

				_roaming = new Roaming( this );
			}
		}
		public override void ExitState () {

			_roaming.Kill();
		}


		// ************* Private ******************

		[SerializeField] private float _movementSpeed;
		[SerializeField] private Personality.States _stateOnSeeEnemy;

		private Personality _personality;
		private Eyes _eyes;
		private Pathfinding _pathFinding;
		private Roaming _roaming;
		
		private void ChangeState ( Personality.States newState ) {

			_roaming.Kill();
			_personality.ChangeState( _stateOnSeeEnemy );
		}
		private void LookForTarget () {
			
			var targetActorsInView = _eyes.LookForActors( SortEyes );
			if ( targetActorsInView.Count > 0 ) {
			
				var target = targetActorsInView[ 0 ];
				_personality.Target = target;
			}
		}
		private void SortEyes ( List<Actor> actors ) {

			for( int i=actors.Count-1; i>=0; i-- ) {

				var actor = actors[ i ];
				var alignmet = actor.GetCharacteristic<Alignment>();

				if ( alignmet == null || alignmet.MyAlignment != Alignment.Type.Player ){
					actors.RemoveAt( i );
				}
			}
		}
	}
}