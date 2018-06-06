using UnityEngine;

public class PlayerPhysicsAnimator : Dumpster.Physics.Animator {

	[SerializeField] private Animator _animator;

	private const float DEAD_ZONE = 0.05f;
	private const string JUMPING_NAME = "Jumping";
	private const string FALLING_NAME = "Falling";
	
	public override void Animate( Dumpster.Physics.Controller.Package package ) {
		
		if ( !package.DownIsColliding ) {
		
			if ( package.Velocity.y > 0 ) {
				_animator.SetBool( JUMPING_NAME, true );
				_animator.SetBool( FALLING_NAME, false );
				return;
			}
			
			if ( package.Velocity.y < 0) {
				_animator.SetBool( JUMPING_NAME, false );
				_animator.SetBool( FALLING_NAME, true );
				return;
			}
		}
		else {

			_animator.SetBool( JUMPING_NAME, false );
			_animator.SetBool( FALLING_NAME, false );
		}
	}
}
