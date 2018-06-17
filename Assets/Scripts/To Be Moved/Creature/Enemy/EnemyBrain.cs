using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : Brain {

	// ***************************

	public override void Think () {

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
	[SerializeField] private Enemy _creature;
	[SerializeField] private Eyes _eyes;

	[SerializeField] private float _minDistance;
	[SerializeField] private float _maxDistance;
	
	private Creature _target;
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
		
		if ( _target == null ) {
			
			var validTargets = _eyes.LookForTargets();
			if ( validTargets.Count > 0 ) {
				_target = FindBestTarget( validTargets );
			}
		}

		if ( _target != null ){
			
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
				_creature.Animator.SetFloat( "Vertical", 1.0f );
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
				_creature.Animator.SetFloat( "Vertical", 1.0f );
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

		_creature.Animator.SetFloat( "Vertical", 0.0f );
		_creature.Animator.SetFloat( "Horizontal", 0.0f );
		_creature.Animator.SetTrigger( "Pickup" );

		LookAtTarget();
	}
	private void OnAttack () {
	
		_creature.Attack();
		ChangeState( State.Chase );
	}
	private void OnExitAttack () {
	}


	// ***************************

	private void LookAtTarget () {

		transform.LookAt( _target.transform );
	}
	private void LookAwayFromTarget () {

		transform.LookAt( _target.transform );
		transform.rotation = transform.rotation * Quaternion.AngleAxis( 180, transform.up );
	}
	private void MoveForward () {
		
		_creature.Physics.MovePosition( transform.forward * _moveSpeed * Time.deltaTime );
	}

	// ***************************

	private Creature FindBestTarget ( List<Creature> validTargets ) {
		
		return validTargets[ 0 ];
	}
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
