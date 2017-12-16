using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridWord : MonoBehaviour {

	public string word;
	public int startX;
	public int startY;
	public Vector2 direction;
	public bool highlighting = false;
	public bool found = false;

	private List<GridLetter> Letters = new List<GridLetter>();
	private GameObject displayWord;


	public GridWord(string wordtext,int x,int y,Vector2 dir){
		this.word = wordtext;
		this.startX = x;
		this.startY = y;
		this.direction = dir;
	}

	public void startDrag(){
		highlighting = true;
	}

	public void endDrag(){
		highlighting = false;
	}


	public void highlightWord(){

		for(int i = 0;i < Letters.Count;i++){
			Letters[i].displayLetter.GetComponent<Text>().color = new Color(1,1,1,1);
		}

		found = true;
		highlighting = false;

		displayWord.GetComponent<Text>().color = new Color(1,1,1,1);

		GameObject highlightIndicator = Instantiate(GameObject.FindWithTag("WordHighlight"));

		highlightIndicator.transform.localScale += new Vector3(0,Letters.Count * Letters[1].letterHeight,0);
		
		highlightIndicator.transform.position = new Vector3(0,-Letters[1].letterHeight * 2,0);

		Quaternion highlightRotation = new Quaternion();

		highlightRotation.SetFromToRotation(new Vector3(0,1,0),new Vector3(direction.x, direction.y, 0));

		highlightIndicator.transform.rotation = highlightIndicator.transform.rotation * highlightRotation;

		highlightIndicator.transform.SetParent(Letters[1].displayLetter.transform,false);

	}

	public void addLetter(GridLetter letter){
		Letters.Add(letter);
	}

	public void setDisplayWord(GameObject displayWord){
		this.displayWord = displayWord;
	}

}