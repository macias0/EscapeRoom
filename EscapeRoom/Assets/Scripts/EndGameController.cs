using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour, IUsable
{

    [SerializeField]
    private GameObject endGameScreen;


    [SerializeField]
    private float secondsToExitGame = 5;

    public Item Use(Item other = null)
    {
        endGameScreen.SetActive(true);

        StartCoroutine(ExitGame(secondsToExitGame));

        return null;
    }

    private IEnumerator ExitGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //load main menu scene
        SceneManager.LoadScene(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
