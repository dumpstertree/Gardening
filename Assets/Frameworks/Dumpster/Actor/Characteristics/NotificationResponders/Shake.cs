using Dumpster.Core;
using Dumpster.BuiltInModules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Shake : Dumpster.Characteristics.NotificationResponder {

		[SerializeField] private Transform _shakeOrigin;
		[SerializeField] private Dumpster.BuiltInModules.ShakePower _power;
		[SerializeField] private Dumpster.BuiltInModules.DecayRate _decay;

		protected override void Respond() {
			
			Game.GetModule<Effects>()?.Shake( _shakeOrigin.position, _power, _decay );
		}
	}
}