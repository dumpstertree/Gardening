

public partial class InventoryItem {
	
	public enum UseAnimation {
		Swing,
		Plant,
		Feed,
		PickUp,
		Dig
	}

	private struct UseAnimationData {
	
		public float AnimationLength{ get; }
		public float AnimationUseFraction{ get; }
		public string AnimationTrigger{ get; }
		
		public UseAnimationData ( float length, float fraction, string trigger ) {
			AnimationLength = length;
			AnimationUseFraction = fraction;
			AnimationTrigger = trigger;
		}
	}

	private static UseAnimationData GetUseAnimationData ( UseAnimation animation ){

		switch( animation ) {
			
			case UseAnimation.Swing:
				return new UseAnimationData( 1.0f, 0.5f, "Swing" );
			
			case UseAnimation.Plant:
				return new UseAnimationData( 1.0f, 0.5f, "Place" );
			
			case UseAnimation.PickUp:
				return new UseAnimationData( 1.0f, 0.5f, "Pickup" );
			
			case UseAnimation.Dig:
				return new UseAnimationData( 1.0f, 0.5f, "Dig" );
			
			default:
				return new UseAnimationData( 1.0f, 0.5f, "" );
		}
	}
}