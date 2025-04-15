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
    private bool currentQuestCompleted; 

    [Header("Quest Ui Elements"), HideInInspector]
    public Button button;
    public TMP_Text boxDescription;
    public Image image;

    private void Start()
    {
        button = transform.GetChild(0).GetComponent<Button>();
        boxDescription = transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        image = transform.GetChild(2).GetComponentInChildren<Image>();

        // PlayerPrefs.DeleteKey(Save.questsSaveKey);
        bool loaded = Save.LoadQuests();
        Debug.Log(loaded);
        if (!loaded)
        {
            Debug.Log("Bouya");
            GetRandomQuest();
        }
        else
        {
            if (questData == null) { GetRandomQuest(); }
        }
    }

    public void AcceptQuest()
    {
        button.GetComponentInChildren<TMP_Text>().text = "Recuperer la recompense";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => CompletedQuest());
        //SceneManager.LoadScene(1); // To replace with the index of the main game
    }

    public void GetRandomQuest()
    {
        int randomIndex = UnityEngine.Random.Range(0, questsPossibles.Count);
        questData = questsPossibles[randomIndex];

        randomIndex = UnityEngine.Random.Range(0, questData.customerAvailables.Count);
        customer = questData.customerAvailables[randomIndex];
        rewards = questData.rewards;
        descriptionQuest = questData.description;
        sellableObject = questData.goalObject;

        boxDescription.text = descriptionQuest;
        image.sprite = customer;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => AcceptQuest());

        Shop shop = Shop.Instance;
        shop.questsAvailables[numberQuestIndex] = this;

        Save.SaveQuests();
    }

    public void CompletedQuest()
    {
        PlayerMoney moneyPlayer = PlayerMoney.Instance;
        moneyPlayer.AddMoney(rewards);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => AcceptQuest());

        button.GetComponentInChildren<TMP_Text>().text = "Accepter";
        GetRandomQuest();

        Save.SaveQuests();
    }


}
