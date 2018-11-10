using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core {
	
	public class WeightBlender {
	 

	 	// ******************* Constructor *************************
		
		public WeightBlender ( List<IBlendable> blendables, IBlendable defualtBlendable = null ) {
	 		
	 		_blendables = blendables;
	 		_defualtBlendable = defualtBlendable;
	 	}
	 	
		
		// ******************* Public *************************
	 	
	 	public void Blend () {
			
			foreach ( IBlendable b in _blendables) {

				if (_weightTotal < 1f) {
					b.SetWeight( b.CurvedWeight );
				} else {
					b.SetWeight( (_weightTotal > 0f ) ? b.CurvedWeight/_weightTotal : 0f );
				}
			}


			// if a defualt exists recalculate its blended weight
			if ( _defualtBlendable != null ) {

				var remainingWeight = Mathf.Clamp01( 1f - _weightTotal );

				_defualtBlendable.TrueWeight = remainingWeight;
				_defualtBlendable.SetWeight( remainingWeight );
			}
	 	}
	 	public void SetWeight ( IBlendable blendable, float weight ) {


	 		// if blendable is not part of the weight blender abort
	 		if ( !_blendables.Contains( blendable ) ) {

	 			Debug.LogError( "Trying to blend a blendable that was not added as part of the WeightBlender!" );
	 			return;
	 		}

				
			// change blendable weight
			blendable.TrueWeight = weight;

			Blend();
		}


		// ******************* Private *************************

		readonly private List<IBlendable> _blendables;
		readonly private IBlendable _defualtBlendable;

		private float _weightTotal {
			get { var t = 0f; foreach ( IBlendable w in _blendables ){ t += w.CurvedWeight; } return t; }
		}
	}

	public interface IBlendable {
		
		float TrueWeight { get; set; }
		float CurvedWeight { get; }

		void SetWeight ( float newWeight );
	}
}