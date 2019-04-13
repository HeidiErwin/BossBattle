using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class MasterGameController : MonoBehaviour
{
    private int quarter = 1;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    public void NextLevel() {
        SceneManager.UnloadScene("Level" + quarter);
        quarter += 1;
        if (quarter == 4) {
            SceneManager.LoadScene("Title");
        } else {
            SceneManager.LoadScene("Level" + quarter, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
