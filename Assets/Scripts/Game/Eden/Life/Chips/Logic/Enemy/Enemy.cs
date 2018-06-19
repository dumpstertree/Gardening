using System.Collections.Generic;
using UnityEngine;

namespace Eden.Life.Chips.Logic {

	public class Enemy : Dumpster.Core.Life.LogicChip {

		public override void Analayze () {

			switch ( _state ) {
				
				case State.Idle:
					OnIdle ();
					break;
				
				case State.Chase:
					OnChase ();
					break;

				case State.Run:
					OnRun ();
					break;
				
				case State.Attack:
					OnAttack ();
					break;
			}
		}

		// ***************************

		[SerializeField] private float _moveSpeed;
		[SerializeField] private Eden.Life.BlackBox _blackBox;
		[SerializeField] private Dumpster.Core.Life.SightChip _eyes;

		[SerializeField] private float _minDistance;
		[SerializeField] private float _maxDistance;
		
		private Eden.Life.BlackBox _target;
		private State _state;

		// *********** STATE MACHINE ****************

		private void Start () {
		
			ChangeState( State.Idle );
		}
		private void ChangeState ( State newState ) {

			if ( _state != newState ) {

				switch ( _state ) {
					case State.Idle: OnExitIdle(); break;
					case State.Chase: OnExitChase(); break;
					case State.Attack: OnExitAttack(); break;
				}

				_state = newState;

				switch ( _state ) {
					case State.Idle: OnEnterIdle(); break;
					case State.Chase: OnEnterChase(); break;
					case State.Attack: OnEnterAttack(); break;
				}
			}
		}

		private void OnEnterIdle () {
		}
		private void OnIdle () {
			
			if ( _target != null ) {

				var dist = GetDistanceToTarget();
					
				if ( dist > _minDistance && dist < _maxDistance ){
					ChangeState( State.Attack );
					return;
				}
				if ( dist < _minDistance ){
					ChangeState( State.Run );
					return;
				}
				if ( dist > _maxDistance ){
					ChangeState( State.Chase );
					return;
				}
			}

			var targets = _blackBox.SightChip.LookForTargets();

			if ( targets.Count > 0 ) {
				_target = targets[ 0 ];
				_state = State.Chase;
			} else {
				_state = State.Idle;
			}
		}
		private void OnExitIdle () {
		}

		private void OnEnterChase () {
		}
		private void OnChase () {
			
			if ( _target != null ) {
				
				var dist = GetDistanceToTarget();
				if ( dist < _maxDistance ) {
					ChangeState( State.Idle );
				} else {
					_blackBox.Animator.SetFloat( "Vertical", 1.0f );
					LookAtTarget();
					MoveForward();	
				}
			}

			if ( _target == null ) {
				
				ChangeState( State.Idle );
			}
		}
		private void OnExitChase () {
		}


		private void OnEnterRun () {
		}
		private void OnRun () {
			
			if ( _target != null ) {
				
				var dist = GetDistanceToTarget();

				if ( dist > _minDistance ) {
					ChangeState( State.Idle );
				} else {
					_blackBox.Animator.SetFloat( "Vertical", 1.0f );
					LookAwayFromTarget();
					MoveForward();	
				}
			}

			if ( _target == null ) {
				
				ChangeState( State.Idle );
			}
		}
		private void OnExitRun () {
		}


		private void OnEnterAttack () {

			_blackBox.Animator.SetFloat( "Vertical", 0.0f );
			_blackBox.Animator.SetFloat( "Horizontal", 0.0f );
			_blackBox.Animator.SetTrigger( "Pickup" );

			LookAtTarget();
		}
		private void OnAttack () {
			
			_blackBox.Interactor.Use();
			ChangeState( State.Chase );
		}
		private void OnExitAttack () {
		}


		// ***************************

		private void LookAtTarget () {

			_blackBox.transform.LookAt( new Vector3( _target.transform.position.x, _blackBox.transform.position.y, _target.transform.position.z) );
		}
		private void LookAwayFromTarget () {

			_blackBox.transform.LookAt( new Vector3( _target.transform.position.x, _blackBox.transform.position.y, _target.transform.position.z) );
			_blackBox.transform.rotation = _blackBox.transform.rotation * Quaternion.AngleAxis( 180, _blackBox.transform.up );
		}
		private void MoveForward () {
			
			_blackBox.Physics.MovePosition( transform.forward * (_moveSpeed * Time.deltaTime) );
		}

		// ***************************

		private float GetDistanceToTarget () {

			if ( _target != null ) {
				return Vector3.Distance( transform.position, _target.transform.position );
			}

			return 999f;
		}

		// ***************************

		private enum State {
			Idle,
			Roam, 
			Chase,
			Run,
			Attack
		}
	}
}