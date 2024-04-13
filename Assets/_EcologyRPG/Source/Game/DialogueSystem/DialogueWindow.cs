using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EcologyRPG.GameSystems.UI;

namespace EcologyRPG.GameSystems.Dialogue
{
    public class DialogueWindow : MonoBehaviour
    {
        public static DialogueWindow current;

        [SerializeField] private PlayerUIHandler playerUIHandler;
        [SerializeField] private DialoguePathLine currentPath;
        [SerializeField] private DialogueChoices currentChoices;
        [SerializeField] private DialogueConnector currentConnector;
        [SerializeField] private DialogueQuest currentQuests;

        [SerializeField] private GameObject questInformationTab;
        [SerializeField] private GameObject choicesTab;
        [SerializeField] private GameObject dialogueTab;
        [SerializeField] private GameObject connectionTab;

        [SerializeField] private Image portrait;
        [SerializeField] private TextMeshProUGUI moniker;
        [SerializeField] private TextMeshProUGUI message;

        [SerializeField] private Button quest1;
        [SerializeField] private Button quest2;

        [SerializeField] private TextMeshProUGUI quest1Text;
        [SerializeField] private TextMeshProUGUI quest2Text;

        [SerializeField] private Button connectionQuests;
        [SerializeField] private Button connectionChoices;

        [SerializeField] private TextMeshProUGUI connectionQuestsText;
        [SerializeField] private TextMeshProUGUI connectionChoicesText;

        [SerializeField] private Button option1;
        [SerializeField] private Button option2;
        [SerializeField] private Button option3;
        [SerializeField] private Button option4;

        [SerializeField] private Button exitDialogue;
        [SerializeField] private Button nextDialogue;

        [SerializeField] private Button BackButton;

        [SerializeField] private TextMeshProUGUI option1Text;
        [SerializeField] private TextMeshProUGUI option2Text;
        [SerializeField] private TextMeshProUGUI option3Text;
        [SerializeField] private TextMeshProUGUI option4Text;

        [SerializeField] private bool ChoicesDialogue = false;
        [SerializeField] private bool connectionStart = false;
        [SerializeField] private bool QuestDialogue = false;



        private Animator animator;
        private string dialogueOpenParameter = "DialogueOpen";

        private int currentPathDialogueIndex = 0;

        private void Awake()
        {
            if(current == null)
            {
                current = this;
            }
            else
            {
                Destroy(gameObject);
            }
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            exitDialogue.onClick.AddListener(delegate { Close(); });
            nextDialogue.onClick.AddListener(delegate { Next(); });
        }

        public void Open(DialoguePathLine pathToPlay)
        {
            playerUIHandler.ToggleUI(false);
            Game.Instance.CurrentState = Game_State.DialoguePlaying;
            ActivateForDialoguePath();
            currentPath = pathToPlay;
            currentPathDialogueIndex = 0;
            DisplayDialogue(currentPath.Dialogues[currentPathDialogueIndex]);

            animator.SetBool(dialogueOpenParameter, true);
        }
        public void Open(DialogueChoices choices)
        {
            currentChoices = choices;
            playerUIHandler.ToggleUI(false);
            Game.Instance.CurrentState = Game_State.DialogueChoices;
            ActivateForDialogueChoices();
            DisplayChoices(currentChoices);

            animator.SetBool(dialogueOpenParameter, true);
        }
        public void Open(DialogueConnector connector)
        {
            currentConnector = connector;
            playerUIHandler.ToggleUI(false);
            Game.Instance.CurrentState = Game_State.DialogueChoices;
            ActivateForDialogueConnector();
            DisplayConnections(currentConnector);
            connectionStart = true;

            animator.SetBool(dialogueOpenParameter, true);
        }

