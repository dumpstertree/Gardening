using Dumpster.Core;
using Dumpster.BuiltInModules;
using Eden.Life;
using UnityEngine;
using Dumpster.Tweening;

namespace Eden.UI.Panels {

	public class Targeting :  Dumpster.BuiltInModules.Panel {

		protected override void OnInit () {
			
			_visual.localScale = Vector3.zero;
			_visualWasVisible = false;
		}


		[SerializeField] private Transform _visual;
		[SerializeField] private UnityEngine.UI.Image _healthFill;

		
		private bool _visualWasVisible;
		private Tween _presentation;

		
		private BlackBox _blackBox {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<BlackBox>(); }
		}


		private void Update () {

			var target = _blackBox.Visual.Target;
			if ( target != null ) {

				var visual = target.Visual;

				SetVisualVisible( true );
				SetUIPosition( target.transform.position );
				SetHealthFill( visual.CurrentHealth, visual.MaxHealth );
			
			} else {

				SetVisualVisible( false );
			}
		}


		private void SetUIPosition ( Vector3 worldPos ) {

			var pos = Camera.main.WorldToScreenPoint( worldPos );
			_visual.position = Vector3.Lerp( _visual.position, pos, 0.8f );
		}
		private void SetHealthFill ( int currentHealth, int maxhealth ) {

			var progress = (float)currentHealth / (float)maxhealth;
			_healthFill.fillAmount = progress;
		}
		private void SetVisualVisible ( bool visible ) {

			if ( _visualWasVisible && !visible ) {

				if ( _presentation != null ) _presentation.Kill();

				_presentation = Tween.Vector3( 
					setter: v => { _visual.localScale = v; }, 
					startValue: _visual.localScale, 
					targetValue: Vector3.zero, 
					time: 0.1f 
				);
			}
			if ( !_visualWasVisible && visible ) {

				if ( _presentation != null ) _presentation.Kill();

				_presentation = Tween.Vector3( 
					setter: v => { _visual.localScale = v; }, 
					startValue: _visual.localScale, 
					targetValue: Vector3.one, 
					time: 0.1f 
				);
			}

			_visualWasVisible = visible;
		}
	}
}
