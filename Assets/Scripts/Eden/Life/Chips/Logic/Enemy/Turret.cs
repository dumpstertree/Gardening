﻿using System.Collections.Generic;
using UnityEngine;

namespace Eden.Life.Chips.Logic {
		
	public class Turret : Dumpster.Core.Life.LogicChip {


		[SerializeField] private Eden.Life.BlackBox _blackBox;

		private Eden.Life.BlackBox _target;

		[SerializeField] private Transform _hook;
		[SerializeField] private Transform _turret;

		public override void Analayze () {

			if ( _target != null ) {

				if ( !_target.IsPowered ) {
					_target = null;
				} else {
					var hookStart = _hook.rotation.eulerAngles;
					var turretStart = _turret.rotation.eulerAngles;
					
					_hook.LookAt( _target.transform );
					_turret.LookAt( _target.transform );

					var hookTarget = _hook.rotation.eulerAngles;
					var turretTarget = _turret.rotation.eulerAngles;

					_hook.transform.rotation = Quaternion.Euler( new Vector3( hookStart.x, hookTarget.y, hookStart.z  ) );
					_turret.transform.rotation = Quaternion.Euler( new Vector3( turretTarget.x, hookTarget.y, turretStart.z  ) );

					_blackBox.Interactor.Use ();
					return;
				}
			}
				
			var targets = _blackBox.SightChip.LookForTargets();

			if ( targets.Count > 0 ) {
				_target = targets[ 0 ];
			} 
		}
	}
}
