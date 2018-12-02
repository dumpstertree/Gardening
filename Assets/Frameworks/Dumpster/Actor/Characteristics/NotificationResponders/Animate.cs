using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Characteristics {

	public class Animate : Dumpster.Characteristics.NotificationResponder {

		[SerializeField] private Animator2 _animator;
		[SerializeField] private string _identifier;
		[SerializeField] private float _animationLength;
		[SerializeField] private float _blendInTime;
		[SerializeField] private float _blendOutTime;

		private AnimationDampener _dampener;

		protected override void OnInit() {
			
			_dampener = new AnimationDampener( _animator, _identifier );
		}
		protected override void Respond () {

			_dampener.SetProgress( 1.0f, _animationLength );
			_dampener.SetWeight( 1.0f, _blendInTime );

			Game.GetModule<Async>()?.WaitForSeconds( _animationLength, () => {
				
				_dampener.SetWeight( 0.0f, _blendOutTime );
				
				Game.GetModule<Async>()?.WaitForSeconds( _blendOutTime, () => {
					_dampener.SetProgress( 0.0f );
				});
			});
		}
	}
}