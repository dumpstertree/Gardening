using System.Collections.Generic;
using UnityEngine;

namespace Model {

	[System.Serializable]
	public class Gun {

		[SerializeField] public List<Part> WeaponParts;
		[SerializeField] public Stats WeaponStats;

		public Gun ( List<Part> parts ) {
			
			WeaponParts = parts;
			WeaponStats = new Stats();
			
			foreach( Part c in WeaponParts ){
				WeaponStats += c.Stats;
			}
		}

		[System.Serializable]
		public class Part {
			
			[SerializeField] public string PrefabName;
			[SerializeField] public Vector3 Position;
			[SerializeField] public Vector3 Rotation;
			[SerializeField] public Stats Stats;

			public Part ( UI.Elements.GunCrafting.Part gunComponent ) {

				PrefabName = gunComponent.PrefabName;
				Position = gunComponent.transform.position;
				Rotation = gunComponent.transform.rotation.eulerAngles;
				Stats = gunComponent.Stats;
			}
			public Part () {}
		}

		[System.Serializable]
		public class Stats {

			[SerializeField] public float FireRate;
			[SerializeField] public float ReloadTime;
			[SerializeField] public float BulletSpeed;
			[SerializeField] public int ClipSize;
			[SerializeField] public int NumberOfBullets;

			public Stats () {

				FireRate = 1;
				ReloadTime = 1;
				BulletSpeed = 1;
				ClipSize = 1;
				NumberOfBullets = 1;
			}

			public static Stats operator +(Stats s1, Stats s2) {
			    var s = new Stats( );
			    s.FireRate = s1.FireRate + s2.FireRate;
				s.ReloadTime = s1.ReloadTime + s2.ReloadTime;
				s.BulletSpeed = s1.BulletSpeed + s2.BulletSpeed;
				s.ClipSize = s1.ClipSize + s2.ClipSize;
				s.NumberOfBullets = s1.NumberOfBullets + s2.NumberOfBullets;
			    return s;
			}
		}
	}
}