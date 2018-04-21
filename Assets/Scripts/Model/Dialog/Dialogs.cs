using System.Collections.Generic;
using Model.Dialog;

public struct Dialogs {

	public struct NPC {

		public struct Crafting {

			public static Sequence Hello {
				get {
					return new Sequence ( 
						new List<Sequence.Dialog> {
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Welcome to my shop!" ),
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Lets work really hard to make something good!!" )
						}
					);
				}
			}
			public static Sequence Goodbye {
				get {
					return new Sequence ( 
						new List<Sequence.Dialog> {
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Don't forget to come back if you think of something new to make" )
						}
					);
				}
			}
		}

		public struct Villager {

			public static Sequence Hype {
				get {
					return new Sequence ( 
						new List<Sequence.Dialog> {
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "OMG!" ),
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "HELLO!!" ),
							new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "I am so fucking hyped to be alive, aren't you?!" )
						}
					);
				}
			}
		}
	}

	public struct Sign {

		public static Sequence Hype {
			get {
				return new Sequence ( 
					new List<Sequence.Dialog> {
						new Sequence.Dialog( SpeakerInfo.Generic(), "I'M A FUCKING SIGN!" )
					}
				);
			}
		}
	}

	public struct Error {

		public static Sequence NullDialog {
			get {
				return new Sequence ( 
					new List<Sequence.Dialog> {
						new Sequence.Dialog( SpeakerInfo.ZeroTwo(), "Hmmmm i dont feel so good, maybe double check my path" )
					}
				);
			}
		}
	}
}
