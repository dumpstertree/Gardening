using UnityEngine;
using System;

partial class InventoryItem {

	public bool CanShoot { get{ return _canShoot; } }

	[HeaderAttribute("Shoot")]
	[SerializeField] private bool _canShoot;
	[SerializeField] public ShootData _shootData;

	private void Shoot ( Player shooter, ShootData shootData, Action onComplete ) {

		var gunStats = shootData.CraftedGun.WeaponStats;
		for ( int i = 0; i < gunStats.NumberOfBullets; i++ ){
			
			var go = Instantiate( shootData.BulletPrefab );
			go.transform.position = Game.Area.LoadedPlayer.transform.position;
			go.transform.rotation = Game.Area.LoadedPlayer.transform.rotation;
			go.GetComponent<Bullet>().SetBullet( shooter, shootData._hitData );

			go.transform.rotation  = go.transform.rotation * Quaternion.AngleAxis( UnityEngine.Random.Range( -5f, 5f), go.transform.right );
			go.transform.rotation  = go.transform.rotation * Quaternion.AngleAxis( UnityEngine.Random.Range( -5f, 5f), go.transform.up );
		}

		Game.Async.WaitForSeconds( 1.0f/gunStats.FireRate, onComplete );
	}
}
