using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Eden.Modules;

namespace Eden.Characteristics {

	public class AddZen : Dumpster.Characteristics.NotificationResponder {

		[SerializeField] private int _amount = 1;

		protected override void Respond () {

			Game.GetModule<Zen>().AddZen( _amount );
		}
	}
}