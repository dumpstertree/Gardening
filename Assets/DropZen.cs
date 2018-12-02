using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {

	public class DropZen : Dumpster.Characteristics.NotificationResponder {
			
		[Header( "Drop Zen"  )]
		[SerializeField] private Transform _spawner;
		[SerializeField] private GameObject _zenDropPrefab;
		[Space]
		[SerializeField] private float _minNumOfDrops = 0;
		[SerializeField] private float _maxNumOfDrops = 1;

		protected override void Respond () {

			var quantityRand = Random.Range(0.0f,1f);
			var quantityCurve = quantityRand*quantityRand;
			var amountDelta = _maxNumOfDrops - _minNumOfDrops;
			var numOfDrops = (int)(_minNumOfDrops + Mathf.Floor( quantityCurve * amountDelta ) );
			
			for ( int i=0; i<numOfDrops; i++ ) {
				CreateZenDrop ();
			}	
		}

		private void CreateZenDrop () {

			var inst = Instantiate( _zenDropPrefab );
			var rigid = inst.GetComponent<Rigidbody>();
			
			inst.transform.position = _spawner.position;
			rigid.MovePosition( _spawner.position );
			rigid.velocity = new Vector3( 
				Random.Range( -7, 7 ),
				Random.Range( 7, 15 ),
				Random.Range( -7, 7 )
			);
		}
	}
}