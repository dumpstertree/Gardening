using Dumpster.Core;
using Dumpster.Characteristics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {
	
	public class EnemyLogic : Dumpster.Core.Characteristic {

		//**************** Public ******************
		
		public const string ENTER_ROAMING 	= "EnemyLogic.EnterRoaming";
		public const string EXIT_ROAMING 	= "EnemyLogic.ExitRoaming";
		public const string ENTER_ATTACKING = "EnemyLogic.EnterAttacking";
		public const string EXIT_ATTACKING 	= "EnemyLogic.ExitAttacking";
		public const string ENTER_RUNNING 	= "EnemyLogic.EnterRunning";
		public const string EXIT_RUNNING 	= "EnemyLogic.ExitRunning";

		public override List<string> GetNotifications () {

			return new List<string>() {
				ENTER_ROAMING,
				EXIT_ROAMING,
				ENTER_ATTACKING,
				EXIT_ATTACKING,
				ENTER_RUNNING,
				EXIT_RUNNING
			};
		}
		public override void RecieveNotification( string notification ) {
			
			if ( notification == Health.DECREASED ) {
				ChangeState( States.Attacking );
			}
		}

		// ************** Protected ************************
		
		protected override void OnInit() {
			
			_item = _itemTemplate.CreateInstance();
		}
		protected override void OnActorUpdate () {

			switch( _state ) {

				case States.Roaming: OnRoaming(); break;
				case States.Attacking: OnAttacking(); break;
			}
		}
		

		//**************** Private ******************
		
		private enum States {
			Roaming,
			Attacking,
			Running
		}

		[SerializeField] private float _roamSpeed = 3;
		[SerializeField] private float _attackSpeed = 8;
		[SerializeField] private float _minDistanceFromTarget;
		[SerializeField] private float _maxDistanceFromTarget;
		[SerializeField] private States _state;
		[SerializeField] private Eden.Templates.Item _itemTemplate;
		[SerializeField] private Actor _target;

		private Eden.Model.Item _item;

		private float _distToTarget {
			get{ return Vector3.Distance( _actor.transform.position, _target.transform.position ); }
		}
		private bool _isTooClose {
			get{ return _distToTarget < _minDistanceFromTarget; }
		}
		private bool _isTooFar {
			get{  return _distToTarget > _maxDistanceFromTarget; }
		}
		private bool _inUnsafeRange {
				get{  return !_isTooClose && !_isTooFar; }
		}
		
		private void ChangeState ( States newState ) {

			if ( newState != _state ) {
				
				switch( _state ) {
					case States.Roaming: OnExitRoaming(); break;
					case States.Attacking: OnExitAttacking(); break;
				}

				_state = newState;

				switch( _state ) {
					case States.Roaming: OnEnterRoaming(); break;
					case States.Attacking: OnEnterAttacking(); break;
				}
			}
		}

		private void OnEnterRoaming () {
			
			_actor.GetCharacteristic<Pathfinding>(true).MovementSpeed = _roamSpeed;
			_actor.PostNotification( ENTER_ROAMING );
		}
		private void OnRoaming () {
			
			var targets = _actor.GetCharacteristic<Eyes>( true )?.LookForActors( SortEyes );
			if ( targets.Count > 0 ) {
				_target = targets[0];
				ChangeState( States.Attacking );
				return;
			}
			
			var pathfinding = _actor.GetCharacteristic<Pathfinding>( true );
			if ( pathfinding != null ) {
				
				if ( !pathfinding.HasDestination ){
				
					var desitination = _actor.transform.position + new Vector3( 
						Random.Range( -10f, 10f ), 
						0f, 
						Random.Range( -10f, 10f ) 
					);
					
					pathfinding.GoToDestination( desitination );
				}
			}
		}
		private void OnExitRoaming () {	

			_actor.PostNotification( EXIT_ROAMING );
		}
	
		private void OnEnterAttacking () {
			
			_actor.GetCharacteristic<Pathfinding>(true).MovementSpeed = _attackSpeed;
			_actor.PostNotification( ENTER_ATTACKING );
		}
		private void OnAttacking () {

			if( _target != null ) {
				
				var eyes = _actor.GetCharacteristic<Eyes>( true );
				var pathfinding = _actor.GetCharacteristic<Pathfinding>( true );
				
				if ( _inUnsafeRange && !pathfinding.HasDestination || !eyes.CanSeeActor( _target ) && !pathfinding.HasDestination ) {
					pathfinding.GoToDestination( Quaternion.AngleAxis( Random.Range( 0f, 360f ), Vector3.up ) * Vector3.forward * Random.Range( _minDistanceFromTarget, _maxDistanceFromTarget ) + _target.transform.position );
				}
			
				if ( !_inUnsafeRange && !pathfinding.HasDestination ) {
					_actor.transform.LookAt( _target.transform );
					_actor.GetCharacteristic<Interactor>( true )?.Use( _item );
				}
			}
		}
		private void OnExitAttacking () {
			
			_actor.PostNotification( EXIT_ATTACKING );
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