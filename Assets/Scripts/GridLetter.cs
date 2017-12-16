using UnityEngine;
using UnityEngine.EventSystems;
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

	private GameObject highlightIndicator;


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

	public void startWordDrag(BaseEventData data){

		Debug.Log(((PointerEventData) data).position);

		if(!isRandomLetter && !highlighting && isEndLetter){
			highlighting = true;

			highlightIndicator = Instantiate(GameObject.FindWithTag("WordHighlight"));

			//highlightIndicator.transform.localScale = displayLetter.GetComponent<RectTransform>().sizeDelta;

			highlightIndicator.transform.position = new Vector3(0,0,0);

			highlightIndicator.transform.SetParent(displayLetter.transform,false);

			for(int i = 0;i < words.Count;i++){
				words[i].startDrag();
			}
			
		}
	}

	public void endWordDrag(BaseEventData data){
		Debug.Log(((PointerEventData) data).position);
		
		Destroy(highlightIndicator);

		if(!isRandomLetter){
			highlighting = false;
			for(int i = 0;i < words.Count;i++){
				words[i].endDrag();
			}
		}
	}

	public void enterLetter(BaseEventData data){
		Debug.Log(((PointerEventData) data).position);

		if(isEndLetter){
			for(int i = 0;i < words.Count;i++){
				if(words[i].highlighting && !highlighting){
					words[i].highlightWord();
				}
			}
		}
	}


}