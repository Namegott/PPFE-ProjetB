using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] HighScore[] HighScores = new HighScore[5];
    string BaseFilePath;

    private void Start()
    {
        BaseFilePath = Application.dataPath;
        foreach (HighScore highScore in HighScores)
        {
            highScore.GameVersion = Application.version;
        }

        LoadSave(1);
        LoadSave(2);
        LoadSave(3);
        LoadSave(4);
        LoadSave(5);
    }

    public void WriteSave(int mapNumber)
    {
        //Debug.Log("e");
        string filepath = BaseFilePath + "/Save/HighScoreMap" + mapNumber + ".json";
        //Debug.Log(filepath);

        string saveData = JsonUtility.ToJson(HighScores[mapNumber - 1]);
        Debug.Log("save" + saveData);

        System.IO.File.WriteAllText(filepath, saveData);
    }
    public void LoadSave(int mapNumber)
    {
        string filepath = BaseFilePath + "/Save/HighScoreMap" + mapNumber + ".json";
        //Debug.Log(filepath);
        string loadData = System.IO.File.ReadAllText(filepath);
        Debug.Log("load" + loadData);
        
        HighScore checkVersion = JsonUtility.FromJson<HighScore>(loadData);
        if (checkVersion.GameVersion == Application.version)
        {
            HighScores[mapNumber - 1] = JsonUtility.FromJson<HighScore>(loadData);
        }
    }

    public void SetHighScore(int mapNumber,int score, string pseudo)
    {
        //Debug.Log("c");
        mapNumber--;
        if (score >= HighScores[mapNumber].Score[0])
        {
            Debug.Log("1");
            //reorder older score and pseudo
            HighScores[mapNumber].Score[4] = HighScores[mapNumber].Score[3];
            HighScores[mapNumber].Pseudo[4] = HighScores[mapNumber].Pseudo[3];
            HighScores[mapNumber].Score[3] = HighScores[mapNumber].Score[2];
            HighScores[mapNumber].Pseudo[3] = HighScores[mapNumber].Pseudo[2];
            HighScores[mapNumber].Score[2] = HighScores[mapNumber].Score[1];
            HighScores[mapNumber].Pseudo[2] = HighScores[mapNumber].Pseudo[1];
            HighScores[mapNumber].Score[1] = HighScores[mapNumber].Score[0];
            HighScores[mapNumber].Pseudo[1] = HighScores[mapNumber].Pseudo[0];

            //set new score and pseudo
            HighScores[mapNumber].Score[0] = score;
            HighScores[mapNumber].Pseudo[0] = pseudo;
        }
        else if (score >= HighScores[mapNumber].Score[1])
        {
            Debug.Log("2");
            //reorder older score and pseudo
            HighScores[mapNumber].Score[4] = HighScores[mapNumber].Score[3];
            HighScores[mapNumber].Pseudo[4] = HighScores[mapNumber].Pseudo[3];
            HighScores[mapNumber].Score[3] = HighScores[mapNumber].Score[2];
            HighScores[mapNumber].Pseudo[3] = HighScores[mapNumber].Pseudo[2];
            HighScores[mapNumber].Score[2] = HighScores[mapNumber].Score[1];
            HighScores[mapNumber].Pseudo[2] = HighScores[mapNumber].Pseudo[1];

            //set new score and pseudo
            HighScores[mapNumber].Score[1] = score;
            HighScores[mapNumber].Pseudo[1] = pseudo;
        }
        else if (score >= HighScores[mapNumber].Score[2])
        {
            Debug.Log("3");
            //reorder older score and pseudo
            HighScores[mapNumber].Score[4] = HighScores[mapNumber].Score[3];
            HighScores[mapNumber].Pseudo[4] = HighScores[mapNumber].Pseudo[3];
            HighScores[mapNumber].Score[3] = HighScores[mapNumber].Score[2];
            HighScores[mapNumber].Pseudo[3] = HighScores[mapNumber].Pseudo[2];

            //set new score and pseudo
            HighScores[mapNumber].Score[2] = score;
            HighScores[mapNumber].Pseudo[2] = pseudo;
        }
        else if (score >= HighScores[mapNumber].Score[3])
        {

            Debug.Log("4");
            //reorder older score and pseudo
            HighScores[mapNumber].Score[4] = HighScores[mapNumber].Score[3];
            HighScores[mapNumber].Pseudo[4] = HighScores[mapNumber].Pseudo[3];

            //set new score and pseudo
            HighScores[mapNumber].Score[3] = score;
            HighScores[mapNumber].Pseudo[3] = pseudo;
        }
        else if (score >= HighScores[mapNumber].Score[4])
        {
            Debug.Log("5");
            //set new score and pseudo
            HighScores[mapNumber].Score[4] = score;
            HighScores[mapNumber].Pseudo[4] = pseudo;
        }
        else
        {
            Debug.Log("your bad");
        }

        //Debug.Log("d");
        //Debug.Log(HighScores[mapNumber]);
        WriteSave(mapNumber + 1);
    }

    public int[] GetScore(int mapNumber)
    {
        mapNumber--;
        return HighScores[mapNumber].Score;
    }
    public string[] GetPseudo(int mapNumber)
    {
        mapNumber--;
        return HighScores[mapNumber].Pseudo;
    }
}

[System.Serializable]
public class HighScore
{
    public string GameVersion;
    public string[] Pseudo = new string[5];
    public int[] Score = new int[5];
}