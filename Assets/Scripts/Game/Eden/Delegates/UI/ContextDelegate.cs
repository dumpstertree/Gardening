using Dumpster.Core.BuiltInModules.UI;
using UnityEngine;
using System.Collections.Generic;

namespace Eden.UI {
	
	public class ContextDelegate : MonoBehaviour, IContextDelgate {

		Context IContextDelgate.GetContext ( string forContextIdentifier ) {

			switch ( forContextIdentifier ) {
				
				case EdensGarden.Constants.UIContexts.None: 
					return null;
				
				case EdensGarden.Constants.UIContexts.Player:
					return GetPlayerContext ();

				case EdensGarden.Constants.UIContexts.Dialog:
					return GetDialogContext ();

				case EdensGarden.Constants.UIContexts.Inventory:
					return GetInventoryContext ();

				default:
					return null;

			} 
		}
		void IContextDelgate.ReturnContext ( Context context ) {

			context.Destroy();
		}

		
		// ******************* Context *******************

		private Context GetPlayerContext () {
			return new Context( EdensGarden.Constants.UIContexts.Player, new List<Panel>{ 
				ConditionForCanvas( _menuButtonsPanel ) 
			});
		}
		private Context GetDialogContext () {
			return null;
		}
		private Context GetInventoryContext () {
			return new InteractiveContext( EdensGarden.Constants.UIContexts.Inventory, EdensGarden.Constants.InputLayers.InventoryUI, new List<Panel>{ 
				ConditionForCanvas( _inventoryPanel ) 
			});
		}


		// ******************* Panels *******************

		private Panel _menuButtonsPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "MenuButtonsUIPanel" ) ).GetComponent<Panel>();  }
		}
		private Panel _inventoryPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "InventoryUIPanel" ) ).GetComponent<Panel>(); }
		}


		// ******************* Private *******************
		
		[SerializeField] private Canvas _canvas;

		private Panel ConditionForCanvas ( Panel panel ) {

			panel.transform.SetParent( _canvas.transform, false );
			return panel;
		}
	}
}