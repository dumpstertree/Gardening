using Dumpster.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dumpster.BuiltInModules {

	[CreateAssetMenu(menuName = "Dumpster/Modules/Effects")]	
	public class Effects : Module {

		private GameObject _smoke {
			get{ return Resources.Load( "SmokeEffect" ) as GameObject; }
		}
		private GameObject _fireworks {
			get{ return Resources.Load( "FireworksEffect" ) as GameObject; }
		}
		private GameObject _hit {
			get{ return Resources.Load( "HitEffect" ) as GameObject; }
		}
		private GameObject _faint {
			get{ return Resources.Load( "FaintEffect" ) as GameObject; }
		}
		private GameObject _wakeUp {
			get{ return Resources.Load( "WakeUpEffect" ) as GameObject; }
		}

		public void OneShot( ParticleType type, Vector3 position, Quaternion rotation ) {

			GameObject prefab = null;

			switch( type ) {
				
				case ParticleType.None:
					return;

				case ParticleType.Fireworks:
					prefab =_fireworks;
					break;

				case ParticleType.Hit:
					prefab = _hit;
					break;

				case ParticleType.Smoke:
					prefab = _smoke;
					break;

				case ParticleType.Faint:
					prefab = _faint;
					break;

				case ParticleType.WakeUp:
					prefab = _wakeUp;
					break;
			}

			if ( prefab != null ) {
				
				var go = Instantiate( prefab );
				go.transform.position = position;
				go.transform.rotation = rotation;
			}
		}
		public void FreezeFrame ( float time ) {

			_game.StartCoroutine( FreezeFrameCoroutine(time) );
		}
		IEnumerator FreezeFrameCoroutine ( float time ) {

			Time.timeScale = 0f;

			for ( float t=0; t<time; t+=Time.unscaledDeltaTime ){
				Time.timeScale = t/time;
				yield return null;
			}
			// yield return new WaitForSecondsRealtime ( time );
			
			Time.timeScale = 1f;
		}	

		private List<Shakable>_shakables = new List<Shakable>();
		
		

		private float _magnitude;
		private float _decay;
		private float _maxDistance = 30;
		private Vector3 _startPos;

		public void Shake ( Vector3 position, ShakePower power, DecayRate rate )  {

			_startPos = position;
			
			switch ( power ) {
				case ShakePower.Miniscule:
					_magnitude += 1;
					break;
				case ShakePower.Light:
					_magnitude += 2;
					break;
				case ShakePower.Medium :
					_magnitude += 3;
					break;
				case ShakePower.Heavy :
					_magnitude += 4;
					break;
				case ShakePower.Colossal:
					_magnitude += 5;
					break;
				case ShakePower.PlanetShattering:
					_magnitude += 6;
					break;
			}

			switch ( rate ) {
				case DecayRate.Quick :
					_decay = 0.2f;
					break;
				case DecayRate.Medium:
					_decay = 0.4f;
					break;
				case DecayRate.Long :
					_decay = 0.6f;
					break;
			}
		}
		public void RegisterShakableForFrame ( Shakable shakable ) {
			
			_shakables.Add( shakable );
		}
		public void LateUpdate () {

			_magnitude = _magnitude * _decay;
			
			if ( _magnitude > 0.01 ) {
				
				foreach ( Shakable s in _shakables ) {
					
					var d = Vector3.Distance( s.transform.position, _startPos ); 
					if ( d < _maxDistance ) {

						var distDecay = 1 - d/_maxDistance;
						s.Shake( _magnitude * distDecay );
					}
				}
			}

			_shakables.Clear();
		}
	}
	public enum ShakePower {
		
		Miniscule,
		Light,
		Medium,
		Heavy,
		Colossal,
		PlanetShattering
	}
	
	public enum DecayRate {
		Quick,
		Medium,
		Long
	}

	public enum ParticleType {
		None,
		Fireworks,
		Hit,
		Smoke,
		Faint,
		WakeUp
	}
}