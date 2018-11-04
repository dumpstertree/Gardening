using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core {
	
	public abstract class Characteristic : MonoBehaviour {

		public const string ON_ACTOR_ENTER_TRIGGER = "ON_ACTOR_ENTER_TRIGGER";
		public const string ON_ACTOR_EXIT_TRIGGER = "ON_ACTOR_EXIT_TRIGGER";

		public Actor Actor {
			get { return _actor; }
		}

		protected Actor _actor;

		private bool _hasInit = false;
		private bool _hasRun = false;

		public void Install ( Actor actor ) {

			_actor = actor;
		}
		public void Init () {
			
			if ( !_hasInit ) {

				OnInit ();
				_hasInit = true;
			}
		}
		public void Run () {

			if ( !_hasRun ) {
				
				OnRun ();
				_hasRun = true;
			}
		}
		public void ActorUpdate () {
			
			OnActorUpdate ();
		}
		public virtual void RecieveNotification ( string notification ) {
		}
		
		protected virtual void OnInit () {}
		protected virtual void OnRun () {}
		protected virtual void OnActorUpdate () {}
		protected virtual void OnActorEnterTrigger ( Actor actor ) {}
		protected virtual void OnActorExitTrigger ( Actor actor ) {}

		private void OnTriggerEnter ( Collider other ) {

			var actor = other.GetComponent<Actor>();
			if ( actor != null ) {
				
				OnActorEnterTrigger( actor );
				_actor.PostNotification( ON_ACTOR_ENTER_TRIGGER );
			}
		}
		private void OnTriggerExit ( Collider other ) {

			var actor = other.GetComponent<Actor>();
			if ( actor != null ) {
			
				OnActorExitTrigger( actor );
				_actor.PostNotification( ON_ACTOR_EXIT_TRIGGER );
			}
		}
	}
}