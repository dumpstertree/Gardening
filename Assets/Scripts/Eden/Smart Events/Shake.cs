using Dumpster.BuiltInModules;
using Dumpster.Core;
using Dumpster.Events;
using UnityEngine;

namespace Eden.Events {
	
	public class Shake : SmartEvent {
		
		[SerializeField] private Transform _shakePosition;
		[SerializeField] private Dumpster.BuiltInModules.ShakePower _power;
		[SerializeField] private Dumpster.BuiltInModules.DecayRate _decay;

		public override void EventTriggered () {
			
			Game.GetModule<Effects>()?.Shake( _shakePosition.position, _power, _decay );
		}
	}
}