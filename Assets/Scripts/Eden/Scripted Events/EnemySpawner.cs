using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;


public class EnemySpawner : MonoBehaviour {


	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private int _numOfEnemies;
	[SerializeField] private Vector2 _timeBetweenSpawns;
	[SerializeField] private Transform[] _spawnLocations;

	private bool _hasBeenTriggered;
	private float _nextSpawnTime;
	private int _spawnedEnemies;
	private Actor _player;

	private void Update () {

		if( _hasBeenTriggered && _spawnedEnemies < _numOfEnemies ) {

			if ( Time.time > _nextSpawnTime ) {

				Spawn();
				_nextSpawnTime = Time.time + Random.Range( _timeBetweenSpawns.x, _timeBetweenSpawns.y );
				_spawnedEnemies++;
			}
		}
	}
	private void Spawn () {

		var inst = Instantiate( _enemyPrefab, _spawnLocations[ Random.Range( 0, _spawnLocations.Length ) ].position, Quaternion.identity );
		inst.GetComponentInChildren<EnemyLogic>().Target = _player;
	}
	private void OnTriggerEnter ( Collider other ) {

		if ( _hasBeenTriggered ) {
			return;
		}

		var otherActor = other.GetComponent<Actor>();
		var isPlayer = otherActor?.GetCharacteristic<Alignment>()?.MyAlignment == Alignment.Type.Player;
		
		if ( isPlayer ) {

			_player = otherActor;	
			_hasBeenTriggered = true;
		}
	}
}
