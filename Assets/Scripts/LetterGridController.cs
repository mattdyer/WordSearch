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

		DisplayWords();

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

		var letterWidth = Convert.ToInt32(Screen.width * 0.04);
		var letterHeight = Convert.ToInt32(Screen.width * 0.04);

		int letterGridWidth = Convert.ToInt32(Screen.width * 0.80);

		gridWidth = letterGridWidth/letterWidth;
		gridHeight = Screen.height/letterHeight;

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
				letterText.text = ((char) randomLetter).ToString().ToUpper();

				GridLetters[j].Add(letter);

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

		int xMin = direction.x == -1 ? word.Length - 1 : 0;// 1 or word.Length
		int xMax = direction.x == 1 ? gridWidth - word.Length : gridWidth - 1;//gridWidth or GridWidth - word.Length

		int yMin = direction.y == -1 ? word.Length - 1 : 0;//1 or word.Length
		int yMax = direction.y == 1 ? gridHeight - word.Length + 1 : gridHeight - 1;//gridHeight or GridHeight - word.Length


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

				gridLetter.displayLetter.GetComponent<Text>().text = word[li].ToString().ToUpper();
				gridLetter.isRandomLetter = false;
				if(li == 0 || li == word.Length - 1){
					gridLetter.isEndLetter = true;
				}

				gridWord.addLetter(gridLetter);
				gridLetter.addWord(gridWord);

				wordPlaceX += (int) direction.x;
				wordPlaceY += (int) direction.y;
			}

			GridWords.Add(gridWord);
		}

		return wordPlaced;
	}

	private void DisplayWords(){
		
		GameObject displayWord;

		int wordHeight = Convert.ToInt32(Screen.width * 0.025);

		for(int i = 0;i < GridWords.Count;i++){
			displayWord = Instantiate(GameObject.FindWithTag("WordBox"));

			displayWord.tag = "Untagged";
				
			displayWord.transform.position = new Vector3(Screen.width - (float)(Screen.width * 0.18),displayWord.transform.position.y - (wordHeight * (i + 2)),displayWord.transform.position.z);
			displayWord.transform.SetParent(transform,false);

			displayWord.GetComponent<Text>().text = GridWords[i].word.ToUpper();

			GridWords[i].setDisplayWord(displayWord);
		}
	}



}
