﻿using Dumpster.Core.BuiltInModules.UI;
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

			return new InteractiveContext( 
				
				EdensGarden.Constants.UIContexts.Player, 
				EdensGarden.Constants.InputLayers.Player, 
				
				new List<InteractivePanel>{
				}, 
				new List<Panel>{ 
					ConditionForCanvas( _hudUIPanel ),
					ConditionForCanvas( _menuButtonsPanel ),
					ConditionForCanvas( _quickslotPanel ) 
				}
			);
		}
		private Context GetDialogContext () {
			
			return new InteractiveContext( 
				
				EdensGarden.Constants.UIContexts.Dialog, 
				EdensGarden.Constants.InputLayers.ForegroundUI, 
				
				new List<InteractivePanel>{
					ConditionForCanvas( _dialogPanel )
				}, 
				new List<Panel>{ 
				}
			);
		}
		private Context GetInventoryContext () {
			return new InteractiveContext( 
				
				EdensGarden.Constants.UIContexts.Inventory, 
				EdensGarden.Constants.InputLayers.InventoryUI, 
				
				new List<InteractivePanel>{
					ConditionForCanvas( _inventoryPanel ) 
				}, 
				new List<Panel>{ 
				}
			);
		}


		// ******************* Panels *******************

		private Panel _menuButtonsPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "MenuButtonsUIPanel" ) ).GetComponent<Panel>();  }
		}
		private Panel _quickslotPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "QuickslotUIPanel" ) ).GetComponent<Panel>(); }
		}
		private Panel _hudUIPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "HUD" ) ).GetComponent<Panel>(); }
		}
		
		

		// *************** Interactive Panels *******************

		private InteractivePanel _inventoryPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "InventoryUIPanel" ) ).GetComponent<InteractivePanel>(); }
		}
		private InteractivePanel _dialogPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "DialogUIPanel" ) ).GetComponent<InteractivePanel>(); }
		}
		


		// ******************* Private *******************
		
		[SerializeField] private Canvas _canvas;

		private Panel ConditionForCanvas ( Panel panel ) {

			panel.transform.SetParent( _canvas.transform, false );
			return panel;
		}
		private InteractivePanel ConditionForCanvas ( InteractivePanel panel ) {

			panel.transform.SetParent( _canvas.transform, false );
			return panel; 
		}		
	}
}