using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;

namespace Dumpster.Characteristics {
	
	public class Targetable : Dumpster.Core.Characteristic {

		protected override void OnActorUpdate () {

			Game.GetModule<Targeting>()?.RegisterTargetable( this );
		}
	}
}