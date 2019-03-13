using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    public Vector3 PreviousPosition = new Vector3(783, -17,0);  
    public Vector2 MapPosition = new Vector3(783, -17, 0);
    public int Key = 1;
    public bool ShowInventory = false;
    public string Content = "";
    public string LastScene = "Terrain";

    internal void Print()
    {
        print("********** Starter "+this.gameObject.name+ 
              " Key= " + Key + 
              " ShowInventory= " + ShowInventory + 
              " Content= " + Content +
              " LastScene= " + LastScene +
              " MapPosition= " + MapPosition +
              " PreviousPosition= " + PreviousPosition);
    }
}