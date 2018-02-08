using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using UnfinityGames.Common.Editor;

namespace UnfinityGames.uNote
{
	//Add a custom inspector that strips out and hides the default "Script" header property.
	[CustomEditor(typeof(Note))]
	public class NoteInspector : Editor
	{
		//Create a private string array that contains our "m_Script" header variable.
		private static readonly string[] excludedProperties = new string[] { "m_Script" };

		//Create an int to store our current note index.
		private static int noteIndex = 0;

		//Handles rendering our Inspector.
		public override void OnInspectorGUI()
		{
			//Draw our GUI, but exclude the script property that's usually in the header.
			DrawPropertiesExcluding(serializedObject, excludedProperties);

			//Apply and modified properties.
			serializedObject.ApplyModifiedProperties();
		}

		//Handles rendering our notes in the Scene View/Scene window.
		void OnSceneGUI()
		{
			//If we don't display notes in the scene, exit out of here early.
			if (!uNoteGlobalData.DisplayNotesInScene)
				return;

			//Set up our title style.
			GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel) { wordWrap = true };

			//Set up our textStyle and width.
			GUIStyle textStyle = new GUIStyle(EditorStyles.label) { wordWrap = true, richText = true };
			var width = Screen.width - 120; //Set our X width to our screen width, - 120 pixels.

