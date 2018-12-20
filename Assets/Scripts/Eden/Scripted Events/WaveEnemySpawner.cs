using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;


public class WaveEnemySpawner : MonoBehaviour {

	[SerializeField] private Wave[] _waves;

	private List<GameObject> _existingCreatures = new List<GameObject>();
	private Actor _player;
	private bool _hasBeenTriggered;
	private int _wave;

	private void SpawnWave ( Wave wave ) {

		foreach( Creature c in wave._creatures ) {
			Spawn( c._prefab, c._spawnLocation.position );
		}
	}
	private void Spawn ( GameObject prefab, Vector3 position ) {

		var inst = Instantiate( prefab, position, Quaternion.identity );
		inst.GetComponentInChildren<EnemyLogic>().Target = _player;

		_existingCreatures.Add( inst );
	}
	private void Update () {

		if( _hasBeenTriggered ) {
		
			for ( int i =_existingCreatures.Count-1; i>=0; i-- ) {
				
				if( _existingCreatures[ i ] == null ) {
					_existingCreatures.RemoveAt( i );
				}
			}

			if ( _existingCreatures.Count == 0 && _wave < _waves.Length ) {

				SpawnWave( _waves[ _wave ] );
				_wave++;
			}
		}
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
	
	[System.Serializable]
	public class Wave {

		[SerializeField] public Creature[] _creatures;
	}

	[System.Serializable]
	public class Creature {

		[SerializeField] public GameObject _prefab;
		[SerializeField] public Transform _spawnLocation;
	}
}
