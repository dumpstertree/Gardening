using System.Collections.Generic;

namespace Eden.Model.Building.Stats {

	public class Gun : Stats<Gun>, Eden.UI.Elements.Building.IStatBlockDelegate {

		
		// ************** Constructors ****************
		
		public Gun () {

			RateOfFire = 1;
			ReloadSpeed = 1;
			Accuracy = 1;
			NumOfBullets = 1;
			ClipSize = 1;
			BulletSpeed = 1;
			BulletSize = 1;
		}
		public Gun ( int rateOfFire, int reloadSpeed, int accuracy, int numOfBullets, int clipSize, int bulletSpeed, int bulletSize ) {

			RateOfFire = rateOfFire;
			ReloadSpeed = reloadSpeed;
			Accuracy = accuracy;
			NumOfBullets = numOfBullets;
			ClipSize = clipSize;
			BulletSpeed = bulletSpeed;
			BulletSize = bulletSize;
		}
		

		// ************** Public ****************

		public int RateOfFire { 
			get; private set; 
		}
		public int ReloadSpeed { 
			get; private set; 
		}
		public int Accuracy { 
			get; private set; 
		}
		public int NumOfBullets { 
			get; private set; 
		}
		public int ClipSize { 
			get; private set; 
		}
		public int BulletSpeed { 
			get; private set; 
		}
		public int BulletSize { 
			get; private set; 
		}

		public override void Add ( Gun statsToAdd ) {

			RateOfFire   += statsToAdd.RateOfFire;
			ReloadSpeed  += statsToAdd.ReloadSpeed;
			Accuracy 	 += statsToAdd.Accuracy;
			NumOfBullets += statsToAdd.NumOfBullets;
			ClipSize 	 += statsToAdd.ClipSize;
			BulletSpeed  += statsToAdd.BulletSpeed;
			BulletSize   += statsToAdd.BulletSize;
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