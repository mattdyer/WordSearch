using UnityEngine;

public class GridWord {

	public string word;
	public int startX;
	public int startY;
	public Vector2 direction;

	public GridWord(string wordtext,int x,int y,Vector2 dir){
		word = wordtext;
		startX = x;
		startY = y;
		direction = dir;
	}

}