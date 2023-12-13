using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    [SerializeField]
    TextAsset inkStoryJSON;

    [SerializeField]
    Story currentStory;

    [SerializeField]
    GameObject StoryPanel;

    [SerializeField]
    TextMeshProUGUI storyText;

    [SerializeField]
    GameObject[] choices;

    [SerializeField]
    GameObject maggie;

    [SerializeField]
    GameObject introManager;

    [SerializeField]
    Sprite maggieUI;

    [SerializeField]
    Sprite spiritUI;

    MaggieScript maggieScript;

    TextMeshProUGUI[] choicesText;

    public bool inkIsPlaying;

    public bool isReady = true;

    private const string MAGGIE = "maggie";
    private const string SPIRIT = "spirit";

    void Start()
    {
        maggieScript = maggie.GetComponent<MaggieScript>();

        StoryPanel.SetActive(false);
        

        choicesText = new TextMeshProUGUI[choices.Length];

        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        StartStory(inkStoryJSON);
    }

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

    public void ContinueClick()
    {
        ContinueStory();
        Debug.Log("click");
    }

    public void StartStory(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        inkIsPlaying = true;
        StoryPanel.SetActive(true);
        isReady = false;

        /*currentStory.BindExternalFunction("hiUnity", (string hiMessage) =>
        {
            Debug.Log(hiMessage);
        });*/

        ContinueStory();
    }

    void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            storyText.text = currentStory.Continue();
            DisplayChoices();
            HandleTags(currentStory.currentTags);
        }
        else
        {
            //ExitStory();
        }
    }

    void ExitStory()
    {
        currentStory.ResetState();
        //currentStory.UnbindExternalFunction("hiUnity");

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

        //EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case MAGGIE:
                    if (tagValue == "closedSmile")
                    {
                        maggieScript.ClosedSmile();
                    } else if (tagValue == "openSmile")
                    {
                        maggieScript.OpenSmile();
                    } else if (tagValue == "sad")
                    {
                        maggieScript.Sad();
                    } else if (tagValue == "idle")
                    {
                        maggieScript.Idle();
                    } else if (tagValue == "spirit")
                    {
                        introManager.GetComponent<introScript>().SpiritAppear();
                    } else if (tagValue == "spiritTalking")
                    {
                        StoryPanel.GetComponent<Image>().sprite = spiritUI;
                    } else if (tagValue == "maggieTalking")
                    {
                        StoryPanel.GetComponent<Image>().sprite = maggieUI;
                    }
                    break;

               

                default:
                    Debug.LogWarning("an unknown key was found: " + tagKey);
                    break;
            }
        }
    }
}
