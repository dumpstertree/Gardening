using UnityEngine;

namespace Eden.Templates {
	
	public class GunBuildableItem : BuildableItem {

		public override Eden.Model.Item CreateInstance () { 

			var blocks = new char[,]{};
			var stats = RollForStats();
			var part = new Eden.Model.Building.Parts.Gun( blocks, stats, _id );
			
			return new Eden.Model.GunBuildableItem( _id, _displayName, _maxCount, _expendable, _sprite, part );
		}
		public Eden.Model.Item ReloadInstance ( Eden.Model.Building.Stats.Gun stats ) {

			var blocks = new char[,]{};
			var part = new Eden.Model.Building.Parts.Gun( blocks, stats, _id );
			
			return new Eden.Model.GunBuildableItem( _id, _displayName, _maxCount, _expendable, _sprite, part );
		}



		[Header( "Stats" )]
		[SerializeField] private int _minRateOfFireExperience;
		[SerializeField] private int _maxRateOfFireExperience;
		[Space( 15 )]
		[SerializeField] private int _minReloadSpeedExperience;
		[SerializeField] private int _maxReloadSpeedExperience;
		[Space( 15 )]
		[SerializeField] private int _minAccuracyExperience;
		[SerializeField] private int _maxAccuracyExperience;
		[Space( 15 )]
		[SerializeField] private int _minNumOfBulletsExperience;
		[SerializeField] private int _maxNumOfBulletsExperience;
		[Space( 15 )]
		[SerializeField] private int _minClipSizeExperience;
		[SerializeField] private int _maxClipSizeExperience;
		[Space( 15 )]
		[SerializeField] private int _minBulletSpeedExperience;
		[SerializeField] private int _maxBulletSpeedExperience;
		[Space( 15 )]
		[SerializeField] private int _minBulletSizeExperience;
		[SerializeField] private int _maxBulletSizeExperience;

		private Eden.Model.Building.Stats.Gun RollForStats () {

			var rateOfFire = Roll( _minRateOfFireExperience, _maxRateOfFireExperience );
			var reloadSpeed = Roll( _minReloadSpeedExperience, _maxReloadSpeedExperience );
			var accuracy = Roll( _minAccuracyExperience, _maxAccuracyExperience );
			var numOfBullets = Roll( _minNumOfBulletsExperience, _maxNumOfBulletsExperience );
			var clipSize = Roll( _minClipSizeExperience, _maxClipSizeExperience );
			var bulletSpeed = Roll( _minBulletSpeedExperience, _maxBulletSpeedExperience );
			var bulletSize = Roll( _minBulletSizeExperience, _maxBulletSizeExperience );

			return new Eden.Model.Building.Stats.Gun( rateOfFire, reloadSpeed, accuracy, numOfBullets, clipSize, bulletSpeed, bulletSize );
		}
		private int Roll ( float min, float max ) {

			var roll = Random.Range( 0f, 1f );
			return Mathf.FloorToInt( Mathf.Lerp( min, max, roll * roll ) );
		}
	}
}