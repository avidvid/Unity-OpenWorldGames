using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlHelper : MonoBehaviour
{
    private static XmlHelper _xmlHelper;
    //Database
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    #region XmlHelper Instance
    public static XmlHelper Instance()
    {
        if (!_xmlHelper)
        {
            _xmlHelper = FindObjectOfType(typeof(XmlHelper)) as XmlHelper;
            if (!_xmlHelper)
                Debug.LogWarning("There needs to be one active _xmlHelper script on a GameObject in your scene.");
        }
        return _xmlHelper;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***XML*** Start!");
        _xmlHelper = XmlHelper.Instance();
    }


    internal void SaveItems(List<ItemContainer> itemContainer)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemContainer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, itemContainer);
        fs.Close();
    }

    internal List<ItemContainer> GetItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemContainer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var itemContainer = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
        return itemContainer;
    }
}
