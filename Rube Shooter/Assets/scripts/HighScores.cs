using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];
    public OperatingSystemFamily currentOS;
    public string directoryDivider;

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    public TextMeshProUGUI scoresTextElement;
    public GameObject scoresParentContainer;
    private string[] scoresArray = new string[10];

    void Start()
    {
        currentOS = SystemInfo.operatingSystemFamily;
        Debug.Log("current OS: " + currentOS);

        switch (currentOS) {
            case OperatingSystemFamily.Windows:
                directoryDivider = "\\";
                break;
            default:
                directoryDivider = "/";
                break;
        }

        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);

        LoadScoresFromFile();
    }

    void Update()
    {
    }

    public void LoadScoresFromFile()
    {
        bool fileExists = File.Exists(currentDirectory + "/" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            Debug.Log("trying to access file at: " + currentDirectory + "/" + scoreFileName);
            return;
        }

        

        scores = new int[scores.Length];

        StreamReader fileReader = new StreamReader(currentDirectory + "/" + scoreFileName);
        int scoreCount = 0;

        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            string fileLine = fileReader.ReadLine();

            int ReadScore = -1;
            bool didParse = int.TryParse(fileLine, out ReadScore);
            if (didParse)
            {
                scores[scoreCount] = ReadScore;
            }
            else
            {
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);
                scores[scoreCount] = 0;
            }
            scoreCount++;
        }

        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "/" + scoreFileName);

        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        fileWriter.Close();

        Debug.Log("High scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }
        if (desiredIndex < 0)
        {
            Debug.Log("score of " + newScore + " not high enough for high scores list.", this);
            return;
        }
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        Debug.Log("score of " + newScore + " entered into high scores at position " + desiredIndex, this);
    }

    public void ToggleHighScores()
    {
        Debug.Log("Toggling HighScores");
        if (!scoresParentContainer.activeSelf)
        {
            for (int i = 0; i < scores.Length - 1; i++) {
                scoresArray[i] = Convert.ToString(scores[i]);
            }

            scoresTextElement.SetText(String.Join("\n", scoresArray));
        }

        scoresParentContainer.SetActive(!scoresParentContainer.activeSelf);
    }

    public void RestartGame()
    {
        
    }
}
