using Dumpster.Core;
using Dumpster.BuiltInModules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class FreezeFrame : Dumpster.Characteristics.NotificationResponder {

		[Header( "Settings")]
		[SerializeField] private FreezeFrameDuration _duration;
		
		protected override void Respond () {
		
			Game.GetModule<Effects>()?.FreezeFrame( _duration );
		}
	}
}