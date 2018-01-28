using UnityEngine;
using Application;

namespace Interactable.Component {

	[RequireComponent(typeof( OptionalComponent.Health ))]
	public class Hitable : MonoBehaviour {

		// ***************** PUBLIC *******************

		public void Hit( Creature user, HitData data ){
			Game.Effects.OneShot( Effects.Type.Hit, transform.position, transform.rotation );
			_health.RemoveHealth( data.Power );
		}

		// ***************** PRIVATE ******************* 

		private OptionalComponent.Health _health;
		private void Awake(){
			
			_health = GetComponent<OptionalComponent.Health>();
		}
	}
}