using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Eden.Modules;

namespace Eden.Modules {

	public class BreakZen : Dumpster.Characteristics.NotificationResponder {

		protected override void Respond () {

			Game.GetModule<Zen>()?.BreakZen();
		}
	}
}