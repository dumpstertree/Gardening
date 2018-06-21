using UnityEngine;
using Dumpster.Events;

namespace Eden.Events {
	
	public class Shake : SmartEvent {
		
		[SerializeField] private Transform _shakePosition;
		[SerializeField] private Dumpster.Core.BuiltInModules.Effects.Controller.ShakePower _power;
		[SerializeField] private Dumpster.Core.BuiltInModules.Effects.Controller.DecayRate _decay;

		public override void EventTriggered () {
			
			EdensGarden.Instance.Effects.Shake( _shakePosition.position, _power, _decay );
		}
	}
}