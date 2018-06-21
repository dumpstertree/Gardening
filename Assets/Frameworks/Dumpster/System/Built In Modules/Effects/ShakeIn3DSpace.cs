using UnityEngine;

public class ShakeIn3DSpace : MonoBehaviour {

	[SerializeField] private Dumpster.Core.BuiltInModules.Effects.Shakable _shakable;
	
	private void Awake () {

		_shakable.OnShake += magnitude => {

			transform.localPosition = new Vector3( 
				Random.Range( -magnitude, magnitude ),
 				Random.Range( -magnitude, magnitude ),
 	 			Random.Range( -magnitude, magnitude )
			);
		};
	}
}
