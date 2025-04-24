using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static Save Instance;
    public string moneySaveKey => "Money";
    public string questsSaveKey => "Quests";
    public string inventorySaveKey => "Inventory";
    public string dictionaryItems => "DictionaryItems";
    public string loreSaveKey => "lorePage";

    public Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();
    public int lastIndex => items.Count;

    public GameObject collectableObject;

    private void Awake()
    {
        if (Save.Instance != null) { Destroy(this.gameObject); }
        Instance = this;
        if (PlayerPrefs.HasKey(dictionaryItems))
        {
            LoadTheDictionnary();
        }
    }

    //Money
    public void SaveMoney(int _value)
    {
        PlayerPrefs.SetInt(moneySaveKey, _value);
    }
    public int GetMoney(int _value)
    {
        if (PlayerPrefs.HasKey(moneySaveKey))
        {
            return PlayerPrefs.GetInt(moneySaveKey);
        }
        return 0;
    }

    //Quests
    public void SaveQuests()
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
                questInfos.questAccepted = shop.questsAvailables[i].questAccepted;
            }
            else
            {
                questInfos.id = -1;
                questInfos.customer = "nothing";
                questInfos.questAccepted = false;
            }
            quests.questsDatas.Add(questInfos);
        }
        string json = JsonUtility.ToJson(quests);
        PlayerPrefs.SetString(questsSaveKey, json);
    }

    public bool LoadQuests()
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

    public QuestScriptableObject GetQuestScriptableObject(int _idQuest, List<QuestScriptableObject> _questScripts)
    {
        if (_idQuest == -1) { return null; }

        foreach (QuestScriptableObject scriptableObject in _questScripts)
        {
            if (scriptableObject.id == _idQuest)
            {
                return scriptableObject;
            }
        }

        return null;
    }

    public Sprite GetSpriteCustomer(string _nameSprite, List<Sprite> _customers)
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

    public void SaveInventory(PlayerControl _player)
    {
        InventoryData inventoryData = new InventoryData();

        PickableObject pickableObject = _player.sellableObject as PickableObject;
        ItemData itemData = new ItemData();
        if (pickableObject != null)
        {
            itemData.price = pickableObject.price;
            itemData.name = pickableObject.name;
            itemData.description = pickableObject.description;
            foreach (KeyValuePair<int, ItemData> entry in items)
            {
                if (entry.Value.name == itemData.name && entry.Value.price == itemData.price && entry.Value.description == itemData.description)
                {
                    inventoryData.sellableObject = entry.Key;
                    break;
                }
            }
            if (inventoryData.sellableObject == -1)
            {
                items.Add(lastIndex, itemData);

                Debug.Log(items.Count);
                inventoryData.sellableObject = lastIndex - 1;
            }
        }
        else
        {
            inventoryData.sellableObject = -1;
        }

        pickableObject = _player.usableObject as PickableObject;
        if (pickableObject != null)
        {
            itemData.price = pickableObject.price; itemData.name = pickableObject.name; itemData.description = pickableObject.description;
            foreach (KeyValuePair<int, ItemData> entry in items)
            {
                if (entry.Value.name == itemData.name && entry.Value.price == itemData.price && entry.Value.description == itemData.description)
                {
                    inventoryData.usableObject = entry.Key;
                    break;
                }
            }
            if (inventoryData.usableObject == -1)
            {
                items.Add(lastIndex, itemData);
                inventoryData.usableObject = lastIndex - 1;
            }
        }
        else
        {
            inventoryData.usableObject = -1;
        }
        pickableObject = _player.collectableObject.Item;
        if (pickableObject != null)
        {
            itemData.price = pickableObject.price; itemData.name = pickableObject.name; itemData.description = pickableObject.description;
            foreach (KeyValuePair<int, ItemData> entry in items)
            {
                if (entry.Value.name == itemData.name && entry.Value.price == itemData.price && entry.Value.description == itemData.description)
                {
                    inventoryData.collectableObject = entry.Key;
                    Debug.Log("Hola");
                    break;
                }
            }
            if (inventoryData.collectableObject == -1)
            {
                items.Add(lastIndex, itemData);
                inventoryData.collectableObject = lastIndex - 1;
            }
        }
        else
        {
            inventoryData.collectableObject = -1;
        }

        string json = JsonUtility.ToJson(inventoryData);
        PlayerPrefs.SetString(inventorySaveKey, json);
        DisplayArray();
        SaveTheDictionnary();

    }

    public bool LoadInventory(PlayerControl _player)
    {
        if (PlayerPrefs.HasKey(inventorySaveKey))
        {
            string json = PlayerPrefs.GetString(inventorySaveKey);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);
            int id;
            ItemData itemData;
            if (inventoryData.sellableObject != -1)
            {
                /*SellableObject sellableObject = new SellableObject();
                id = inventoryData.sellableObject;
                itemData = items[id];
                sellableObject.price = itemData.price;
                sellableObject.name = itemData.name;
                sellableObject.description = itemData.description;*/
            }

            if (inventoryData.usableObject != -1)
            {
                /*id = inventoryData.usableObject;
                itemData = items[id];
                PickableObject pickableObject = new PickableObject();
                switch (itemData.name)
                {
                    case "WoodenPlank":
                        {
                            WoodenPlank woodenPlank = new WoodenPlank();
                            pickableObject = woodenPlank;
                            break;
                        }
                    case "MonsterCan":
                        {
                            MonsterCan monsterCan = new MonsterCan();
                            pickableObject = monsterCan;
                            break;
                        }
                    case "PepperSpray":
                        {
                            PepperSpray spray = new PepperSpray();
                            pickableObject = spray;
                            break;
                        }
                    case "EmptyBottle":
                        {
                            EmptyBottle bottle = new EmptyBottle();
                            pickableObject = bottle;
                            break;
                        }
                }
                pickableObject.price = itemData.price;
                pickableObject.name = itemData.name;
                pickableObject.description = itemData.description;*/
            }
            DisplayArray();
            if (inventoryData.collectableObject != -1)
            {
                GameObject objectCreated = Instantiate(collectableObject, transform.position, Quaternion.identity);
                CollectableItem collectable = objectCreated.GetComponent<CollectableItem>();
                objectCreated.SetActive(false);

                id = inventoryData.collectableObject;
                Debug.Log(items.Count);
                itemData = items[id];
                collectable.Item.price = itemData.price;
                collectable.Item.name = itemData.name;
                collectable.Item.description = itemData.description;
            }

            return true;
        }
        return false;
    }

    public void DisplayArray()
    {
        foreach (KeyValuePair<int, ItemData> entry in items)
        {
            Debug.Log(entry.Key.ToString() + ": " + entry.Value.ToString());
        }
    }

    public void SaveTheDictionnary()
    {
        DictinaryItem dictinary = new DictinaryItem();
        if (items.Count > 0)
        {
            foreach (KeyValuePair<int, ItemData> entry in items)
            {
                dictinary.ids.Add(entry.Key);
                dictinary.datas.Add(entry.Value);
            }
        }
        string json = JsonUtility.ToJson(dictinary);
        Debug.Log(json);
        PlayerPrefs.SetString(dictionaryItems, json);

    }

    public void LoadTheDictionnary()
    {
        string json = PlayerPrefs.GetString(dictionaryItems);
        DictinaryItem dict = JsonUtility.FromJson<DictinaryItem>(json);
        Debug.Log(json);

        items.Clear();
        if (dict != null && dict.ids != null && dict.datas != null)
        {
            for (int i = 0; i < dict.ids.Count; i++)
            {
                items.Add(dict.ids[i], dict.datas[i]);
            }
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public int sellableObject = -1;
        public int usableObject = -1;
        public int collectableObject = -1;


    }

    [System.Serializable]
    public class DictinaryItem
    {
        public List<int> ids = new List<int>();
        public List<ItemData> datas = new List<ItemData>();
    }

    [System.Serializable]
    public class ItemData
    {
        public int price;
        public string name;
        public string description;
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
    
    public int GetCurrentQuest()
    {
        if (PlayerPrefs.HasKey(questsSaveKey))
        {
            
            string json = PlayerPrefs.GetString(questsSaveKey);
            Quests quests = JsonUtility.FromJson<Quests>(json);
            for (int i = 0; i < quests.questsDatas.Count; i++)
            {
                if (quests.questsDatas[i].questAccepted)
                    return quests.questsDatas[i].id;
            }
        }

        return -1;
    }
}
