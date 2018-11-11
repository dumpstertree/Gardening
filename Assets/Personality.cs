using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;

namespace Dumpster.AI {

	[CreateAssetMenu( menuName = "Dumpster/AI/Personality" )]
	public class Personality : ScriptableObject {

		// *************** Public *********************
	
		public enum States {
			Roaming,
			Attacking,
			Fleeing
		}

		public Actor Target { get; set; }
		public Actor Actor  { get; set; }
	
		public void ChangeState ( States newState ) {

			if ( newState != _state ) {
				
				switch( _state ) {

					case States.Roaming: 
						_roaming.ExitState (); 
						break;
					
					case States.Attacking: 
						_attacking.ExitState (); 
						break;
					
					case States.Fleeing: 
						_fleeing.ExitState (); 
						break;
				}

				_state = newState;

				switch( _state ) {

					case States.Roaming: 
						_roaming.EnterState (); 
						break;
					
					case States.Attacking: 
						_attacking.EnterState (); 
						break;
					
					case States.Fleeing: 
						_fleeing.EnterState (); 
						break;
				}
			}
		}
		public void Think () {

			switch( _state ) {

				case States.Roaming: 
					_roaming.TryToLeaveState (); 
					break;
				
				case States.Attacking: 
					_attacking.TryToLeaveState (); 
					break;
				
				case States.Fleeing: 
					_fleeing.TryToLeaveState (); 
					break;
			}

			switch( _state ) {

				case States.Roaming: 
					_roaming.UpdateState (); 
					break;
				
				case States.Attacking: 
					_attacking.UpdateState (); 
					break;
				
				case States.Fleeing: 
					_fleeing.UpdateState (); 
					break;
			}
		}

		
		// *************** Private *********************

		[SerializeField] private State _roaming;
		[SerializeField] private State _attacking;
		[SerializeField] private State _fleeing;

		private States _state;
	}

	
	public abstract class State : ScriptableObject {

		public abstract void TryToLeaveState ();

		public abstract void EnterState ();
		public abstract void UpdateState ();
		public abstract void ExitState ();
	}



	public class Roaming {

		// ************* Public ******************

		public enum Actions { Walk, Idle }

		public Actions Action { get; }
		public bool ActionIsFinished { get; private set; }

		public Roaming ( Roam roam ) {

			_roam = roam;

			Action = (Actions)Random.Range( 0, 2 );
			
			switch( Action ) {
				case Actions.Idle: StartIdle(); break;
				case Actions.Walk: StartWalk(); break;
			}
		}
		public void Kill () {

			switch( Action ) {
				case Actions.Idle: KillIdle(); break;
				case Actions.Walk: KillWalk(); break;
			}
		}


		// ************* Private ******************
		
		private Roam _roam;
		private Coroutine _loop;

		private float _minWalkTime = 5f;
		private float _maxWalkTime = 15f;
		private float _minIdleTime = 1f;
		private float _maxIdleTime = 5f;

		private void StartWalk () {
			
			var actionTime = Random.Range( _minWalkTime, _maxWalkTime );
			_loop = Game.Instance.StartCoroutine( Walking( actionTime, () => { ActionIsFinished = true; } ) );
		}
		private void StartIdle () {
			
			var actionTime = Random.Range( _minIdleTime, _maxIdleTime );
			_loop = Game.Instance.StartCoroutine( Idling( actionTime, () => { ActionIsFinished = true; } ) );
		}
		
		private void KillWalk () {

			Game.Instance.StopCoroutine( _loop );
			
			_roam.Pathfinding.ClearDestination();
		}
		private void KillIdle () {

			Game.Instance.StopCoroutine( _loop );
		}

		IEnumerator Walking ( float time, System.Action onComplete ) {

			for( float t=0f; t<time; t+=Time.deltaTime ) {
				
				if( !_roam.Pathfinding.HasDestination ) {

					var newDestination = Vector3.zero;
					_roam.Pathfinding.GoToDestination( newDestination );
				}

				yield return null;
			}

			onComplete ();
		}
		IEnumerator Idling ( float time, System.Action onComplete ) {

			for( float t=0f; t<time; t+=Time.deltaTime ) {
				
				yield return null;
			}

			onComplete ();
		}
	}
}