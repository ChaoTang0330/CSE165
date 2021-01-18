using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public const int CONTINUE = 0;
    public const int PAUSE = 1;
    public const int CHANGED = 2;
    public const int FINISH = 3;

    public GameObject CrosshairCanvas;
    public GameObject SettingCanvas;
    public GameObject FinishCanvas;
    public Image pauseButton;
    public Text survivalTime;
    public Text finishText;
    public Sprite pauseSprite;
    public Sprite playSprite;

    private float mouseSensitive = 2.0f;
    private float spawnInterval = 6.0f;
    private float characterSpeed = 1.25f;

    private float timer = 0;
    private float finishTimer = 0;

    private int status = 0;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(status == CONTINUE)
        {
            timer += Time.deltaTime;
            int minutes = (int)timer / 60;
            int seconds = (int)timer % 60;
            survivalTime.text = string.Format("Survival {0:d2}:{1:d2}", minutes, seconds);
            //survivalTime.text = "Survival " + minutes + ":" + seconds;
        }
        else if(status == FINISH)
        {
            finishTimer -= Time.deltaTime;
            if(finishTimer <= 0)
            {
                finishTimer = 0;
                openMainGame();
            }
        }
        
    }

    // set values
    public void setMouseSensitive(float value)
    {
        status = CHANGED;
        mouseSensitive = 1.0f + value * 2.0f;
    }

    public void setSpawnInterval(float value)
    {
        status = CHANGED;
        spawnInterval = 3.0f + value * 6.0f;
    }

    public void setCharacterSpeed(float value)
    {
        status = CHANGED;
        characterSpeed = 0.5f + value * 1.5f;
    }

    //get values
    public float getMouseSensitive()
    {
        return mouseSensitive;
    }

    public float getSpawnInterval()
    {
        return spawnInterval;
    }

    public float getCharacterSpeed()
    {
        return characterSpeed;
    }

    //status
    public int getStatus()
    {
        return status;
    }

    public void setPauseStatus()
    {
        status = PAUSE;
    }

    //public void setContinueStatus()
    //{
    //    status = CONTINUE;
    //    CrosshairCanvas.SetActive(true);
    //    SettingCanvas.SetActive(false);
    //    FinishCanvas.SetActive(false);
    //    pauseButton.sprite = pauseSprite;
    //}

    public void setFinishStatus()
    {
        status = FINISH;
        finishTimer = 5.0f;
        CrosshairCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        FinishCanvas.SetActive(true);

        finishText.text = string.Format("You survived {0:d} seconds.", (int)timer);
        timer = 0.0f;
    }

    //activity
    public void openSetting()
    {
        status = PAUSE;
        CrosshairCanvas.SetActive(false);
        SettingCanvas.SetActive(true);
    }

    public void openMainGame()
    {
        if(status == CHANGED)
        {
            timer = 0.0f;
        }
        status = CONTINUE;
        CrosshairCanvas.SetActive(true);
        SettingCanvas.SetActive(false);
        FinishCanvas.SetActive(false);
        pauseButton.sprite = pauseSprite;
    }

    public void pauseGame()
    {
        if(status == CONTINUE)
        {
            status = PAUSE;
            pauseButton.sprite = playSprite;
        }
        else if(status == PAUSE)
        {
            status = CONTINUE;
            pauseButton.sprite = pauseSprite;
        }
    }

    public void closeGame()
    {
        Application.Quit();
    }
}
