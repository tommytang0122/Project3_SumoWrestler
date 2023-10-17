using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public GameObject endPanel;

    [SerializeField]
    GameObject[] players;

    private int liveplayer = 2;
    private int winerNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject.Find("collisionVFX(Clone)");
        //do delete

        //Debug.Log(liveplayer);
        if (liveplayer == 1)
        {
            for(int i =0;i < players.Length; i++)
            {    
                if(players[i].activeSelf)
                {
                    winerNumber = (i + 1);
                    Invoke("WinGame", 2.0f);
                }
            }
        }
    }

    void WinGame()
    {
        endPanel.SetActive(true);
        endPanel.GetComponent<TextMeshProUGUI>().text = "Player " + winerNumber.ToString() + " is the Winner";
        Invoke("ResetScene", 2.0f);
    }

    public void playerDie()
    {
        liveplayer--;
    }

    void ResetScene()
    {
        SceneManager.LoadScene(1);
    }
}
