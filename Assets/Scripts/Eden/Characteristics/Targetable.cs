using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {

	public class Targetable : Dumpster.Characteristics.Targetable {

		public bool ShowUI {
			get{ return _showUI; }
		}

		[SerializeField] bool _showUI;
	}
}