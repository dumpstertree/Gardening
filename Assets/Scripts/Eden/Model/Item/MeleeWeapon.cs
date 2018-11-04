using Eden.Interactors.Melee;
using Eden.Life.Chips;
using Eden.Model.Interactable;
using System;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Eden.Characteristics;

namespace Eden.Model {
		
	public class MeleeWeapon : Item {

		// ******************** Protected *********************

		public MeleeWeapon( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite, Slash[] swingPrefabs ) 
		:  base ( prefabID, displayName, maxCount, expendable, sprite ) {

			_swingPrefabs = swingPrefabs;
		}


		protected override void OnUse( Dumpster.Core.Actor actor, Action onComplete ) {

			var melee = actor.GetCharacteristic<CanUseMeleeItems>( true );
			
			if ( melee != null ) {
				StartSwing( actor, onComplete );
			} else {
				onComplete ();
			}
		}

		
		// ******************** Private *********************

		private Slash[] _swingPrefabs;

		private int _combo =  0;
		private float _comboConnectTime = 0.2f;
		private float _comboCooldown = 0.5f;
		private bool _canConnectCombo = false;

		private bool _hasMoreCombos {
			get{ return _combo + 1 < _swingPrefabs.Length; } 
		}


		private void StartSwing ( Dumpster.Core.Actor actor, Action onComplete ) {

			// update combo
			_combo = GetComboIndex();
			
			// get new swing prefab
			var prefab = _swingPrefabs[ _combo ];

			// create the swing
			CreateSwing( prefab, actor, () => EndSwing( actor, onComplete ) );
		}
		
		private void EndSwing ( Dumpster.Core.Actor actor, Action onComplete ) {

			// allow for a combo
			if ( _hasMoreCombos ) {
				AllowForCombo ();
			}

			// on complete
			onComplete ();
		}
		private void AllowForCombo () {

			_canConnectCombo = true;
			Game.GetModule<Async>()?.WaitForSeconds( _comboConnectTime, () => _canConnectCombo = false );
		}
		private int GetComboIndex () {

			return ( _canConnectCombo && _hasMoreCombos ) ? _combo + 1 : 0;
		}

		private void CreateSwing ( Slash swingPrefab, Dumpster.Core.Actor actor, Action endSwing ) {

			var swing = GameObject.Instantiate( swingPrefab );
			var hit = new Hit( null, 1 );

			swing.Set( actor, hit, _combo, endSwing );
		}
	}
}