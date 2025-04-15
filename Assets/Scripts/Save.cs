using System.Collections.Generic;
using UnityEngine;

public static class Save 
{
    public static string moneySaveKey => "Money";
    public static string questsSaveKey => "Quests";
    public static string inventorySaveKey => "Inventory";
    public static string loreSaveKey => "lorePage";

    //Money
    public static void SaveMoney (int _value )
    {
        PlayerPrefs.SetInt(moneySaveKey, _value);
    }
    public static int GetMoney (int _value )
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
        Quests quest = new Quests();
        foreach (Quest questAvailable in shop.questsAvailables)
        {
            QuestData questData = new QuestData();
            if (questAvailable != null)
            {
                questData.sellableObject = questAvailable.sellableObject;
                questData.rewards = questAvailable.rewards;
                questData.customer = questAvailable.customer;
            }
            quest.questsDatas.Add(questData);
        }
        string json = JsonUtility.ToJson(quest);
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
            Debug.Log(shop.questsAvailables.Length);
            for (int i = 0; i < shop.questsAvailables.Length; i++)
            {
                if (quests.questsDatas[i] != null)
                {
                    Quest quest = new Quest();
                    quest.sellableObject = quests.questsDatas[i].sellableObject;
                    quest.rewards = quests.questsDatas[i].rewards;
                    quest.customer = quests.questsDatas[i].customer;
                    shop.questsAvailables[i] = quest;
                }
                else { shop.questsAvailables[i] = null; }
            }
            return true;
        }
        return false;
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
        public List<QuestData> questsDatas = new List<QuestData>();
    }

    [System.Serializable]
    public class QuestData
    {
        public SellableObject sellableObject;
        public int rewards;
        public Sprite customer;
    }

}
