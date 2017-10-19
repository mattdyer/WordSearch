using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class LetterGridController : MonoBehaviour {

	private int gridWidth;
	private int gridHeight;

	private int wordsInGrid = 20;

	private int defaultWordsInGrid = 20;

	private List<List<GameObject>> GridLetters = new List<List<GameObject>>	();

	private List<GridWord> GridWords = new List<GridWord>();

	private List<Vector2> Directions = new List<Vector2>();

	private int indexTestCount = 0;

	// Use this for initialization
	void Start () {

		for(var i = -1;i <= 1;i++){
			for(var j = -1;j <= 1;j++){
				if(i != 0 || j != 0){
					Directions.Add(new Vector2(i,j));
				}
			}
		}
		

		FillInLetters();
		AddWords();

		//wordsInGrid = PlayerPrefs.GetInt("WordsInGrid",defaultWordsInGrid);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void FillInLetters(){
		GameObject letter;
		Text letterText;
		GridLetter gridLetter;

		Debug.Log(Screen.width);

		var letterWidth = 30;
		var letterHeight = 30;

		gridWidth = Screen.width/letterWidth;
		gridHeight = Screen.height/letterHeight;

		//GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");
		//GameObject letterGrid = GameObject.FindWithTag("LetterGrid");

		//Debug.Log(mainCanvas.transform.position.x);

		//letterGrid.transform.position = new Vector3(0,0,letterGrid.transform.position.z);

		//letterGrid.width = 0;

		//letter = GameObject.FindWithTag("LetterBox");

		//letter.transform.position = new Vector3((Screen.width / -2),Screen.height / 2,letter.transform.position.z);

		for(var j = 0;j < gridHeight;j++){
			GridLetters.Add(new List<GameObject>());
			for(var i = 0;i < gridWidth;i++){
				letter = Instantiate(GameObject.FindWithTag("LetterBox"));
				letter.tag = "Untagged";
				
				letter.transform.position = new Vector3(letter.transform.position.x + letterWidth * (i + 1),letter.transform.position.y - letterHeight * (j + 2) + 20,letter.transform.position.z);
				letter.transform.SetParent(transform,false);
				
				letterText = letter.GetComponent<Text>();

				gridLetter = letter.GetComponent<GridLetter>();

				var randomLetter = Math.Floor(UnityEngine.Random.Range(1f,26.99f)) + 64;

				gridLetter.letter = ((char) randomLetter);
				gridLetter.x = i;
				gridLetter.y = j;
				gridLetter.isRandomLetter = true;
				gridLetter.displayLetter = letter;

				//letterText.text = ((char) randomLetter).ToString() + ' ' + i.ToString() + ' ' + j.ToString();
				letterText.text = ((char) randomLetter).ToString();

				GridLetters[j].Add(letter);

				//GridLetters[j].Add(new GridLetter((char) randomLetter,i,j,letter));

				//Debug.Log(randomLetter);
			}
		}

		Destroy(GameObject.FindWithTag("LetterBox"));

	}


	private void AddWords(){
		int MaxWordLength = Math.Max(gridHeight,gridWidth);
		int randomWordIndex = 0;
		string randomWord;

		char[] delimiterChars = {','};

		TextAsset wordText = Resources.Load("words") as TextAsset;

		string[] Words = wordText.text.Split(delimiterChars);
		Debug.Log(Words.Length);

		

		int wordsPlaced = 0;

		int wordsTried = 0;
		int maxWordsToTry = 20;

		Debug.Log(wordsInGrid);

		while(wordsPlaced < wordsInGrid && wordsTried < maxWordsToTry){
			randomWordIndex = (int) Math.Floor(UnityEngine.Random.Range(0f,Words.Length - 1));

			randomWord = Words[randomWordIndex];

			//randomWord = "testing";

			//Debug.Log(randomWord.Length);

			if(randomWord.Length <= MaxWordLength){
				if(tryToAddWordToGrid(randomWord)){
					wordsPlaced++;
				}
			}
			

			wordsTried++;
		}

		Debug.Log(wordsPlaced);

	}


	private bool tryToAddWordToGrid(string word){

		int randomDirectionIndex = (int) Math.Floor(UnityEngine.Random.Range(0f,Directions.Count - 1));

		//randomDirectionIndex = indexTestCount;

		//indexTestCount++;

		Vector2 direction = Directions[randomDirectionIndex];

		//Debug.Log(word.Length);
		//Debug.Log(direction);
		Debug.Log(word);

		int xMin = direction.x == -1 ? word.Length - 1 : 0;// 1 or word.Length
		int xMax = direction.x == 1 ? gridWidth - word.Length : gridWidth - 1;//gridWidth or GridWidth - word.Length

		int yMin = direction.y == -1 ? word.Length - 1 : 0;//1 or word.Length
		int yMax = direction.y == 1 ? gridHeight - word.Length + 1 : gridHeight - 1;//gridHeight or GridHeight - word.Length

		//Debug.Log(xMin.ToString() + ' ' + xMax.ToString() + ' ' + yMin.ToString() + ' ' + yMax.ToString());
		//Debug.Log(xMax);
		//Debug.Log(yMin);
		//Debug.Log(yMax);

		GameObject checkLetter;

		bool wordPlaced = false;
		int gridX = 0;
		int gridY = 0;
		int foundX = 0;
		int foundY = 0;

		for(int x = xMin;x <= xMax;x++){
			for(int y = yMin;y <= yMax;y++){
				if(!wordPlaced){
					gridX = x;
					gridY = y;
					
					wordPlaced = true;

					for(int li = 0;li < word.Length;li++){
						if(gridY < GridLetters.Count && gridX < GridLetters[gridY].Count){
							checkLetter = GridLetters[gridY][gridX];
							GridLetter gridLetter = checkLetter.GetComponent<GridLetter>();
							//Debug.Log(gridLetter.isRandomLetter);
							if(gridLetter.isRandomLetter || gridLetter.letter == word[li]){
								//wordPlaced = true;
							}else{
								wordPlaced = false;
							}

							gridX += (int) direction.x;
							gridY += (int) direction.y;
						}else{
							wordPlaced = false;
						}
					}

					if(wordPlaced){
						foundX = x;
						foundY = y;
					}

					//Debug.Log(GridLetters[y][x].letter);
				}
			}
		}
		
		int wordPlaceX = 0;
		int wordPlaceY = 0;

		if(wordPlaced){
			
			wordPlaceX = foundX;
			wordPlaceY = foundY;

			GridWord gridWord = new GridWord(word,foundX,foundY,direction);

			for(int li = 0;li < word.Length;li++){
				GridLetter gridLetter = GridLetters[wordPlaceY][wordPlaceX].GetComponent<GridLetter>();

				gridLetter.letter = word[li];

				gridLetter.displayLetter.GetComponent<Text>().text = word[li].ToString();
				gridLetter.isRandomLetter = false;
				if(li == 0 || li == word.Length - 1){
					gridLetter.isEndLetter = true;
				}

				gridLetter.setWord(gridWord);

				gridWord.addLetter(gridLetter);

				wordPlaceX += (int) direction.x;
				wordPlaceY += (int) direction.y;
			}

			GridWords.Add(gridWord);
		}

		return wordPlaced;
	}





}
