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
		public Personality Personality {
			get { return _personality; }
		}
	
		public override State GetInstance ( Personality personality ) {
			
			var inst = ScriptableObject.Instantiate( this );
			
			inst._personality = personality;
			inst._eyes = personality.Logic.Actor.GetCharacteristic<Eyes>( true );

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

			if ( _currentAction != null && !_currentAction.Complete ) {
				return;
			}

			switch( Random.Range( 0, 2 ) ) {
				
				case 0: 
					_currentAction = new Idle( _personality );
					_currentAction.Start();
					break;
				
				case 1: 
					_currentAction = new Wander( _personality );
					_currentAction.Start();
					break;
			}
		}
		public override void ExitState () {

			_currentAction.Kill();
		}


		// ************* Private ******************

		[SerializeField] private float _movementSpeed;
		[SerializeField] private Personality.States _stateOnSeeEnemy;
		
		private Eyes _eyes;
		private Personality _personality;
		private IStateAction _currentAction;
		
		private void ChangeState ( Personality.States newState ) {

			_currentAction.Kill();
			_personality.ChangeState( _stateOnSeeEnemy );
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