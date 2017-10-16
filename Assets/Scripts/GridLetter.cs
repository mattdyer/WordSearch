using UnityEngine;

public class GridLetter : MonoBehaviour {

	public char letter;
	public int x;
	public int y;
	
	public GameObject displayLetter;
	public GridWord gridWord;

	public bool isRandomLetter = true;

	public bool isEndLetter = false;

	public bool highlighting = false;

	public GridLetter(char letter,int x,int y,GameObject displayLetter){
		this.letter = letter;
		this.x = x;
		this.y = y;
		this.displayLetter = displayLetter;
	}

	public void setWord(GridWord word){
		gridWord = word;
	}

	public void startWordDrag(){
		if(!highlighting && isEndLetter){
			Debug.Log(letter);
			highlighting = true;
		}
	}

	public void endWordDrag(){
		//if(isEndLetter){
			Debug.Log(letter);
			highlighting = false;
		//}
	}

	public void enterLetter(){
		if(isEndLetter){
			Debug.Log(letter);
		}
	}

}