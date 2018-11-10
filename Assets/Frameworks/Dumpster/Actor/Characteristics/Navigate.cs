using Dumpster.BuiltInModules;
using Dumpster.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core {

	public class Navigate : Dumpster.Core.Characteristic {

		private const string CHANGE_AREA = "Navigate.ChangeArea";

		public void ChangeArea ( string areaID, string doorID, string transitionTag ) {

			_actor.PostNotification( CHANGE_AREA );
			Game.GetModule<Navigation>()?.ChangeArea( areaID, doorID, transitionTag );
		}

		public override List<string> GetNotifications () {
			
			return new List<string>(){ 
				CHANGE_AREA 
			};
		}
	}
}