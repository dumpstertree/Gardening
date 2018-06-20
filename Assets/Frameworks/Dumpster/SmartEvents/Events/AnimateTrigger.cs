using UnityEngine;

namespace Dumpster.Events {

	public class AnimateTrigger : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private string _animationTrigger;
		[SerializeField] private Animator _animator;
		
	
		//***************** Public *******************
		
		public override void EventTriggered() {
			
			_animator.SetTrigger( _animationTrigger );
		}
	}
}