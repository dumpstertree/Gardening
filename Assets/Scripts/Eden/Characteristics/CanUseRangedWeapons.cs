using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

namespace Eden.Characteristics {

	public class CanUseRangedWeapons : Characteristic {

			
		// ************** Data ***************
		
		private enum ForwardDirecton {
			Local,
			Camera
		}


		// ************** Public ***************

		public Vector3 GetSpawnLocation () {
			return _projectileSpawner.position;
		}
		public Collider[] GetForbiddenColliders () {
			return _actor.GetComponentsInChildren<Collider>();
		}
		public Vector3 GetLookingDirection () {

			switch( _forwardDirection ) {
				case ForwardDirecton.Local: return _projectileSpawner.forward;
				case ForwardDirecton.Camera: return UnityEngine.Camera.main.transform.forward;
			}

			return Vector3.zero;
		}
		public void Animate ( System.Action onComplete ) {

			if ( _animator != null ) {

				_damper.SetProgress( 1.0f, _animationTime );
				_damper.SetWeight( 1.0f, 0.1f );

				Game.GetModule<Async>()?.WaitForSeconds( _animationTime * _fireAtAnimationProgress, onComplete );
				Game.GetModule<Async>()?.WaitForSeconds( _animationTime, 
					()=> { 
						_damper.SetWeight( 0.0f, 0.1f ); 
						_damper.SetProgress( 0.0f, 0.0f ); 
				});

			} else {

				onComplete ();
			}
		}
		
		// ************** Protected ***************
		
		protected override void OnInit() {
			
			_damper = new AnimationDampener( _animator, _animationIdentifier );
			_forbiddenColliders = _actor.GetComponentsInChildren<Collider>();
		}

		// ************** Private ***************

		[Header( "Spawning" )]
		[SerializeField] private Transform _projectileSpawner;
		[SerializeField] private ForwardDirecton _forwardDirection;
		
		[Header( "Animation" )]
		[SerializeField] private Animator2 _animator;
		[SerializeField] private string _animationIdentifier;
		[SerializeField] private float _animationTime;
		[SerializeField] private float _fireAtAnimationProgress;	

		private Collider[] _forbiddenColliders;
		private AnimationDampener _damper;
	}
}