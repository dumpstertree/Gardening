using UnityEngine;
using Dumpster.Core.BuiltInModules.Effects;

namespace Interactable.Component {

	[RequireComponent(typeof( OptionalComponent.Health ))]
	public class Hitable : MonoBehaviour {

		// ***************** PUBLIC *******************

		public void Hit( Eden.Life.Brain.BlackBoxBrain user, HitData data ){
			
			EdensGarden.Instance.Effects.OneShot( ParticleType.Hit, transform.position, transform.rotation );
			_health.RemoveHealth( data.Power );
		}

		// ***************** PRIVATE ******************* 

		private OptionalComponent.Health _health;
		private void Awake(){
			
			_health = GetComponent<OptionalComponent.Health>();
		}
	}
}