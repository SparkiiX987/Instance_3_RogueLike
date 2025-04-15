using System.Collections.Generic;
using UnityEngine;
using static Save;

public static class Save
{
    public static string moneySaveKey => "Money";
    public static string questsSaveKey => "Quests";
    public static string inventorySaveKey => "Inventory";
    public static string loreSaveKey => "lorePage";

    //Money
    public static void SaveMoney(int _value)
    {
        PlayerPrefs.SetInt(moneySaveKey, _value);
    }
    public static int GetMoney(int _value)
    {
        if (PlayerPrefs.HasKey(moneySaveKey))
        {
            return PlayerPrefs.GetInt(moneySaveKey);
        }
        return 0;
    }

    //Quests
    public static void SaveQuests()
    {
        Shop shop = Shop.Instance;
        Quests quests = new Quests();
        for (int i = 0; i < shop.questsAvailables.Length; i++)
        {
            QuestInfos questInfos = new QuestInfos();
            if (shop.questsAvailables[i] != null && shop.questsAvailables[i].questData != null)
            {
                questInfos.id = shop.questsAvailables[i].questData.id;
                questInfos.customer = shop.questsAvailables[i].customer.name;
            }
            else
            {
                questInfos.id = -1;
                questInfos.customer = "nothing";
            }
            questInfos.questAccepted = shop.questsAvailables[i].questAccepted;
            quests.questsDatas.Add(questInfos);
        }
        string json = JsonUtility.ToJson(quests);
        PlayerPrefs.SetString(questsSaveKey, json);

        Debug.Log(json);
    }

    public static bool LoadQuests()
    {
        if (PlayerPrefs.HasKey(questsSaveKey))
        {
            string json = PlayerPrefs.GetString(questsSaveKey);
            Quests quests = JsonUtility.FromJson<Quests>(json);
            Debug.Log(json);
            Shop shop = Shop.Instance;
            for (int i = 0; i < shop.questsAvailables.Length; i++)
            {
                if (shop.questsAvailables[i] != null)
                {
                    shop.questsAvailables[i].questData = GetQuestScriptableObject(quests.questsDatas[i].id, shop.questsAvailables[i].questsPossibles);
                    shop.questsAvailables[i].customer = GetSpriteCustomer(quests.questsDatas[i].customer, shop.questsAvailables[i].questData.customerAvailables);
                    shop.questsAvailables[i].questAccepted = quests.questsDatas[i].questAccepted;
                }
            }
            return true;
        }
        return false;
    }

    public static QuestScriptableObject GetQuestScriptableObject (int _idQuest, List<QuestScriptableObject> _questScripts)
    {
        if (_idQuest == -1 ) { return null; }

        foreach (QuestScriptableObject scriptableObject in _questScripts)
        {
            if (scriptableObject.id == _idQuest)
            {
                return scriptableObject;
            }
        }

        return null;
    }

    public static Sprite GetSpriteCustomer(string _nameSprite, List<Sprite> _customers)
    {
        if (_nameSprite == "nothing") { return null; }

        foreach (Sprite customer in _customers)
        {
            if (customer.name == _nameSprite)
            {
                return customer;
            }
        }

        return null;
    }

    //Inventory
    public static void SaveInventory()
    {
        Transform entities = GameObject.Find("Entities").transform;
        PlayerControl player = entities.gameObject.transform.GetChild(0).GetComponent<PlayerControl>();
        InventoryData inventoryData = new InventoryData();
        inventoryData.sellableObject = player.sellableObject;
        inventoryData.usableObject = player.usableObject;
        inventoryData.collectableObject = player.collectableObject;

        string json = JsonUtility.ToJson(inventoryData);
        PlayerPrefs.SetString(inventorySaveKey, json);

    }
    public static bool LoadInventory()
    {
        if (PlayerPrefs.HasKey(inventorySaveKey))
        {
            string json = PlayerPrefs.GetString(inventorySaveKey);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

            Transform entities = GameObject.Find("Entities").transform;
            PlayerControl player = entities.gameObject.transform.GetChild(0).GetComponent<PlayerControl>();

            player.sellableObject = inventoryData.sellableObject;
            player.usableObject = inventoryData.usableObject;
            player.collectableObject = inventoryData.collectableObject;
            return true;
        }
        return false;
    }

    //

    [System.Serializable]
    public class InventoryData
    {
        public SellableObject sellableObject;
        public UsableObject usableObject;
        public CollectableItem collectableObject;
    }

    [System.Serializable]
    public class Quests
    {
        public List<QuestInfos> questsDatas = new List<QuestInfos>();
    }

    [System.Serializable]
    public class QuestInfos
    {
        public int id;
        public string customer;
        public bool questAccepted;
    }

}
