using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GridLayoutGroup CardHolder;
    [SerializeField] private GameObject robotCardPrefab;
    [SerializeField] private Base playerBase;
    [SerializeField] private Base botBase;
    [SerializeField] private Slider playerLife;
    [SerializeField] private Slider botLife;
    [SerializeField] private RectTransform win;
    [SerializeField] private RectTransform lose;
    [SerializeField] private TMP_Text money;
    [SerializeField] private float offset;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [Header("Music Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public Slider MasterSlider
    {
        get
        {
            return masterSlider;
        }
    }
    public Slider MusicSlider
    {
        get
        {
            return musicSlider;
        }
    }
    public Slider SfxSlider
    {
        get
        {
            return sfxSlider;
        }
    }

    public void ShowWin()
    {
        win.DOAnchorPosY(win.anchoredPosition.y - offset, duration).SetEase(ease).OnComplete(() => Time.timeScale = 0);
    }
    public void ShowLose()
    {
        lose.DOAnchorPosY(lose.anchoredPosition.y - offset, duration).SetEase(ease).OnComplete(() => Time.timeScale = 0);
    }
    private void OnEnable()
    {
        playerInventory.OnInventoryUpdated += UpdateDisplayedRobots;
        playerBase.onBaseAttacked += UpdatePlayerLifeBar;
        botBase.onBaseAttacked += UpdateBotLifeBar;
        botBase.onBaseDestroyed += ShowWin;
        playerBase.onBaseDestroyed += ShowLose;
    }
    private void OnDisable()
    {
        playerInventory.OnInventoryUpdated -= UpdateDisplayedRobots;
        playerBase.onBaseAttacked -= UpdatePlayerLifeBar;
        botBase.onBaseAttacked -= UpdateBotLifeBar;
        botBase.onBaseDestroyed -= ShowWin;
        playerBase.onBaseDestroyed -= ShowLose;
    }
    private void Start()
    {
        Time.timeScale = 1;
        playerLife.maxValue = playerBase.MaxLife;
        botLife.maxValue = botBase.MaxLife;
        UpdatePlayerLifeBar(playerBase.Life);
        UpdateBotLifeBar(botBase.Life);
    }
    public void UpdatePlayerLifeBar(int newLife)
    {
        playerLife.value = newLife;
    }
    public void UpdateBotLifeBar(int newLife)
    {
        botLife.value = newLife;
    }
    private void UpdateDisplayedRobots(int start)
    {
        for (int i = 0; i < CardHolder.transform.childCount; ++i)
        {
            GameObject child = CardHolder.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        for (int i = 0; i < playerInventory.DisplayedRobots.Length; ++i)
        {
            RobotCard robot = playerInventory.DisplayedRobots[i];
            if (robot != null)
            {
                GameObject robotCard = Instantiate(robotCardPrefab, CardHolder.transform);
                RobotCardController robotCardController = robotCard.GetComponent<RobotCardController>();
                robotCardController.SetData(robot, start + i);
            }
        }
    }
    public void UpdateMoneyDisplay(int money)
    {
        this.money.text = money.ToString();
    }
}