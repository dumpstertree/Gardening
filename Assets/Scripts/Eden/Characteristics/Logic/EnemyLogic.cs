using Dumpster.Core;
using Dumpster.Characteristics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.AI;

namespace Dumpster.Characteristics {
	
	public class EnemyLogic : Dumpster.Core.Characteristic {

		public Actor Target {
			get{ return _target; }
			set{ 
				if ( _target != value ) {
					_actor.PostNotification( ON_GET_TARGET );
					_target = value;
				}
			 }
		}
		public Eden.Model.Item Item {
			get{ return __item; }
		}

		[SerializeField] private Animator2 _animator;
		[SerializeField] private Personality _personality;
		[SerializeField] private Eden.Templates.Item _item;

		public LayerMask _mask;
		public float MinDistanceFromTarget;
		public float MaxDistanceFromTarget;

		private const string IDLE_ANIMATION   = "Idle";
		private const string WALK_ANIMATION   = "Walk";
		private const string RUN_ANIMATION 	  = "Run";
		private const string ATTACK_ANIMATION = "Attack";

		private Personality __personality;
		private Eden.Model.Item __item;

		private Actor _target;

		private float _offset;

		
		// ***************** Public ********************

		public bool IsIdling    { get; set; }
		public bool IsWalking   { get; set; }
		public bool IsRunning   { get; set; }
		public bool IsAttacking { get; set; }
	
		public override List<string> GetNotifications () {

			return new List<string>() {
				ON_GET_TARGET
			};
		}
		public override void ActorDisabled () {
		
			__personality.Kill();
		}
		
		// **************** Protected ********************
		
		protected override void OnInit () {
			
			_offset =  Random.Range( 0f, 1f );
			__item = _item.CreateInstance();
			__personality = _personality.GetInstance( this );

			_idleDampner   = new AnimationDampener( _animator, IDLE_ANIMATION );
			_walkDampner   = new AnimationDampener( _animator, WALK_ANIMATION );
			_runDampner    = new AnimationDampener( _animator, RUN_ANIMATION );
			_attackDampner = new AnimationDampener( _animator, ATTACK_ANIMATION );
		}
		protected override void OnActorUpdate () {

			__personality.Think ();

			if ( !_wasIdling && IsIdling ) {
				OnBeginIdling ();
			} else if ( _wasIdling && !IsIdling ) {
				OnEndIdling ();
			} else if ( IsIdling ) { 
				OnIdling (); 
			}
			_wasIdling = IsIdling;


			if ( !_wasWalking && IsWalking ) {
				OnBeginWalking ();
			} else if ( _wasWalking && !IsWalking ) {
				OnEndWalking ();
			} else if ( IsWalking) { 
				OnWalking (); 
			}
			_wasWalking = IsWalking;


			if ( !_wasRunning && IsRunning ) {
				OnBeginRunning ();
			} else if ( _wasRunning && !IsRunning ) {
				OnEndRunning ();
			} else if ( IsRunning) { 
				OnRunning (); 
			}
			_wasRunning = IsRunning;


			if ( !_wasAttacking && IsAttacking ) {
				OnBeginAttacking ();
			} else if ( _wasAttacking && !IsAttacking ) {
				OnEndAttacking ();
			} else if ( IsAttacking) { 
				OnAttacking (); 
			}
			_wasAttacking = IsAttacking;
		}
	

		// **************** Private ********************
		
		private AnimationDampener _idleDampner;
		private AnimationDampener _walkDampner;
		private AnimationDampener _runDampner;
		private AnimationDampener _attackDampner;

		public const string ON_GET_TARGET 	= "EnemyLogic.GetTarget";

		private bool _wasIdling;
		private bool _wasWalking;
		private bool _wasRunning;
		private bool _wasAttacking;

		private void OnBeginIdling () {

			_idleDampner.SetWeight( 1f, 0.2f );
		}
		private void OnIdling () {

			_idleDampner.SetProgress( Mathf.Repeat( Time.time, 1f ) );
		}
		private void OnEndIdling () {
			
			_idleDampner.SetWeight( 0f, 0.2f );
		}
		
		private void OnBeginWalking () {

			_walkDampner.SetWeight( 1f, 0.2f );
		}
		private void OnWalking () {

			_walkDampner.SetProgress( Mathf.Repeat( _offset + Time.time * 2, 1f ) );
		}
		private void OnEndWalking () {

			_walkDampner.SetWeight( 0f, 0.2f );
		}
		
		private void OnBeginRunning () {
		}
		private void OnRunning () {
		}
		private void OnEndRunning () {
		}
		
		private void OnBeginAttacking () {
		}
		private void OnAttacking () {
		}
		private void OnEndAttacking () {
		}
	}
}