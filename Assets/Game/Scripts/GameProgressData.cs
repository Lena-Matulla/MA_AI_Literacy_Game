using System;
using System.Collections.Generic;

[System.Serializable]
public class GameProgressData
{
    public int currentPoints;
    public int currentLevel;

    public List<string> studyOrderImageIds = new List<string>();
    public int studyIndex = 0;
    public bool studyFinished = false;
}