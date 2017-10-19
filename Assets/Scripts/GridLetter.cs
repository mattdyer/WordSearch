using UnityEngine;
using System.Collections.Generic;

public class GridLetter : MonoBehaviour {

	public char letter;
	public int x;
	public int y;
	
	public GameObject displayLetter;
	private List<GridWord> words = new List<GridWord>();

	public bool isRandomLetter = true;
	public bool isEndLetter = false;
	public bool highlighting = false;

	public GridLetter(char letter,int x,int y,GameObject displayLetter){
		this.letter = letter;
		this.x = x;
		this.y = y;
		this.displayLetter = displayLetter;
	}

	public void addWord(GridWord word){
		if(words == null){
			words = new List<GridWord>();
		}
		words.Add(word);
	}

	public void startWordDrag(){
		if(!isRandomLetter && !highlighting && isEndLetter){
			Debug.Log(letter);
			highlighting = true;
			for(int i = 0;i < words.Count;i++){
				words[i].startDrag();
			}
			
		}
	}

	public void endWordDrag(){
		if(!isRandomLetter){
			Debug.Log(letter);
			highlighting = false;
			for(int i = 0;i < words.Count;i++){
				words[i].endDrag();
			}
		}
	}

	public void enterLetter(){
		if(isEndLetter){
			Debug.Log(letter);
			for(int i = 0;i < words.Count;i++){
				if(words[i].highlighting && !highlighting){
					Debug.Log("word found");
					words[i].highlightWord();
				}
			}
		}
	}

}