using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

public class PlayAudio :  Dumpster.Characteristics.NotificationResponder {

	// *************** Protected ********************

	protected override void Respond () {

		Play ();
	}


	// *************** Private ********************

	[Header( "Play Audio" )]
	[SerializeField] private AudioClip _clip;
	[SerializeField] private Transform _spawner;

	private void Play () {

		Game.GetModule<Audio>()?.PlayWorldAudio( _clip, _spawner.position );
	}
}
