using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;

namespace Dumpster.Characteristics {

	public class Targeter : Dumpster.Core.Characteristic {

		private enum Mode {
			Camera,
			Object
		}

		[SerializeField] private Mode _mode;
		[SerializeField] private float _maxAngle;
		[SerializeField] private float _maxDistance;

		[SerializeField] private Transform _forward;
		[SerializeField] private Transform _position;

		public Actor GetBestTarget () {

			var forward = Vector3.zero;
			var position = Vector3.zero;

			switch ( _mode ) {
				
				case Mode.Camera :
					forward = Camera.main.transform.forward;
					position = Camera.main.transform.position;
					break;
				
				case Mode.Object :
					forward = _forward.forward;
					position = _forward.position;
					break;
			}

			return Game.GetModule<Targeting>()?.GetBestTarget( position, forward, _maxAngle, _maxDistance );
		}
	}
}