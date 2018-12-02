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
			Invalid,
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

			inst.ChangeState( States.Roaming );

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
}