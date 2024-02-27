using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DialogueWindow : MonoBehaviour
{
    //testing
    [SerializeField] private DialoguePathLine testPath;
    [SerializeField] private DialogueChoices testChoices;
    [SerializeField] private PlayerUIHandler playerUIHandler;
    [SerializeField] private DialoguePathLine currentPath;
    [SerializeField] private DialogueChoices currentChoices;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI moniker;
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button option1;
    [SerializeField] private Button option2;
    [SerializeField] private Button option3;
    [SerializeField] private Button option4;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;
    [SerializeField] private TextMeshProUGUI option4Text;
    [SerializeField] private bool ChoicesDialogue = false;


    private Animator animator;
    private string dialogueOpenParameter = "DialogueOpen";

    private int currentPathDialogueIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Open(testPath);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Open(testChoices); 
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (GameManager.Instance.CurrentState.Equals(Game_State.DialoguePlaying))
            {


                currentPathDialogueIndex++;
                if (currentPathDialogueIndex < currentPath.Dialogues.Count)
                {
                    DisplayDialogue(currentPath.Dialogues[currentPathDialogueIndex]);
                }
                else
                {
                    if (ChoicesDialogue)
                    {
                        TransistionToDialogueChoices(currentChoices);
                    }
                    else if (!ChoicesDialogue)
                    {
                        Close();
                    }
                }
            }
            else if (GameManager.Instance.CurrentState.Equals(Game_State.DialogueChoices))
            {
                Close();
            }
        }
    }

    public void Open(DialoguePathLine pathToPlay)
    {
        playerUIHandler.ToggleUI(false);        
        GameManager.Instance.CurrentState = Game_State.DialoguePlaying;
        ActivateForDialoguePath();
        currentPath = pathToPlay;
        currentPathDialogueIndex = 0;
        DisplayDialogue(currentPath.Dialogues[currentPathDialogueIndex]);

        animator.SetBool(dialogueOpenParameter, true);
    }
    public void Open(DialogueChoices choices)
    {
        playerUIHandler.ToggleUI(false);
        GameManager.Instance.CurrentState = Game_State.DialogueChoices;
        ActivateForDialogueChoices();
        currentChoices = choices;
        DisplayChoices(currentChoices);

        animator.SetBool(dialogueOpenParameter, true);
    }

    public void TransistionToDialoguePlay(DialoguePathLine pathToPlay)
    {
        GameManager.Instance.CurrentState = Game_State.DialoguePlaying;
        ChoicesDialogue = true;
        DeactivateForDialogueChoices();
        ActivateForDialoguePath();
        currentPath = pathToPlay;
        currentPathDialogueIndex = 0;
        DisplayDialogue(currentPath.Dialogues[currentPathDialogueIndex]);
    }
    public void TransistionToDialogueChoices(DialogueChoices choices)
    {
        GameManager.Instance.CurrentState = Game_State.DialogueChoices;
        ChoicesDialogue = false;
        DeactivateForDialoguePath();
        ActivateForDialogueChoices();
        currentChoices = choices;
        DisplayChoices(currentChoices);
    }

    private void DisplayDialogue(Dialogue dialogue)
    {
        portrait.sprite = dialogue.Sprite;
        moniker.text = dialogue.Name;
        message.text = dialogue.Message;
    }
    private void DisplayChoices(DialogueChoices choices)
    {
        option1Text.text = choices.Options[0].ChoiceText;
        option2Text.text = choices.Options[1].ChoiceText;
        option3Text.text = choices.Options[2].ChoiceText;
        option4Text.text = choices.Options[3].ChoiceText;
        option1.onClick.AddListener(delegate { TransistionToDialoguePlay(choices.Options[0].ChoicePath); });
        option2.onClick.AddListener(delegate { TransistionToDialoguePlay(choices.Options[1].ChoicePath); });
        option3.onClick.AddListener(delegate { TransistionToDialoguePlay(choices.Options[2].ChoicePath); });
        option4.onClick.AddListener(delegate { TransistionToDialoguePlay(choices.Options[3].ChoicePath); });
    }

    private void Close()
    {
        
        DeactivateForDialoguePath();
        DeactivateForDialogueChoices();

        GameManager.Instance.CurrentState = Game_State.Menu;
        playerUIHandler.ToggleUI(true);

        animator.SetBool(dialogueOpenParameter, false);
    }
    private void ActivateForDialoguePath()
    {
        portrait.gameObject.SetActive(true);
        moniker.gameObject.SetActive(true);
        message.gameObject.SetActive(true);
    }
    private void DeactivateForDialoguePath()
    {
        portrait.gameObject.SetActive(false);
        moniker.gameObject.SetActive(false);
        message.gameObject.SetActive(false);
    }
    private void ActivateForDialogueChoices()
    {
        option1.gameObject.SetActive(true);
        option2.gameObject.SetActive(true);
        option3.gameObject.SetActive(true);
        option4.gameObject.SetActive(true);
    }
    private void DeactivateForDialogueChoices()
    {
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        option3.gameObject.SetActive(false);
        option4.gameObject.SetActive(false);
    }
}