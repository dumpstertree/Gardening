using UnityEngine;

namespace Eden.Modules {

	[CreateAssetMenu( menuName = "Eden/Module/Constants" )]
	public class Constants : Dumpster.Core.Module {

		public Eden.Model.Constants.Paths Paths {
			get{ return _paths; }
		}
		public Eden.Model.Constants.RangedWeapons RangedWeapons {
			get{ return _rangedWeapons; }
		}
		public Eden.Model.Constants.InputLayers InputLayers {
			get{ return _inputLayers; }
		}
		public Eden.Model.Constants.UIContexts UIContexts {
			get{ return _uiContexts; }
		}
		public Eden.Model.Constants.UILayers UILayers {
			get{ return _uiLayers; }
		}
		
		[SerializeField] private Eden.Model.Constants.RangedWeapons _rangedWeapons;
		[SerializeField] private Eden.Model.Constants.Paths _paths;
		[SerializeField] private Eden.Model.Constants.InputLayers _inputLayers;
		[SerializeField] private Eden.Model.Constants.UIContexts _uiContexts;
		[SerializeField] private Eden.Model.Constants.UILayers _uiLayers;

		[SerializeField] private bool _useDialogs;
		public bool UseDialogs {
			get { return _useDialogs; }
		}
	}
}