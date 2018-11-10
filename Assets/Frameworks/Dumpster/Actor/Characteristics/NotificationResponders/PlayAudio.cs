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
	
	[SerializeField] private Transform _spawner;	
	[SerializeField] private AudioClip _clip;
	
	[Range(0f,1f)]
	[SerializeField] private float _volume = 0.5f;

	private void Play () {

		Game.GetModule<Audio>()?.PlayWorldAudio( _clip, _spawner.position, _volume );
	}
}
