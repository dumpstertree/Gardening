using System.Collections.Generic;
using UnityEngine;

namespace Eden.Life.Chips.Logic {

	public class Enemy : Dumpster.Core.Life.LogicChip {

		public override void Analayze () {

			if ( _target == null ) {
				
				var targets = _blackBox.SightChip.LookForTargets();
				if ( targets.Count > 0 ) {
					foreach ( Eden.Life.BlackBox b in targets ) {
						if ( b.Visual.Alignment == Eden.Life.Alignment.Player || b.Visual.Alignment == Eden.Life.Alignment.Friendly ){
							_target = b;
							break;
						}
					}
				} 
			}

			if ( _target != null ) {

				if ( !_target.IsPowered ) {
					
					_target = null;
					ChangeState( State.Roam );
				} 
				else {

					var dist = GetDistanceToTarget();
						
					if ( dist > _minDistance && dist < _maxDistance ){
						ChangeState( State.Attack );
					}
					if ( dist < _minDistance ){
						ChangeState( State.Run );
					}
					if ( dist > _maxDistance ){
						ChangeState( State.Chase );
					}
				}
			}

			switch ( _state ) {
				
				case State.Chase:
					OnChase ();
					break;

				case State.Run:
					OnRun ();
					break;
				
				case State.Attack:
					OnAttack ();
					break;

				case State.Roam:
					OnRoam ();
					break;
			}
		}

		// ***************************

		[SerializeField] private Eden.Interactable.Hitable _hitable;
		[SerializeField] private float _moveSpeed;
		[SerializeField] private Eden.Life.BlackBox _blackBox;
		[SerializeField] private Dumpster.Core.Life.SightChip _eyes;

		[SerializeField] private float _minDistance;
		[SerializeField] private float _maxDistance;

		private Eden.Life.BlackBox _target;
		private State _state;

		// *********** STATE MACHINE ****************

		private void Start () {
			
			_hitable.OnHit += HandleOnHit;
			ChangeState( State.Roam );
		}
		private void ChangeState ( State newState ) {

			if ( _state != newState ) {

				switch ( _state ) {
					case State.Chase: OnExitChase(); break;
					case State.Attack: OnExitAttack(); break;
					case State.Run: OnExitRun(); break;
					case State.Roam: OnExitRoam(); break;
				}

				_state = newState;

				switch ( _state ) {
					case State.Chase: OnEnterChase(); break;
					case State.Attack: OnEnterAttack(); break;
					case State.Run: OnEnterRun(); break;
					case State.Roam: OnEnterRoam(); break;
				}
			}
		}


		private void OnEnterChase () {
		}
		private void OnChase () {

			_blackBox.Animator.SetFloat( "Vertical", 1.0f );
			LookAtTarget();
			MoveForward();
		}
		private void OnExitChase () {
		}


		private void OnEnterRun () {
		}
		private void OnRun () {

			_blackBox.Animator.SetFloat( "Vertical", 1.0f );
			LookAwayFromTarget();
			MoveForward();	
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
		}
		private void OnExitAttack () {
		}


		private void OnEnterRoam () {
		}
		private void OnRoam () {

			var roll = Random.Range( 0, 50 );
			if ( roll == 1 ) {
				_blackBox.transform.rotation = Quaternion.Euler( new Vector3( _blackBox.transform.rotation.x, Random.Range( 0f, 360f ) ,_blackBox.transform.rotation.x ));
			} else {
				_blackBox.Animator.SetFloat( "Vertical", 1.0f );
				MoveForward();	
			}
		}
		private void OnExitRoam () {
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

		private void HandleOnHit ( BlackBox user, HitData data ) {
			
			_target = user;
		}
		private enum State {
			None,
			Roam, 
			Chase,
			Run,
			Attack
		}
	}
}