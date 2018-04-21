using UnityEngine;

public class GunPartTemplate : ScriptableObject {


	[SerializeField] private GameObject _uiPrefab;

	[SpaceAttribute( 10 )]
	[SerializeField] private float _minFireRate = -1.0f;
	[SerializeField] private float _maxFireRate =  1.0f;

	[SpaceAttribute( 10 )]
	[SerializeField] private float _minReloadTime = -1.0f;
	[SerializeField] private float _maxReloadTime =  1.0f;

	[SpaceAttribute( 10 )]
	[SerializeField] private float _minBulletSpeed = -1;
	[SerializeField] private float _maxBulletSpeed =  1;

	[SpaceAttribute( 10 )]
	[SerializeField] private int _minClipSize = -1;
	[SerializeField] private int _maxClipSize =  1;

	[SpaceAttribute( 10 )]
	[SerializeField] private int _minNumberOfBullets = 0;
	[SerializeField] private int _maxNumberOfBullets = 1;

 	// **************************

 	public Model.Gun.Part GetPart () {

 		var part = new Model.Gun.Part();

 		part.PrefabName = _uiPrefab.name;
		part.Position = new Vector3();
		part.Rotation = new Vector3();
		part.Stats = Roll();

		return part;
 	}
	
	public Model.Gun.Stats Roll () {

		var stats = new Model.Gun.Stats();
		
		stats.FireRate = Random.Range( _minFireRate, _maxFireRate ); 
		stats.ReloadTime = Random.Range( _minReloadTime, _maxReloadTime );
		stats.BulletSpeed = Random.Range( _minBulletSpeed, _maxBulletSpeed );
		stats.ClipSize = Random.Range( _minClipSize, _maxClipSize );
		stats.NumberOfBullets = Random.Range( _minNumberOfBullets, _maxNumberOfBullets );

		return stats;
	}
}
