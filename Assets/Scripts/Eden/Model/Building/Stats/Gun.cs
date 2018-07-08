using UnityEngine;
using System.Collections.Generic;

namespace Eden.Model.Building.Stats {

	[System.Serializable]
	public class Gun : Stats<Gun>, Eden.UI.Elements.Building.IStatBlockDelegate {

		
		// ************** Constructors ****************
		
		public Gun () {

			_rateOfFire   = 1;
			_reloadSpeed  = 1;
			_accuracy 	  = 1;
			_numOfBullets = 1;
			_clipSize 	  = 1;
			_bulletSpeed  = 1;
			_bulletSize   = 1;
		}
		public Gun ( int rateOfFire, int reloadSpeed, int accuracy, int numOfBullets, int clipSize, int bulletSpeed, int bulletSize ) {

			 _rateOfFire = rateOfFire;
			 _reloadSpeed = reloadSpeed;
			 _accuracy  = accuracy;
			 _numOfBullets = numOfBullets;
			 _clipSize  = clipSize;
			 _bulletSpeed = bulletSpeed;
			 _bulletSize = bulletSize;
		}
		

		// ************** Public ****************

		public int RateOfFire { 
			get { return _rateOfFire; }
		}
		public int ReloadSpeed { 
			get { return _reloadSpeed; }
		}
		public int Accuracy { 
			get { return _accuracy; }
		}
		public int NumOfBullets { 
			get { return _numOfBullets; }
		}
		public int ClipSize { 
			get { return _clipSize; }
		}
		public int BulletSpeed { 
			get { return _bulletSpeed; }
		}
		public int BulletSize { 
			get { return _bulletSize; }
		}

		[SerializeField] private int _rateOfFire;
		[SerializeField] private int _reloadSpeed;
		[SerializeField] private int _accuracy;
		[SerializeField] private int _numOfBullets;
		[SerializeField] private int _clipSize;
		[SerializeField] private int _bulletSpeed;
		[SerializeField] private int _bulletSize;

		public override void Add ( Gun statsToAdd ) {

			_rateOfFire 	+= statsToAdd.RateOfFire;
			_reloadSpeed 	+= statsToAdd.ReloadSpeed;
			_accuracy 		+= statsToAdd.Accuracy;
			_numOfBullets 	+= statsToAdd.NumOfBullets;
			_clipSize 		+= statsToAdd.ClipSize;
			_bulletSpeed 	+= statsToAdd.BulletSpeed;
			_bulletSize 	+= statsToAdd.BulletSize;
		}


		// ************** IStatBlockDelegate ****************

		List<Eden.UI.Elements.Building.Stat> Eden.UI.Elements.Building.IStatBlockDelegate.GetStats () {

			return new List<Eden.UI.Elements.Building.Stat>{ 

				new Eden.UI.Elements.Building.Stat( "Rate of Fire", null, RateOfFire ),
				new Eden.UI.Elements.Building.Stat( "Reload Speed", null, ReloadSpeed ),
				new Eden.UI.Elements.Building.Stat( "Accuracy", null, Accuracy ),
				new Eden.UI.Elements.Building.Stat( "Number Of Bullets", null, NumOfBullets ),
				new Eden.UI.Elements.Building.Stat( "Clip Size", null, ClipSize ),
				new Eden.UI.Elements.Building.Stat( "Bullet Speed", null, BulletSpeed ),
				new Eden.UI.Elements.Building.Stat( "Bullet Size", null, BulletSize ) 

			};
		}
	}
}