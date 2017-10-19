using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridWord {

	public string word;
	public int startX;
	public int startY;
	public Vector2 direction;
	public bool highlighting = false;
	public bool found = false;
	private List<GridLetter> Letters = new List<GridLetter>();

	public GridWord(string wordtext,int x,int y,Vector2 dir){
		word = wordtext;
		startX = x;
		startY = y;
		direction = dir;
	}

	public void startDrag(){
		Debug.Log(word);
		highlighting = true;
	}

	public void endDrag(){
		highlighting = false;
	}


	public void highlightWord(){

		for(int i = 0;i < Letters.Count;i++){
			Letters[i].displayLetter.GetComponent<Text>().color = new Color(1,1,1,1);
			Debug.Log(Letters[i].letter);
		}

		found = true;
		highlighting = false;

	}

	public void addLetter(GridLetter letter){
		Letters.Add(letter);
	}

}