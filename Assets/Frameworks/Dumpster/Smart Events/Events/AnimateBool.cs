using UnityEngine;

namespace Dumpster.Events {

	public class AnimateBool : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private string _boolName;
		[SerializeField] private bool _value;
		[SerializeField] private Animator _animator;
		
	
		//***************** Public *******************
		
		public override void EventTriggered() {
			
			_animator.SetBool( _boolName, _value );
		}
	}
}