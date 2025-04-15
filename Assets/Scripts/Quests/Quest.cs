using System;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [Header("QuestDatas")]
    public List<QuestScriptableObject> questsPossibles;
    public int numberQuestIndex;

    [Header("QuestDataChosen"), HideInInspector]
    public QuestScriptableObject questData;
    public int rewards;
    public string descriptionQuest;
    public SellableObject sellableObject;
    public Sprite customer;
    public bool questAccepted; 

    [Header("Quest Ui Elements"), HideInInspector]
    public Button button;
    public TMP_Text boxDescription;
    public Image image;

    private void Start()
    {
        button = transform.GetChild(0).GetComponent<Button>();
        boxDescription = transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        image = transform.GetChild(2).GetComponentInChildren<Image>();
        
        Shop.Instance.questsAvailables[numberQuestIndex] = this;

        if (!Save.LoadQuests() || questData == null)
            GetRandomQuest();
        else
            GetQuestInfos();
    }

    public void AcceptQuest()
    {
        questAccepted = true;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => CompletedQuest());
        button.GetComponentInChildren<TMP_Text>().text = "Recuperer la recompense";

        Save.SaveQuests();
        //SceneManager.LoadScene(1); // To replace with the index of the main game
    }

    public void CompletedQuest()
    {
        questAccepted = false;

        PlayerMoney moneyPlayer = PlayerMoney.Instance;
        moneyPlayer.AddMoney(rewards);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => AcceptQuest());
        button.GetComponentInChildren<TMP_Text>().text = "Accepter";

        GetRandomQuest();
        Save.SaveQuests();
    }

    public void GetRandomQuest()
    {
        int randomIndex = UnityEngine.Random.Range(0, questsPossibles.Count);
        questData = questsPossibles[randomIndex];

        randomIndex = UnityEngine.Random.Range(0, questData.customerAvailables.Count);
        customer = questData.customerAvailables[randomIndex];
        GetQuestInfos();

        Shop shop = Shop.Instance;
        Save.SaveQuests();
    }

    public void GetQuestInfos()
    {
        rewards = questData.rewards;
        descriptionQuest = questData.description;
        sellableObject = questData.goalObject;

        boxDescription.text = descriptionQuest;
        image.sprite = customer;

        if (questAccepted)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => AcceptQuest());
            button.GetComponentInChildren<TMP_Text>().text = "Recuperer la recompense";
        }
        else
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => AcceptQuest());
            button.GetComponentInChildren<TMP_Text>().text = "Accepter";
        }
    }
}
