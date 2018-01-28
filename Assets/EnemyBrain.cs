using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : Brain {

	// ***************************

	public override void Think () {

		switch ( _state ) {
			
			case State.Idle:
				OnIdle ();
				break;
			
			case State.Roam:
				OnRoam ();
				break;
			
			case State.Chase:
				OnChase ();
				break;
			
			case State.Attack:
				OnAttack ();
				break;
		}
	}

	// ***************************

	[SerializeField] private GameObject _attackPrefab;

	[SerializeField] private float _moveSpeed;
	[SerializeField] private Enemy _creature;
	[SerializeField] private Eyes _eyes;
	
	private const float ATTACK_RANGE = 1.0f;

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
				case State.Roam: OnExitRoam(); break;
				case State.Chase: OnExitChase(); break;
				case State.Attack: OnExitAttack(); break;
				case State.Dead: OnExitDead(); break;
			}

			_state = newState;

			switch ( _state ) {
				case State.Idle: OnEnterIdle(); break;
				case State.Roam: OnEnterRoam(); break;
				case State.Chase: OnEnterChase(); break;
				case State.Attack: OnEnterAttack(); break;
				case State.Dead: OnEnterDead(); break;
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
				ChangeState( State.Chase );
			}
		}
	}
	private void OnExitIdle () {
	}

	
	private void OnEnterRoam () {
	}
	private void OnRoam () {

		if ( _target == null ) {
			
			var validTargets = _eyes.LookForTargets();
			if ( validTargets.Count > 0 ) {
			
				_target = FindBestTarget( validTargets );
				ChangeState( State.Chase );
			}
		}
	}
	private void OnExitRoam () {
	}


	private void OnEnterChase () {
	}
	private void OnChase () {
		
		if ( _target != null ) {
			
			if ( GetDistanceToTarget() <= ATTACK_RANGE ) {
				ChangeState( State.Attack );
			} else {
				_creature.Animator.SetFloat( "Vertical", 1.0f );
				LookAtTarget();
				MoveTowardTarget();	
			}
		}

		if ( _target == null ) {
			
			ChangeState( State.Idle );
		}
	}
	private void OnExitChase () {
	}


	private void OnEnterAttack () {

		_creature.Animator.SetFloat( "Vertical", 0.0f );
		_creature.Animator.SetFloat( "Horizontal", 0.0f );
		_creature.Animator.SetTrigger( "Pickup" );

		// var inst = Instantiate( _attackPrefab );
		// inst.transform.position = _creature.GunProjector.position;
		// inst.transform.rotation = _creature.GunProjector.rotation;
	}
	private void OnAttack () {
	
		_creature.Attack();
		ChangeState( State.Chase );
	}
	private void OnExitAttack () {
	}

	private void OnEnterDead () {

		_creature.Animator.SetTrigger( "Dead" );
	}
	private void OnDead () {
	}
	private void OnExitDead () {
	}


	// ***************************

	private void LookAtTarget () {

		transform.LookAt( _target.transform );
	}
	private void MoveTowardTarget () {
		
		_creature.Rigidbody.MovePosition( transform.position + transform.forward * _moveSpeed * Time.deltaTime );
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
		Attack,
		Dead
	}
}
