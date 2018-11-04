using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;

namespace Dumpster.Characteristics {
	
	public interface IMagneticDelegate  {
		
		void TargetReached ( Actor otherActor );
	}

	public class Magnetic : Characteristic {
		
		// ************** Public ****************
		
		public Vector3 Position {
			get{ return _position.position; }
			set{ _position.position = value; }
		}
		
		// ************** Protected ****************

		protected override void OnActorUpdate () {

		 	Game.GetModule<Magnetism>()?.RegisterMagneticObject( this );
		}


		// ************** Private ****************
		
		[SerializeField] private Transform _position;
	}
}