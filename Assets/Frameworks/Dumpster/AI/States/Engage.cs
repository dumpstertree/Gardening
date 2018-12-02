using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Characteristics;
using Eden.Characteristics;
using Dumpster.Core;
using Eden.AI;

namespace Dumpster.AI {

	[CreateAssetMenu( menuName = "Dumpster/AI/States/Engage/Attack" )]
	public class Engage : State {

	
		// ************** Public ********************
		
		public override State GetInstance ( Personality personality ) {

			var inst = ScriptableObject.Instantiate( this );

			inst._personality = personality;
			inst._eyes = personality.Logic.Actor.GetCharacteristic<Eyes>( true );

			return inst;
		}		
		public override void TryToLeaveState () {

			if( _personality.Logic.Target == null ) {

				// look for a new target

				// if no new target exists
				ChangeState( Personality.States.Roaming );
			}
		}
		public override void EnterState () {
		}
		public override void UpdateState () {

			if ( _personality.Logic.Target == null ) {
				return;
			}

			if ( _currentAction is AttackTarget && !_currentAction.Complete ) {
				return;
			}

			if( !_inSafeRange ) {
				if ( _isTooFar ) { 
					Approach ();
				}
				if ( _isTooClose  ) { 
					BackOff (); 
				}
			
			// } else if ( !_canSeeTarget ) {

			// 	RepositionForVisibility ();
			
			} else {
				Attack ();
			}
		}
		public override void ExitState () {

			_currentAction.Kill ();
		}

	
		// ************** Private ********************

		private float _distToTarget {
			get { return Vector3.Distance( _personality.Logic.Actor.transform.position, _personality.Logic.Target.transform.position ); }
		}
		private bool _canSeeTarget {
			get { return _eyes.CanSeeActor( _personality.Logic.Target ); }
		}
		private bool _isTooClose {
			get { return _distToTarget < _personality.Logic.MinDistanceFromTarget; }
		}
		private bool _isTooFar {
			get { return _distToTarget > _personality.Logic.MaxDistanceFromTarget; }
		}
		private bool _inSafeRange {
			get { return !_isTooClose && !_isTooFar; }
		}

		private Eyes _eyes;
		private Personality _personality;
		private IStateAction _currentAction;

		private void Approach () {

			if ( _currentAction is ApproachTarget ) {
				return;
			}

			if ( _currentAction != null && !_currentAction.Complete ) {
				_currentAction.Kill ();
			}

			_currentAction = new ApproachTarget( _personality );
			_currentAction.Start();
		}
		private void BackOff () {

			if ( _currentAction is BackOffTarget ) {
				return;
			}

			if ( _currentAction != null && !_currentAction.Complete ) {
				_currentAction.Kill ();
			}

			_currentAction = new BackOffTarget( _personality );
			_currentAction.Start();
		}
		private void Attack () {

			if ( _currentAction is AttackTarget && !_currentAction.Complete ) {
				return;
			}

			if ( _currentAction != null && !_currentAction.Complete ) {
				_currentAction.Kill ();
			}

			_currentAction = new AttackTarget( _personality );
			_currentAction.Start();
		}
		private void RepositionForVisibility () {


			_currentAction = new AttackTarget( _personality );
			_currentAction.Start();
		}

		private void ChangeState ( Personality.States newState ) {

			_currentAction.Kill();
			_personality.ChangeState( newState );
		}
	}
	
	public class RepositionForTargetVisibility : IStateAction {

		bool IStateAction.Complete {
			get { return false; }
		}
		void IStateAction.Start () {
		}
		void IStateAction.Kill () {
		}

		private Vector3 GetNewPosition () {
			
			return Vector3.zero;
		}
	}
}