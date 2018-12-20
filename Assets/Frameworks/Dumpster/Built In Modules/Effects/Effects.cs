using Dumpster.Core;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dumpster.BuiltInModules {

	[CreateAssetMenu(menuName = "Dumpster/Modules/Effects")]	
	public class Effects : Module {
		
		public void RegisterShakableForFrame ( Shakable shakable ) {
			
			_shakables.Add( shakable );
		}
		public void Shake ( Vector3 position, ShakePower power, DecayRate rate )  {

			_startPos = position;
			
			switch ( power ) {
				case ShakePower.Miniscule:
					_magnitude += 0.05f;
					break;
				case ShakePower.Light:
					_magnitude += 0.2f;
					break;
				case ShakePower.Medium :
					_magnitude += 0.8f;
					break;
				case ShakePower.Heavy :
					_magnitude += 2;
					break;
				case ShakePower.Colossal:
					_magnitude += 4;
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
					_decay = 0.5f;
					break;
				case DecayRate.Long :
					_decay = 0.8f;
					break;
			}
		}
		public void FreezeFrame ( FreezeFrameDuration duration ) {
			
			var time = 0f;

			switch( duration ) {
				case FreezeFrameDuration.Quick:
					time = 0.1f;
					break;
				case FreezeFrameDuration.Medium:
					time = 0.25f;
					break;
				case FreezeFrameDuration.Long:
					time = 0.5f;
					break;
			}

			_game.StartCoroutine( FreezeFrameCoroutine(time) );
		}
	
	
		IEnumerator FreezeFrameCoroutine ( float time ) {

			Time.timeScale = 0f;

			yield return new WaitForSecondsRealtime ( time );
			
			Time.timeScale = 1f;
		}	

		private List<Shakable>_shakables = new List<Shakable>();
		
		private float _magnitude;
		private float _decay;
		private float _maxDistance = 30;
		private Vector3 _startPos;

		protected override void OnLateUpdate () {

			_magnitude = _magnitude * _decay;
			
			if ( _magnitude > 0.001 ) {
				
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

	public enum FreezeFrameDuration {
		Quick,
		Medium,
		Long
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