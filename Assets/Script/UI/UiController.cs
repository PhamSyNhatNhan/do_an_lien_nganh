using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [Header("Number")]
    [SerializeField] private Sprite number_1_Ui;
    [SerializeField] private Sprite number_2_Ui;
    [SerializeField] private Sprite number_3_Ui;
    [SerializeField] private List<Sprite> numberUI = new List<Sprite>();
    
    [Header("Controller")] 
    [SerializeField] private GameObject mobileUi;
    [SerializeField] private GameObject pcUi;
    [SerializeField] private GameObject mobileUiDash;
    [SerializeField] private GameObject pcUiDash;
    
    [Header("State")] 
    [SerializeField] private GameObject mobileHpUi;
    [SerializeField] private GameObject mobileManaUi;
    [SerializeField] private GameObject pcHpUi;
    [SerializeField] private GameObject pcManaUi;
    
    [Header("interact")]
    [SerializeField] private GameObject interactUi;
    private bool statusInteractUi = false;

    [Header("Normal")] 
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    
    [Header("Countdown")]
    [SerializeField] private GameObject countdownUi;
    [SerializeField] private GameObject countdownNumber;
    
    void Start()
    {
        
        if (Application.isMobilePlatform)
        {
            mobileUi.SetActive(true);
            pcUi.SetActive(false);
        }
        else
        {
            mobileUi.SetActive(false);
            pcUi.SetActive(true);
        }
    }

    public void setActiveInteract(bool active)
    {
        statusInteractUi = active;
        interactUi.SetActive(active);
    }

    public void updateNumberDash(int number)
    {
        if (Application.isMobilePlatform)
        {
            mobileUiDash.GetComponent<Image>().sprite = numberUI[number];
        }
        else
        {
            pcUiDash.GetComponent<Image>().sprite = numberUI[number];
        }
    }

    public IEnumerator countDownScene()
    {
        countdownUi.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            if(i==0) countdownNumber.GetComponent<Image>().sprite = number_3_Ui;
            if(i==1) countdownNumber.GetComponent<Image>().sprite = number_2_Ui;
            if(i==2) countdownNumber.GetComponent<Image>().sprite = number_1_Ui;

            yield return new WaitForSeconds(1.0f);
        }
        countdownUi.SetActive(false);
    }
    
    void Update()
    {
        
    }

    public GameObject MobileHpUi
    {
        get => mobileHpUi;
        set => mobileHpUi = value;
    }

    public GameObject MobileManaUi
    {
        get => mobileManaUi;
        set => mobileManaUi = value;
    }

    public GameObject PCHpUi
    {
        get => pcHpUi;
        set => pcHpUi = value;
    }

    public GameObject PCManaUi
    {
        get => pcManaUi;
        set => pcManaUi = value;
    }

    public GameObject InteractUi
    {
        get => interactUi;
        set => interactUi = value;
    }

    public bool StatusInteractUi
    {
        get => statusInteractUi;
        set => statusInteractUi = value;
    }

    public GameObject PauseMenu
    {
        get => pauseMenu;
        set => pauseMenu = value;
    }

    public GameObject MainMenu
    {
        get => mainMenu;
        set => mainMenu = value;
    }

    public GameObject SettingMenu
    {
        get => settingMenu;
        set => settingMenu = value;
    }
}
