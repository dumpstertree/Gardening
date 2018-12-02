using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;

namespace Dumpster.AI {

	public class Wander : Dumpster.AI.IStateAction {

		//********* Constructor ************

		public Wander ( Personality personality ) {

			_personality = personality;
			_pathfinding = _personality.Logic.Actor.GetCharacteristic<Pathfinding>( true );
		}


		//********* IStateAction ************

		bool IStateAction.Complete { 
			get{ return _complete; } 
		}
		void IStateAction.Start () {

			_pathfinding.ClearDestination();
			_loop = Game.Instance.StartCoroutine( Wandering( Random.Range( _minWalkTime, _maxWalkTime ) ) );
		}
		void IStateAction.Kill () {

			_personality.Logic.IsIdling = false;
			_pathfinding.ClearDestination();
			Game.Instance.StopCoroutine( _loop );
		}

		
		//********* Private ************
		
		private Coroutine _loop;
		private Personality _personality;
		private Pathfinding _pathfinding;
		
		private bool _complete = false;
		private float _minWalkTime = 3f;
		private float _maxWalkTime = 5f;

		private IEnumerator Wandering( float time ) {

			_personality.Logic.IsWalking = true;

			for( float t=0f; t<time; t+=Time.deltaTime ) {
				
				if( !_pathfinding.HasDestination ){
					var newPos =  Random.insideUnitCircle.normalized * 5f;
					_pathfinding.GoToDestination( _pathfinding.Actor.transform.position + new Vector3( newPos.x, 0f, newPos.y ) );
				}

				yield return null;
			}

			_pathfinding.ClearDestination();
			_personality.Logic.IsWalking = false;
			_complete = true;
		}
	}
}