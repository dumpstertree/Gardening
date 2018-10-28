using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Model.Constants {

	[System.Serializable]
	public class RangedWeapons {

		public float RateOfFire ( int level ) {
			return Mathf.Lerp( _minRateOfFire, _maxRateOfFire, (float)level/(float)_maxLevel );
		}
		public float ReloadSpeed ( int level ) {
			return Mathf.Lerp( _minReloadSpeed, _maxReloadSpeed, (float)level/(float)_maxLevel );
		}
		public float Accuracy ( int level ) {
			return Mathf.Lerp( _minAccuracy, _maxAccuracy, (float)level/(float)_maxLevel );
		}
		public float BulletSpeed ( int level ) {
			return Mathf.Lerp( _minBulletSpeed, _maxBulletSpeed, (float)level/(float)_maxLevel );
		}
		public float BulletSize ( int level ) {
			return Mathf.Lerp( _minBulletSize, _maxBulletSize, (float)level/(float)_maxLevel );
		}
		public int NumOfBullets ( int level ) {
			return Mathf.RoundToInt( Mathf.Lerp( _minNumOfBullets, _maxNumOfBullets, (float)level/(float)_maxLevel ) );
		}
		public int ClipSize ( int level ) {
			return Mathf.RoundToInt( Mathf.Lerp( _minClipSize, _maxClipSize, (float)level/(float)_maxLevel ) );
		}

		[SerializeField] private int _maxLevel;
		[Space]
		[SerializeField] private float _minRateOfFire;
		[SerializeField] private float _maxRateOfFire;
		[Space]
		[SerializeField] private float _minReloadSpeed;
		[SerializeField] private float _maxReloadSpeed;
		[Space]
		[SerializeField] private float _minAccuracy;
		[SerializeField] private float _maxAccuracy;
		[Space]
		[SerializeField] private float _minBulletSize;
		[SerializeField] private float _maxBulletSize;
		[Space]
		[SerializeField] private float _minBulletSpeed;
		[SerializeField] private float _maxBulletSpeed;
		[Space]
		[SerializeField] private int _minClipSize;
		[SerializeField] private int _maxClipSize;
		[Space]
		[SerializeField] private int _minNumOfBullets;
		[SerializeField] private int _maxNumOfBullets;
	}
}
