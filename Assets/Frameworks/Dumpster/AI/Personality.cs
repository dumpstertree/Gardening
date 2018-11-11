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


		public EnemyLogic Logic  { get; set; }
	
		public Personality GetInstance ( EnemyLogic logic ) {

			var inst = ScriptableObject.Instantiate( this );
			inst.Logic = logic;

			if ( _roaming != null ) {
				inst.__roaming = _roaming.GetInstance( inst );
			}
			if ( _attacking != null ) {
				inst.__attacking = _attacking.GetInstance( inst );
			}
			if ( _fleeing != null ) {
				inst.__fleeing = _fleeing.GetInstance( inst );
			}

			return inst;
		}
		public void ChangeState ( States newState ) {

			if ( newState != _state ) {
				
				switch( _state ) {

					case States.Roaming: 
						__roaming.ExitState (); 
						break;
					
					case States.Attacking: 
						__attacking.ExitState (); 
						break;
					
					case States.Fleeing: 
						__fleeing.ExitState (); 
						break;
				}

				_state = newState;

				switch( _state ) {

					case States.Roaming: 
						__roaming.EnterState (); 
						break;
					
					case States.Attacking: 
						__attacking.EnterState (); 
						break;
					
					case States.Fleeing: 
						__fleeing.EnterState (); 
						break;
				}
			}
		}
		public void Think () {

			switch( _state ) {

				case States.Roaming: 
					__roaming.TryToLeaveState (); 
					break;
				
				case States.Attacking: 
					__attacking.TryToLeaveState (); 
					break;
				
				case States.Fleeing: 
					__fleeing.TryToLeaveState (); 
					break;
			}

			switch( _state ) {

				case States.Roaming: 
					__roaming.UpdateState (); 
					break;
				
				case States.Attacking: 
					__attacking.UpdateState (); 
					break;
				
				case States.Fleeing: 
					__fleeing.UpdateState (); 
					break;
			}
		}

		
		// *************** Private *********************

		[SerializeField] private State _roaming;
		[SerializeField] private State _attacking;
		[SerializeField] private State _fleeing;
		[SerializeField] private State __roaming;
		[SerializeField] private State __attacking;
		[SerializeField] private State __fleeing;

		private States _state;
	}

	
	public abstract class State : ScriptableObject {

		public abstract State GetInstance ( Personality personality );
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

					var newDestination = _roam.Personality.Logic.Actor.transform.position + new Vector3( Random.Range( -10f, 10f ), 0, Random.Range( -10f, 10f )  );
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