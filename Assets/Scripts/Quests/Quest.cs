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
    public Sprite sellableObject;
    public Sprite customer;
    public bool questAccepted; 

    [Header("Quest Ui Elements"), HideInInspector]
    public Button button;
    public TMP_Text boxDescription;
    public Image image;

    private Save save => Save.Instance;
    private void Start()
    {
        button = transform.GetChild(0).GetComponent<Button>();
        boxDescription = transform.GetChild(1).GetComponentInChildren<TMP_Text>();
        image = transform.GetChild(2).GetComponentInChildren<Image>();
        
        Shop.Instance.questsAvailables[numberQuestIndex] = this;
        //PlayerPrefs.DeleteKey(save.questsSaveKey);
        if (!save.LoadQuests() || questData == null)
            GetRandomQuest();
        else
            GetQuestInfos();
    }

    public void AcceptQuest()
    {
        questAccepted = true;
        ChangeButtonFunction(questAccepted);

        save.SaveQuests();
        //SceneManager.LoadScene(1); // To replace with the index of the main game
    }

    public void CompletedQuest()
    {
        questAccepted = false;
        ChangeButtonFunction(questAccepted);

        PlayerMoney moneyPlayer = PlayerMoney.Instance;
        moneyPlayer.AddMoney(questData.rewards);

        GetRandomQuest();
        save.SaveQuests();
    }

    public void GetRandomQuest()
    {
        int randomIndex = UnityEngine.Random.Range(0, questsPossibles.Count);
        questData = questsPossibles[randomIndex];

        randomIndex = UnityEngine.Random.Range(0, questData.customerAvailables.Count);
        customer = questData.customerAvailables[randomIndex];
        GetQuestInfos();

        Shop shop = Shop.Instance;
        save.SaveQuests();
    }

    public void GetQuestInfos()
    {
        sellableObject = questData.goalObject;
        boxDescription.text = questData.description;
        image.sprite = customer;

        ChangeButtonFunction(questAccepted);
    }

    public void ChangeButtonFunction(bool _hasBeenAccepted)
    {
        if (_hasBeenAccepted)
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
