using UnityEngine;

namespace Eden.Events {

	// public class DropHealthDrop : Dumpster.Events.SmartEvent {

	// 	[SerializeField] private int _minHealth;
	// 	[SerializeField] private int _maxHealth;
	// 	[SerializeField] private float _minNumOfDrops;
	// 	[SerializeField] private float _maxNumOfDrops;

	// 	private const string HEALTH_DROP_PATH = "Health Drop";

	// 	public override void EventTriggered () {

	// 		var quantityRand = Random.Range(0.0f,1f);
	// 		var quantityCurve = quantityRand*quantityRand;
	// 		var amountDelta = _maxNumOfDrops - _minNumOfDrops;
	// 		var numOfDrops = (int)(_minNumOfDrops + Mathf.Floor( quantityCurve * amountDelta ) );
			
	// 		for ( int i=0; i<numOfDrops; i++ ) {
	// 			CreateHealthDrop( Random.Range( _minHealth, _maxHealth ) );
	// 		}
	// 	}
	// 	private void CreateHealthDrop( int health ) {

	// 		var prefab = Resources.Load<HealthDrop>( HEALTH_DROP_PATH );
	// 		var inst = Instantiate( prefab );

	// 		inst.transform.position = transform.position + new Vector3( Random.Range( -1f, 1f ), 0.5f, Random.Range( -1f, 1f ) );
	// 		inst.SetHealth( health );
	// 	}
	// }
}