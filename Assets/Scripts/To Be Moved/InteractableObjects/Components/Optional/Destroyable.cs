/*using UnityEngine;
using Dumpster.Core.BuiltInModules.Effects;

namespace Interactable.OptionalComponent {

	[RequireComponent(typeof(Animator))]
	public class Destroyable : MonoBehaviour {

		// ***************** PUBLIC ***************

		public void Destroy () {

			if ( !_beingDestroyed ){

				Destroy( GetComponent<InteractableObject>() );
				
				_beingDestroyed = true;
				_animator.SetTrigger( DESTROY_ANIMATION_TRIGGER );

				EdensGarden.Instance.Async.WaitForSeconds( ANIMATION_LENGTH, () => {

					EdensGarden.Instance.Effects.OneShot( _effectType, transform.position, transform.rotation );

					if ( _itemDropper ) {
						_itemDropper.DropItems();
					}

					Destroy( gameObject );
				});
			}
		}

		// ***************** PRIVATE ***************

		[SerializeField] private ParticleType _effectType;

		private const string DESTROY_ANIMATION_TRIGGER = "Destroy";
		private const float ANIMATION_LENGTH = 0.4f;

		private bool _beingDestroyed;
		private Animator _animator;
		private ItemDropper _itemDropper;

		// ***************************************

		private void Awake () {

			_animator = GetComponent<Animator>();
			_itemDropper = GetComponent<ItemDropper>();

			_beingDestroyed = false;
		}
	}
}*/