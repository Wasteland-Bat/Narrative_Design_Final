using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;

public class InkManager : MonoBehaviour
{
    [SerializeField]
    Story currentStory;

    [SerializeField]
    GameObject StoryPanel;

    [SerializeField]
    TextMeshProUGUI storyText;

    [SerializeField]
    GameObject[] choices;

    TextMeshProUGUI[] choicesText;

    public bool inkIsPlaying;

    public bool isReady = true;
    // Start is called before the first frame update
    void Start()
    {
        StoryPanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];

        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inkIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ContinueStory();
        }
    }

    public void StartStory(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        inkIsPlaying = true;
        StoryPanel.SetActive(true);
        isReady = false;

        ContinueStory();
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            storyText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            ExitStory();
        }
    }

    void ExitStory()
    {
        currentStory.ResetState();
        inkIsPlaying = false;
        StoryPanel.SetActive(false);

        StartCoroutine(MakeReady());
    }

    IEnumerator MakeReady()
    {
        yield return new WaitForSeconds(0.2f);
        isReady = true;
    }

    void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("more choices than UI can support");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
