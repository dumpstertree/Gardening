using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eden.Characteristics;
using Dumpster.Characteristics;

namespace Dumpster.Characteristics {

	public abstract class NotificationResponder : Dumpster.Core.Characteristic {

		[Header( "Triggers" )]
		[SerializeField] private bool _onHealthDecrease;
		[SerializeField] private bool _onHealthIncrease;
		[SerializeField] private bool _onDeath;
		[SerializeField] private bool _onActorEnterTrigger;
		[SerializeField] private bool _onActorExitTrigger;
		[SerializeField] private bool _onRestore;

		public override void RecieveNotification( string notification ) {

			switch( notification ) {

				case Dumpster.Characteristics.Health.DECREASED : 
					if ( _onHealthDecrease ) { Respond(); }
					break;
				
				case Dumpster.Characteristics.Health.INCREASED : 
					if ( _onHealthIncrease ) { Respond(); }
					break;

				case Dumpster.Characteristics.Health.DEATH : 
					if ( _onDeath ) { Respond(); }
					break;

				case Dumpster.Core.Characteristic.ON_ACTOR_ENTER_TRIGGER :
					if ( _onActorEnterTrigger ) { Respond(); }
					break;
				
				case Dumpster.Core.Characteristic.ON_ACTOR_EXIT_TRIGGER :
					if ( _onActorExitTrigger ) { Respond(); }
					break;

				case Dumpster.Characteristics.Restore.RESTORE :
					if ( _onRestore ) { Respond(); }
					break;
			}
		} 

		protected abstract void Respond ();
	}
}