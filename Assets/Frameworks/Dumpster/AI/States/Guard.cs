using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.AI;
using Dumpster.Core;
using Dumpster.Characteristics;

namespace Dumpster.AI {

	[CreateAssetMenu( menuName = "Dumpster/AI/States/Guard" )]
	public class Guard : State {

		
		// ************* Public *****************

		public override State GetInstance ( Personality personality ) {

			var inst = ScriptableObject.Instantiate( this );

			inst._personality = personality;
			inst._eyes = personality.Logic.Actor.GetCharacteristic<Eyes>();

			return inst;
		}

		public override void TryToLeaveState () {
			
			LookForTarget ();

			if ( _personality.Logic.Target != null ) {
				ChangeState ( _stateOnSeeEnemy );
			}
		}
		public override void EnterState () {
			
		
		}
		public override void UpdateState () {
			
			if ( _currentAction == null || _currentAction.Complete ) {
				_currentAction = new Idle( _personality );
				_currentAction.Start();
			}
		}
		public override void ExitState () {
			
			_currentAction.Kill();
		}

		
		// ************* Private *****************
		
		[SerializeField] private Personality.States _stateOnSeeEnemy;
		
		private Personality _personality;
		private Eyes _eyes;
		private IStateAction _currentAction;
		
		private void ChangeState ( Personality.States newState ) {

			_currentAction.Kill();
			_personality.ChangeState( newState );
		}
		private void LookForTarget () {
			
			var targetActorsInView = _eyes.LookForActors( SortEyes );
			if ( targetActorsInView.Count > 0 ) {
			
				var target = targetActorsInView[ 0 ];
				_personality.Logic.Target = target;
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