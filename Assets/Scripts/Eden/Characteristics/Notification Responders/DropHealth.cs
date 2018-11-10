using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {
	
	public class DropHealth : Dumpster.Characteristics.NotificationResponder {

		[Header( "DropHealth"  )]
		[SerializeField] private GameObject _healthDropPrefab;
		[SerializeField] private Transform _spawner;
		[Space]
		[SerializeField] private int _minHealth;
		[SerializeField] private int _maxHealth;
		[Space]
		[SerializeField] private float _minNumOfDrops;
		[SerializeField] private float _maxNumOfDrops;
		[Space]
		[SerializeField] private float _minVelocity;
		[SerializeField] private float _maxVelocity;


		protected override void Respond () {

			var quantityRand = Random.Range(0.0f,1f);
			var quantityCurve = quantityRand*quantityRand;
			var amountDelta = _maxNumOfDrops - _minNumOfDrops;
			var numOfDrops = (int)(_minNumOfDrops + Mathf.Floor( quantityCurve * amountDelta ) );
			
			for ( int i=0; i<numOfDrops; i++ ) {
				CreateHealthDrop( Random.Range( _minHealth, _maxHealth ) );
			}	
		}

		private void CreateHealthDrop( int health ) {

			var inst = Instantiate( _healthDropPrefab );
			var rigid = inst.GetComponent<Rigidbody>();
			
			inst.transform.position = _spawner.position;
			rigid.MovePosition( _spawner.position );
			rigid.velocity = new Vector3( 
				Random.Range( -7, 7 ),
				Random.Range( 7, 15 ),
				Random.Range( -7, 7 )
			);

			// inst.SetHealth( health );
		}
	}
}