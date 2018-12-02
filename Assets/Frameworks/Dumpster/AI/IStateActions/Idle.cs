using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Dumpster.AI {

	public class Idle : IStateAction {


		//********* Constructor ************

		public Idle ( Personality personality ) {
			
			_personality = personality;
		}

		
		//********* IStateAction ************

		bool IStateAction.Complete { 
			get{ return _complete; } 
		}
		void IStateAction.Start () {

			_loop = Game.Instance.StartCoroutine( Idling( Random.Range( _minIdleTime, _maxIdleTime ) ) );
		}
		void IStateAction.Kill () {

			_personality.Logic.IsIdling = false;
			Game.Instance.StopCoroutine( _loop );
		}

		
		//********* Private ************
		
		private Coroutine _loop;
		private Personality _personality;
		
		private bool _complete = false;
		private float _minIdleTime = 3f;
		private float _maxIdleTime = 5f;

		private IEnumerator Idling ( float time ) {

			_personality.Logic.IsIdling = true;

			for( float t=0f; t<time; t+=Time.deltaTime ) {
				yield return null;
			}

			_personality.Logic.IsIdling = false;
			_complete = true;
		}
	}
}
