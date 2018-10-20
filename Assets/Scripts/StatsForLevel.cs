using UnityEngine;

namespace Eden.Model {

	public class StatsForLevel : MonoBehaviour {


		[SerializeField] private AnimationCurve _rateOfFireCurve;
		[SerializeField] private AnimationCurve _reloadSpeedCurve;
		[SerializeField] private AnimationCurve _accuracyCurve;
		[SerializeField] private AnimationCurve _numOfBulletsCurve;
		[SerializeField] private AnimationCurve _clipSizeCurve;
		[SerializeField] private AnimationCurve _bulletSpeedCurve;
		[SerializeField] private AnimationCurve _bulletSizeCurve;

		private const int MAX_LEVEL = 10;

		public float RateOfFire ( int level ) {
			return 1f / GetValueFromCurve( _rateOfFireCurve, level );
		}
		public float ReloadSpeed ( int level ) {
			return GetValueFromCurve( _reloadSpeedCurve, level );
		}
		public float Accuracy ( int level ) {
			return GetValueFromCurve( _accuracyCurve, level );
		}
		public int NumOfBullets ( int level ) {
			return Mathf.RoundToInt( GetValueFromCurve( _numOfBulletsCurve, level ) );
		}
		public int ClipSize ( int level ) {
			return Mathf.RoundToInt( GetValueFromCurve( _clipSizeCurve, level ) );
		}
		public float BulletSpeed ( int level ) {
			return GetValueFromCurve( _bulletSpeedCurve, level );
		}
		public float BulletSize ( int level ) {
			return GetValueFromCurve( _bulletSizeCurve, level );
		}


		private float GetValueFromCurve ( AnimationCurve curve, int level ) {

			var frac = level/MAX_LEVEL;
			var val = curve.Evaluate( frac );

			return val;
		}
	}
}