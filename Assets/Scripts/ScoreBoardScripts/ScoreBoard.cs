using System.IO;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 5;
    [SerializeField] private Transform higscoresHolderTransform = null;

    [SerializeField] private GameObject scoreboardEntryObject = null;

    [Header("Test")]
    [SerializeField] ScoreBoardEntryData testEntrydata = new ScoreBoardEntryData();

    private string SavePath => $"{Application.persistentDataPath}/highscores.json";

    private void Start(){

        ScoreBoardSaveData savedScores = GetSavedScores();

        SaveScores(savedScores);

        UpdateUI(savedScores); 
    }

    [ContextMenu("Add Test Entry")]
    public void addTestEntry(){
        AddEntry(testEntrydata);
    }

    private void UpdateUI(ScoreBoardSaveData savedScores){
        foreach(Transform child in higscoresHolderTransform){
            Destroy(child.gameObject);
        }
        int i = 0;
        foreach( ScoreBoardEntryData highscore in savedScores.highscores){
            i++;
            /*GameObject res = Instantiate(scoreboardEntryObject, higscoresHolderTransform);
            res.GetComponent<ScoreBoardEntryUI>().Initialize(highscore);
            */
            Instantiate(scoreboardEntryObject,higscoresHolderTransform).GetComponent<ScoreBoardEntryUI>().Initialize(highscore);
        }
    }

    public void AddEntry(ScoreBoardEntryData scoreBoardEntryData){
        ScoreBoardSaveData savedScores = GetSavedScores();

        bool scoreAdded = false;
        for(int i = 0; i < savedScores.highscores.Count; i++){

            if(scoreBoardEntryData.entryScore > savedScores.highscores[i].entryScore){
                savedScores.highscores.Insert(i,scoreBoardEntryData);
                scoreAdded = true;
                break;
            }

        }

        if(!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries){
            savedScores.highscores.Add(scoreBoardEntryData);
        }

        if(savedScores.highscores.Count > maxScoreboardEntries){
            savedScores.highscores.RemoveRange(maxScoreboardEntries,savedScores.highscores.Count - maxScoreboardEntries);
        }

        SaveScores(savedScores);

        UpdateUI(savedScores);
        

    }

    private ScoreBoardSaveData GetSavedScores(){

       
        if(!File.Exists(SavePath)){
             File.Create(SavePath).Dispose();
             
            return new ScoreBoardSaveData();
        }

        using(StreamReader stream = new StreamReader(SavePath)){

            string json = stream.ReadToEnd();

            

           

            ScoreBoardSaveData x = JsonUtility.FromJson<ScoreBoardSaveData>(json);

            if(x == null){
               
                return new ScoreBoardSaveData();
            }
            
            return x; 

        }

    }

    private void SaveScores(ScoreBoardSaveData scoreBoardSaveData){
        using(StreamWriter stream = new StreamWriter(SavePath)){

            string json = JsonUtility.ToJson(scoreBoardSaveData, true);
            stream.Write(json);
        
        }
    }

}
