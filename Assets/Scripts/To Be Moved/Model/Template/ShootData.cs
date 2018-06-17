using UnityEngine;

namespace Model.Template {

	[System.Serializable]
	public class ShootData {

		public Controller.Item.ShootData GetController () {
			var controller = new Controller.Item.ShootData();
			controller.SetGunGUIDToLookup( "null" );
			controller.SetBulletPrefab( _bulletPrefab );
			return controller;
		}

		[SerializeField] private string _animation;
	 	[SerializeField] private GameObject _bulletPrefab;
	}
}