			//Begin rendering our GUI.
			Handles.BeginGUI();
			{
				//Set up a note array, and attempt to get all of the notes from the selected object.
				Note[] notes = new Note[0];
				if (Selection.activeGameObject != null)
				{
					//If we're displaying parent notes...
					if (uNoteGlobalData.DisplayParentNotes)
					{
						//If our selected object's parent isn't null...
						if (Selection.activeGameObject.transform.parent != null)
						{
							//Only search the parent object for notes if OnlySearchFirstParent is enabled.
							if(uNoteGlobalData.OnlySearchFirstParent)
							{
								//Create a list that will house our parent and child notes.
								List<Note> CombinedList = new List<Note>();

								//Add our child notes, then our parent notes to our combined list of notes.
								CombinedList.AddRange(Selection.activeGameObject.GetComponents<Note>());
								CombinedList.AddRange(Selection.activeGameObject.transform.parent.GetComponents<Note>());

								//Copy our combined list over to our notes array.
								notes = CombinedList.ToArray();

								//Clear our combined list, as it's no longer needed.
								CombinedList.Clear();
							}
							else
							{
								//Use GetComponentsInParent to get all of our parent notes (Note: this only works in Unity 4.5+).
								notes = Selection.activeGameObject.transform.GetComponentsInParent<Note>(true);
							}								
						}
						else //Otherwise, just search the selected object for notes.
							notes = Selection.activeGameObject.GetComponents<Note>();
					}
					else //Otherwise, just search the selected object for notes.
						notes = Selection.activeGameObject.GetComponents<Note>();
				}

				//Set up a NoteData variable...
				NoteData data;

				//Before we begin, make sure that our noteIndex is currently within bounds.
				noteIndex = Mathf.Clamp(noteIndex, 0, notes.Length - 1);

				//If we have more than one note, set our displayed note to the currently-selected one.
				if (notes.Length > 1)
					data = notes[noteIndex].note;
				else //Otherwise just use our inspector target's note.
					data = (target as Note).note;

				//If our note or note's text is null or empty, exit early as we have nothing to display.
				if (data == null || string.IsNullOrEmpty(data.Text))
				{
					//If we've gone over to some other note that happens to be empty, reset our noteIndex.
					if (noteIndex > 0)
						noteIndex = 0;

					//End our GUI and return.
					Handles.EndGUI();
					return;
				}

				//The height of our title.  We set this to 10 initially on purpose -- the 10 here will be used as
				//padding.  Later we add our title's height to this value, resulting in the height of our title + 10.
				var titleHeight = 10f;

				//Create our height variable.  Our height is the calculated height plus 30 pixels of padding, due to
				//the height of our header/title.
				var height = 0f;

				//Set up a yPosition variable.
				var yPosition = 0f;

				//Create or retrieve our note's title.
				var title = (string.IsNullOrEmpty(data.Title) ? "Note" : data.Title);

				//If we have more than one note, we need to do some title formatting.
				if (notes.Length > 1)
				{
					//If this is a note from a parent prefix the note's title with the name of the gameobject.
					if (notes[noteIndex].gameObject != Selection.activeGameObject)
					{
						title = "(" + notes[noteIndex].gameObject.name + ") " + title;
					}

					//Add a note number prefix to our note, as we have more than one to display.
					//We add one to our noteIndex due to the fact that the index is zero-based, while we would like
					//to display a number starting at 1.
					title = (noteIndex + 1) + ". " + title;
				}

				//Set up our padding for our navigation arrows.  50 pixels if they're visible, 10 pixels if they're not.
				var arrowButtonPadding = (notes.Length > 1) ? 50 : 10;

				//Switch through our NotePositions, and work out our yPosition based off of the selected position.
				switch (uNoteGlobalData.NotePositionInScene)
				{
					case NotePosition.Top:
						titleHeight += titleStyle.CalcHeight(new GUIContent(title), width - arrowButtonPadding);

						//Create our height variable.  Our height is the calculated height plus 30 pixels of padding, due to
						//the height of our header/title.
						height = textStyle.CalcHeight(new GUIContent(data.Text), width - 10) + titleHeight;

						yPosition = 0 + 25;//Our y position is 0 (top of the window) plus 25 pixels of padding.
						break;

					case NotePosition.Bottom:
						width = Screen.width - 285; //If we're on the bottom we need to make the box a bit smaller, as
						//things like camera previews take up more room on the right.

						titleHeight += titleStyle.CalcHeight(new GUIContent(title), width - arrowButtonPadding);

						//Create our height variable.  Our height is the calculated height plus the height of our header/title.
						height = textStyle.CalcHeight(new GUIContent(data.Text), width - 10) + titleHeight;

						yPosition = (Screen.height - height) - 30;//Our y position is the screen's height 
						//(bottom of the window) minus our box height
						//and 30 pixels of padding.
						break;
				}

				//Add some padding to our window, depending on if it's at the bottom or the top.
				if(uNoteGlobalData.NotePositionInScene == NotePosition.Bottom)
				{
					//Note that if the window is at the bottom, we subtract an amount equal to the height
					//padding from it's position to keep it at the same position.
					yPosition -= 5;
					height += 5;
				}
				else if (uNoteGlobalData.NotePositionInScene == NotePosition.Top)
					height += 5;

				//Begin rendering our note's GUI window...
				GUILayout.Window(0, new Rect(10, yPosition, width, height), (id) =>
				{
					using(new UnfinityVertical())
					{
						//Render our title/header, so it looks a bit different from the text below it.
						//We use the HelpBox style to do the majority of this.
						using(new UnfinityArea(new Rect(0, 0, width, titleHeight), GUI.skin.GetStyle("HelpBox")))
						{
							using(new UnfinityHorizontal())
							{
								GUILayout.Label(title, titleStyle, GUILayout.MaxWidth(width - arrowButtonPadding));

								//If we have more than one note, render our previous and next buttons.
								if(notes.Length > 1)
								{
									if(GUILayout.Button(new GUIContent("◄", "Previous Note"), EditorStyles.boldLabel))
									{
										noteIndex--;

										if(noteIndex < 0)
											noteIndex = notes.Length - 1;
									}

									if(GUILayout.Button(new GUIContent("►", "Next Note"), EditorStyles.boldLabel))
									{
										noteIndex++;

										if(noteIndex >= notes.Length)
											noteIndex = 0;
									}
								}
							}
						}

						//Add some space...
						GUILayout.Space(titleHeight);

						//Render our note's text.
						GUILayout.Label(data.Text, textStyle);
					}
				}, string.Empty, EditorStyles.textField);
			}
			Handles.EndGUI(); //End our GUI -- we're done.
		}
	}
}
