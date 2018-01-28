using UnityEngine;
using System;

partial class InventoryItem {

	public bool CanShoot { get{ return _canShoot; } }

	[HeaderAttribute("Shoot")]
	[SerializeField] private bool _canShoot;
	[SerializeField] public ShootData _shootData;

	private GunRef _gunRef;

	private void InitCanShoot () {
	
		if ( _canShoot ) {
			
			_gunRef = Game.GunControl.GetGun( _id );
		}
	}

	private void Shoot ( Creature user, ShootData shootData, Action onComplete ) {

		_gunRef.Fire( user, _shootData.BulletPrefab );

 		onComplete();
	}

	[System.Serializable] 
	public class ShootData : InventoryItemData {	

		// *********************************

	 	public HitData _hitData;
		public GameObject BulletPrefab;
	}
}
