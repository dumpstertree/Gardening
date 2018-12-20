using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.AI;
using Dumpster.Core;
using Dumpster.Characteristics;
using Eden.Characteristics;

namespace Eden.AI {

public class AttackTarget : Dumpster.AI.IStateAction {
		
	
		// *************** Public *******************
		
		public AttackTarget( Personality personality ) {

			_item = personality.Logic.Item;
			_actor = personality.Logic.Actor;
			_target = personality.Logic.Target;
			_interactor = personality.Logic.Actor.GetCharacteristic<Interactor>( true );
		}


		// ************ State Action *******************

		bool Dumpster.AI.IStateAction.Complete {
			get { return _completed; }
		}
		void Dumpster.AI.IStateAction.Start () {

			var time = Random.Range( MIN_ATTACK_TIME, MAX_ATTACK_TIME );
			_loop = Game.Instance.StartCoroutine( Attacking( time ) );
		}
		void Dumpster.AI.IStateAction.Kill () {
	
			Game.Instance.StopCoroutine( _loop );
		}

	
		// *************** Public *******************

		private const float MIN_ATTACK_TIME = 1.0f;
		private const float MAX_ATTACK_TIME = 3.0f;

		private Eden.Model.Item _item;
		private Interactor _interactor;
		private Coroutine _loop;
		private Actor _target;
		private Actor _actor;
		private bool _completed;

		private IEnumerator Attacking ( float time ) {

			for ( float t=0f; t<time; t+=Time.deltaTime ) {

				if ( _target == null ) {
					break;
				}

				LookAtTarget ();
				UseItem ();

				yield return null;
			}

			_completed = true;
		}
		private void LookAtTarget () {
			
			_actor?.transform.LookAt( _target.transform );
		}
		private void UseItem () {
			
			_interactor?.Use( _item );
		}
	}
}