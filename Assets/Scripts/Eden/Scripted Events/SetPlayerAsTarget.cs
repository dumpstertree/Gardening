using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;

public class SetPlayerAsTarget : MonoBehaviour {
	
	[SerializeField] private EnemyLogic[] _enemiesToAlert;
	
	private bool _hasBeenTriggered;

	private void OnTriggerEnter ( Collider other ) {	

		if ( _hasBeenTriggered ) {
			return;
		}

		var otherActor = other.GetComponent<Actor>();
		var isPlayer = otherActor?.GetCharacteristic<Alignment>()?.MyAlignment == Alignment.Type.Player;
		
		if ( isPlayer ) {

			foreach (EnemyLogic e in _enemiesToAlert) {

				if ( e == null ) {
					continue;
				}

				e.Target = otherActor;
			}
			
			_hasBeenTriggered = true;
		}
	}
}
