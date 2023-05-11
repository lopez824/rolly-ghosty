using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputRecorder
{
    public static List<RecordedInput> RecordedInputList;
    public static List<RecordedInput> PlayerInputList;

    public static void Init()
    {
        RecordedInputList = new List<RecordedInput>();
        PlayerInputList = new List<RecordedInput>();
    }

    public static void SavePlayerInput()
    {
        RecordedInputList.Clear();
        foreach(RecordedInput input in PlayerInputList)
            RecordedInputList.Add(input);

        Debug.Log("PlayerInputCount: " + PlayerInputList.Count);
        Debug.Log("RecordedInputCount: " + RecordedInputList.Count);
        PlayerInputList.Clear();
    }

    public static void AddPlayerInput(string inputName, Vector2 inputVector)
    {
        RecordedInput newInput = new RecordedInput();

        newInput.actionName = inputName;
        newInput.inputVector = inputVector;
        newInput.timePerformed = Time.time;
        if (PlayerInputList.Count > 0)
            newInput.timeSinceLastAction = Time.time - PlayerInputList[PlayerInputList.Count - 1].timePerformed;
        else
            newInput.timeSinceLastAction = 0;

        PlayerInputList.Add(newInput);
    }

    public static List<RecordedInput> GetSavedList()
    {
        return RecordedInputList;
    }
    
    public static RecordedInput GetLastInput()
    {
        return PlayerInputList[PlayerInputList.Count - 1];
    }
}
