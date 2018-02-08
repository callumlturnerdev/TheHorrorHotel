using UnityEngine;
using System.Collections;

///Below you will find an example of how to add uNote notes into your custom scripts.
///Before continuing, it's important to note that the below example class is contained inside of the 
///UnfinityGames.uNote namespace, so as to not clutter up your global namespace.
///When adding uNote Notes to your custom objects, you will have to include the UnfinityGames.uNote namespace.
///It should look like this:
///
///using UnfinityGames.uNote;
///
///After including the above namespace in your script, you're ready to add notes!

namespace UnfinityGames.uNote
{
	public class uNoteHeroExample : MonoBehaviour
	{
		//All it takes to add a note is the below two lines of code.  The [Note] attribute handles turning our
		//NoteData object into a visual Note object you can interact with.
		//
		//[Note]
		//public NoteData note;
		//
		//Below we use these two lines of code to create a note about our Hero.  The only difference is that we're 
		//pre-populating our note with a Title and Text (which is optional).
		//We use strings from an ExampleStrings class to keep things tidy and readable.

		[Note(ExampleStrings.HeroNoteTitle, ExampleStrings.HeroNote)]
		public NoteData note;

		//Below are some example fields showing how you can build out a class containing uNote Notes.
		//They are not needed to add Notes to your objects.

		public enum HeroRace
		{
			Human,
			Elf,
			Dwarf,
			Goblin
		}
		public HeroRace Race = HeroRace.Human;

		public enum HeroClass
		{
			Warrior,
			Archer,
			Wizard,
			Jester
		}
		public HeroClass Class = HeroClass.Warrior;

		public string HeroName = "Player 1";

		[Range(0, 100)]
		public float HeroHealth = 100;

		[Range(0, 100)]
		public float HeroAttackStrength = 3;

		[Range(0, 100)]
		public float HeroDefenseRating = 10;

		[Range(0, 100)]
		public float HeroEncumbrance = 0;

		//You can include multiple notes in a single script.
		[Note(ExampleStrings.BottomNoteTitle, ExampleStrings.BottomNote)]
		public NoteData bottomNote;

		//Note that the text string has been separated into smaller chunks, so it can be read in source code form
		//more easily.  The separation is not required, and does not change how the note is displayed.
		[Note("This title is set inside of the uNoteHeroExample script.", "Note titles and text can be configured " +
															"inside of scripts, as well as in the inspector.  " +
															"<b>Rich</b> <i>Text</i> also works when text " +
															"is set in scripts.", true)]
		public NoteData scriptNote;
	}

	//This class is *not* needed to create your notes.  
	//It's only storing strings that we use to populate our example notes.
	internal class ExampleStrings
	{
		public const string HeroNoteTitle = "This is a Hero.";
		public const string HeroNote = "Heroes can either be player-controlled, or computer controlled (NPCs or AI-based enemies).\n\n" +
								 "<b>Race:</b> Can be a Human, Elf, Dwarf or Goblin.\n\n" +
								 "<b>Hero Class:</b> Can be a Warrior, Archer, Wizard or Jester.\n\n" +
								 "<b>Hero Name:</b> The name of this hero.\n\n" +
								 "<b>Hero Health:</b> Represents how much health our hero currently has, represented as a float value between <color=red>0</color> and <color=green>100</color>.\n\n" +
								 "<b>Hero Attack Strength:</b> Represents how much damage our hero will do to enemies, represented as a float value between <color=red>0</color> and <color=green>100</color>.\n\n" +
								 "<b>Hero Defense Rating:</b> Represents how much damage our hero will deflect, when attacked. (Represented as a float value between <color=red>0</color> and <color=green>100</color>.)\n\n" +
								 "<b>Hero Encumbrance:</b> Represents how encumbered our hero is, represented as a float value between <color=green>0</color> and <color=red>100</color>.  The higher the encumbrance, the slower our hero moves -- so clean out all of that useless inventory junk!\n\n" +
								 "Unfortunately, this hero script won't actually get you ready to start adventuring.  Instead it serves as an example of how to add uNote notes inside of your custom scripts.";

		public const string BottomNoteTitle = "This is a note title that's a bit on the long side.  Why, you ask?  To showcase that note titles can wrap, if needed.";
		public const string BottomNote = "Multiple notes can be included in a single script.  The notes are positioned relative to where they are included in the script (i.e. if a note is at the bottom of your declared public variables, the note will display last).\n\n" + 
									"This is great for drawing attention to important settings, or simply to put reminders at the bottom of scripts rather than (or in addition tto!) placing them at the top.";
	}
}

///One last important thing to remember is that the [Note] attribute can take multiple parameters if you want
///to set up notes via code.  Curious as to what options you have?  Look no further:
///
///[Note(bool compact)] will let you specify whether the note starts in compact mode or not.  This defaults to false.
///[Note(string defaultTitle, string defaultText)] will let you specify a default title and text for your note.
///[Note(string defaultTitle, string defaultText, bool compact)] will let you combine all three previous options.