        public void TransistionToDialoguePlay(DialoguePathLine pathToPlay)
        {
            Game.Instance.CurrentState = Game_State.DialoguePlaying;
            ChoicesDialogue = true;
            ActivateForDialoguePath();
            currentPath = pathToPlay;
            currentPathDialogueIndex = 0;
            DisplayDialogue(currentPath.Dialogues[currentPathDialogueIndex]);
        }
        public void TransistionToDialogueChoices(DialogueChoices choices)
        {
            Game.Instance.CurrentState = Game_State.DialogueChoices;
            ChoicesDialogue = false;
            ActivateForDialogueChoices();
            currentChoices = choices;
            DisplayChoices(currentChoices);
        }
        public void TransitionToDialogueQuests(DialogueQuest quests)
        {
            Game.Instance.CurrentState = Game_State.DialogueChoices;
            ChoicesDialogue = false;
            ActivateForDialogueQuest();
            currentQuests = quests;
            DisplayQuests(currentQuests);
            QuestDialogue = true;
        }
        public void TransitionToDialogueConnector(DialogueConnector connector)
        {
            Game.Instance.CurrentState = Game_State.DialogueChoices;
            currentConnector = connector;
            ActivateForDialogueConnector();
            DisplayConnections(currentConnector);
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
        private void DisplayQuests(DialogueQuest quests)
        {
            quest1Text.text = quests.Quests[0].QuestText;
            quest2Text.text = quests.Quests[1].QuestText;

            string quest1Flag = quests.Quests[0].QuestFlag;
            string quest2Flag = quests.Quests[1].QuestFlag;
            int flag1val;
            int flag2val;
            int completionValue1 = quests.Quests[0].CompletionValue;
            int completionValue2 = quests.Quests[1].CompletionValue;
            
            if (Game.Flags.TryGetFlag(quest1Flag, out flag1val))
            {
                if(flag1val == completionValue1) quest1.onClick.AddListener(delegate { TransistionToDialoguePlay(quests.Quests[0].CompletionPath); });
                else quest1.onClick.AddListener(delegate { TransistionToDialoguePlay(quests.Quests[0].InfoPath); });
            }
            

            if (Game.Flags.TryGetFlag(quest2Flag, out flag2val))
            {
                if(flag2val == completionValue2) quest2.onClick.AddListener(delegate { TransistionToDialoguePlay(quests.Quests[1].CompletionPath); });
                else quest2.onClick.AddListener(delegate { TransistionToDialoguePlay(quests.Quests[1].InfoPath); });
            }
        }
        private void DisplayConnections(DialogueConnector connector)
        {
            connectionQuestsText.text = connector.DialogueConnection.QuestTalk;
            connectionChoicesText.text = connector.DialogueConnection.LetsTalk;

            connectionQuests.onClick.AddListener(delegate { TransitionToDialogueQuests(connector.DialogueConnection.DialogueQuest); });
            connectionChoices.onClick.AddListener(delegate { TransistionToDialogueChoices(connector.DialogueConnection.Choices); });
        }

        private void Close()
        {
            DeactivateAll();

            connectionStart = false;
            Game.Instance.CurrentState = Game_State.Playing;
            playerUIHandler.ToggleUI(true);

            animator.SetBool(dialogueOpenParameter, false);
        }
        private void Next()
        {
            if (Game.Instance.CurrentState.Equals(Game_State.DialoguePlaying))
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
                        if (QuestDialogue)
                        {
                            TransitionToDialogueQuests(currentQuests);
                        }
                        else TransistionToDialogueChoices(currentChoices);
                    }
                    else if (!ChoicesDialogue)
                    {
                        Close();
                    }
                }
            }
        }
        private void DeactivateAll()
        {
            DeactivateForDialoguePath();
            DeactivateForDialogueConnector();
            DeactivateForDialogueQuest();
            DeactivateForDialogueChoices();
        }
        private void BackToConnection()
        {
            TransitionToDialogueConnector(currentConnector);
            QuestDialogue = false;
        }
        private void ActivateForDialoguePath()
        {
            dialogueTab.SetActive(true);
            DeactivateForDialogueChoices();
            DeactivateForDialogueQuest();
            DeactivateForDialogueConnector();
        }
        private void DeactivateForDialoguePath()
        {
            dialogueTab.SetActive(false);
        }
        private void ActivateForDialogueChoices()
        {
            choicesTab.SetActive(true);
            DeactivateForDialoguePath();
            DeactivateForDialogueQuest();
            DeactivateForDialogueConnector();
            if (connectionStart)
            {
                BackButton.gameObject.SetActive(true);
                BackButton.onClick.AddListener(delegate { BackToConnection(); });
            }
        }
        private void DeactivateForDialogueChoices()
        {
            choicesTab.SetActive(false);
            if (connectionStart)
            {
                BackButton.gameObject.SetActive(false);
            }
        }

        private void ActivateForDialogueQuest()
        {
            questInformationTab.SetActive(true);
            DeactivateForDialogueChoices();
            DeactivateForDialoguePath();
            DeactivateForDialogueConnector();
            if (connectionStart)
            {
                BackButton.gameObject.SetActive(true);
                BackButton.onClick.AddListener(delegate { BackToConnection(); });
            }
        }

        private void DeactivateForDialogueQuest()
        {
            questInformationTab.SetActive(false);
            if (connectionStart)
            {
                BackButton.gameObject.SetActive(false);
            }
        }

        private void ActivateForDialogueConnector()
        {
            connectionTab.SetActive(true);
            DeactivateForDialogueChoices();
            DeactivateForDialogueQuest();
            DeactivateForDialoguePath();
        }

        private void DeactivateForDialogueConnector()
        {
            connectionTab.SetActive(false);
        }
    }
}