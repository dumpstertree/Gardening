using System.Collections.Generic;
using UnityEngine;
using Eden.Model.Life;
using Eden.Life.Chips;
using Dumpster.Life.Chips;

namespace Eden.Life.Chips.Logic {
		
	public class Turret : Dumpster.Core.Life.LogicChip<Visual> {

		
		[Header( "Mesh" )]
		[SerializeField] private Transform _hook;
		[SerializeField] private Transform _turret;
		
		[Header( "Chips" )]
		[SerializeField] private InteractorChip _interactorChip;
		[SerializeField] private SightChip _sightChip;


		private BlackBox _target;

		
		private BlackBox _blackBox {
			get{ return BlackBox as BlackBox; }
		}


		protected override void Think () {

			if ( _target != null ) {
				
				if ( !_target.IsPowered ) {
					
					ClearTarget ();
				
				} else {

					FaceTarget ();
					AttackTarget ();
				}
			}
				
			LookForTargets ();
		}

		
		private void LookForTargets () {

			var targets = _sightChip.LookForTargets ();

			if ( targets.Count > 0 ) {
				_target = targets[ 0 ];
			} 
		}
		private void ClearTarget () {

			_target = null;
		}
		private void FaceTarget () {

			var hookStart = _hook.rotation.eulerAngles;
			var turretStart = _turret.rotation.eulerAngles;
				
			_hook.LookAt( _target.transform );
			_turret.LookAt( _target.transform );

			var hookTarget = _hook.rotation.eulerAngles;
			var turretTarget = _turret.rotation.eulerAngles;

			_hook.transform.rotation = Quaternion.Euler( new Vector3( hookStart.x, hookTarget.y, hookStart.z  ) );
			_turret.transform.rotation = Quaternion.Euler( new Vector3( turretTarget.x, hookTarget.y, turretStart.z  ) );
		}
		private void AttackTarget () {

			_interactorChip.Use( _blackBox.PrimaryEquipedItem );
		}
	}
}
