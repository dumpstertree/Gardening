using Dumpster.Characteristics;
using Dumpster.Core;
using Dumpster.BuiltInModules;
using Dumpster.Tweening;
using Eden.Characteristics;
using UnityEngine;

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

		private Actor _lastTarget;

		
		private Actor _actor {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<Actor>(); }
		}


		private void Update () {

			var target = _actor.GetCharacteristic<Targeter>( true )?.GetBestTarget();
			if ( target != null && target.Actor.GetCharacteristic<Eden.Characteristics.Targetable>().ShowUI ) {
				
				if ( target.Actor != _lastTarget && target != null ) {
					SetHealthFill( 
						target.Actor.GetCharacteristic<Health>( true ).Current, 
						target.Actor.GetCharacteristic<Health>( true ).Max, 
						true 
					);
				}

				SetVisualVisible( true );
				SetUIPosition( target.transform.position );
				SetHealthFill( target.Actor.GetCharacteristic<Health>( true ).Current, target.Actor.GetCharacteristic<Health>( true ).Max, false );
			
			} else {

				SetVisualVisible( false );
			}

			if ( target != null ) {
				_lastTarget = target.Actor;
			}
		}


		private void SetUIPosition ( Vector3 worldPos ) {

			var pos = Camera.main.WorldToScreenPoint( worldPos );
			_visual.position = Vector3.Lerp( _visual.position, pos, 0.8f );
		}
		private void SetHealthFill ( int currentHealth, int maxhealth, bool instant ) {

			var progress = (float)currentHealth / (float)maxhealth;
			_healthFill.fillAmount = Mathf.Lerp( _healthFill.fillAmount, progress, instant ? 1f : 0.2f );
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
