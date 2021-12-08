using Godot;
using System;
using System.Collections.Generic; 
using System.Linq;
using ScoreFile = System.IO.File;
//using System.Text.Json; 
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

public class HighScore 
{
	public List<Score> Scores {get;set;}
	public static int CurrentScore {get;set;}
	public static string Name {get;set;} = "Unknown";
	
	public void Sort(){
		Scores = Scores.OrderByDescending(s => s.Value).ToList();
	}
	public void AddScore(string name, int value){
		Scores.Add(new Score(){Name = name, Value = value});
		Sort();
		while(Scores.Count > 10){
			Scores.RemoveAt(10);
		}
	}
	public /*async Task*/ void LoudScores(){
		string data = ScoreFile.ReadAllText("Score/scores.txt");
		string[] datas = data.Split(',');
		Scores = new List<Score>();
		foreach(var dataset in datas){
			string[] parts = dataset.Split(':');
			if(parts.Length == 2)
			{
				Scores.Add(new Score(){Name = parts[0], Value = Int32.Parse(parts[1])});
			}
		}
	}
	
	public /*async Task*/ void SaveScores(){
		string data = ""; 
		foreach(var score in Scores){
			data += score.Name + ":" + score.Value + ",";
		}
		ScoreFile.WriteAllText("Score/scores.txt", data);
		//using (FileStream createStream = File.Create("scores.json")){
		//await JsonSerializer.SerializeAsync(createStream, _data);
		//}
	}
}

public class Score
{
	public string Name {get;set;}
	public int Value {get;set;}
}
