using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;

namespace Dumpster.Characteristics {

	public class Magnet : Characteristic {

		// ************** Protected ****************

		protected override void OnActorUpdate() {
			
			DrawInMagneticObjects( Game.GetModule<Magnetism>()?.GetMagneticObjectsInArea( _target.position, _range ) );
		}

		
		// ************** Private ****************

		[Header( "Magnet" )]
		[SerializeField] private float _range = 1.0f;
		[SerializeField] private float _targetRange = 0.2f;
		[SerializeField] private float _strength = 3;
		[SerializeField] private Transform _target;

		[Header( "Gizmos" )]
		[SerializeField] private bool _visualize;

		private void OnDrawGizmos () {

			if ( _visualize ) {

				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere( _target.position, _range );

				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere( _target.position, _targetRange );
			}
		}
		private void DrawInMagneticObjects ( List<Magnetic> magneticObjects ) {
			
			foreach( Magnetic m in magneticObjects ) {
				
				var dist = Vector3.Distance( m.Position, _target.position );				
				m.Position = Vector3.Lerp( m.Position, _target.position, ( ( 1f - dist/_range ) * _strength ) * Time.deltaTime);
			}
		}
	}
}