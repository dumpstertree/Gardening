using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Dumpster.Characteristics;

namespace Dumpster.BuiltInModules {

	[CreateAssetMenu(menuName = "Dumpster/Modules/Magnetism")]
	public class Magnetism : Module {

		
		// **************** Public *******************
		
		public List<Magnetic> GetMagneticObjectsInArea ( Vector3 position, float range ) {
			
			var magneticObjectsInRange = new List<Magnetic>();
			
			foreach( Magnetic m in _lastFrameMagneticObjects ) {
				
				if ( m == null ) {
					continue;
				}

				var d = Vector3.Distance( position, m.Position );
				if ( d <= range ) {

					magneticObjectsInRange.Add( m );
				}
			}

			return magneticObjectsInRange;
		}
		public void RegisterMagneticObject ( Magnetic magneticObject ) {

			_thisFrameMagneticObjects.Add( magneticObject );
		}
		

		// **************** Protected *******************
		
		protected override void OnInit () {

			_lastFrameMagneticObjects = new List<Magnetic>();
			_thisFrameMagneticObjects = new List<Magnetic>();
		}
		protected override void OnReload () {
			
			Clear();
		}
		protected override void OnLateUpdate () {
			
			_lastFrameMagneticObjects = new List<Magnetic>( _thisFrameMagneticObjects );
			_thisFrameMagneticObjects.Clear();
		}
		
		
		// **************** Private *******************

		private List<Magnetic> _lastFrameMagneticObjects;
		private List<Magnetic> _thisFrameMagneticObjects;
		
		private void Clear () {

			_lastFrameMagneticObjects.Clear();
			_thisFrameMagneticObjects.Clear();
		}
	}
}
