using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    //Componentes
    private Rigidbody _compRigidbody;
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerInventory inventory;
	[SerializeField] private GameGrid grid;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Base playerBase;
    [SerializeField] private BaseController botBase;
    [SerializeField] private BaseController baseController;
    public event Action<bool> onRobotPlacement;
    private bool isRobotPlace = false;
    public bool IsRobotPlace
    {
        get 
        { 
            return isRobotPlace; 
        }
        set
        {
            isRobotPlace = value;
            if(isRobotPlace == true)
            {
                onRobotPlacement?.Invoke(isRobotPlace);
                isRobotPlace = false;
            }      
        }
    }
    [Header("Camera Properties")]
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float screenDistanceDetection; // Valores de 0 a 1
    [SerializeField] float speed;
    [Header("Properties")]
    private Vector2 _mousePosition;
    private Vector2 _directionXZ;
    private float _directionY;
    private bool _leftClick = false;
    private bool _rightClick = false;
    [SerializeField] private int money; // Esta variable representa la cantidad de photon credits que tiene el jugador
    [SerializeField] private RobotCard currentData;
    [SerializeField] private GameObject currentRobot;
    [SerializeField] private LayerMask slabLayer;
    private RaycastHit hitInfo;
    [SerializeField] private int originalLayer = -1;
    public PlayerInventory Inventory
    {
        get
        {
            return inventory;
        }
        set
        {
            inventory = value;
        }
    }
    public BaseController BotBase
    {
        get
        {
            return botBase;
        }
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        _compRigidbody = GetComponent<Rigidbody>();
    }
    public void GetDirectionXZ(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        _directionXZ = new Vector2(direction.x * speed, direction.y * speed);
    }
    public void GetDirectionY(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        _directionY = direction * speed;
    }
    public void GetLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _leftClick = true;
        }
        else if (context.canceled)
        {
            _leftClick = false;
        }
    }
    public void GetRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _rightClick = true;
        }
        else if (context.canceled)
        {
            _rightClick = false;
        }
    }
    public void GetMousePosition(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }
    private void Update()
    {
        playerCamera.transform.position = transform.position;
        RotateCamera();
        PlaceRobot();
    }
    public void PlaceRobot()
    {
        if (currentRobot != null)
        {     
            Ray ray = playerCamera.ScreenPointToRay(_mousePosition);
            if (originalLayer == -1)
            {
                originalLayer = currentRobot.layer;
            }
            if (Physics.Raycast(ray, out hitInfo, 100f, slabLayer))
            {
                currentRobot.layer = LayerMask.NameToLayer("Ignore");
                currentRobot.transform.position = hitInfo.point;
            }
            if (_leftClick == true && currentRobot != null && currentData != null)
            {
                if (hitInfo.collider != null && hitInfo.collider.tag == "Slab")
                {
                    SlabController slab = hitInfo.collider.gameObject.GetComponent<SlabController>();
                    if (grid.Robots[slab.XIndex, slab.YIndex] == null)
                    {
                        currentRobot.layer = originalLayer;
                        Robot tmp = currentRobot.GetComponent<Robot>();
                        grid.Robots[slab.XIndex, slab.YIndex] = tmp;
                        tmp.SetData(currentData);
                        currentRobot.transform.position = grid.Slabs[slab.XIndex, slab.YIndex].transform.position;
                        isRobotPlace = true;
                        currentData = null;
                        currentRobot = null;
                        _leftClick = false;
                        originalLayer = -1;
                    }
                }
            }
            if (_rightClick == true && currentRobot != null)
            {
                currentData.RobotPrefab.layer = originalLayer;
                inventory.AddRobot(currentData);
                Destroy(currentRobot);
                currentData = null;
                currentRobot = null;
                _rightClick = false;
                originalLayer = -1;
            }
        }
    }
    private void RotateCamera()
    {
        Vector2 viewportMousePosition = playerCamera.ScreenToViewportPoint(_mousePosition);
        Quaternion currentRotation = playerCamera.transform.rotation;
        float maxAngle = 85f;
        float minAngle = -85f;
        Quaternion xRotationDelta = Quaternion.identity;
        Quaternion yRotationDelta = Quaternion.identity;

        if (viewportMousePosition.y >= 1 - screenDistanceDetection)
        {
            xRotationDelta = Quaternion.Euler(-cameraRotationSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (viewportMousePosition.y <= screenDistanceDetection)
        {
            xRotationDelta = Quaternion.Euler(cameraRotationSpeed * Time.deltaTime, 0f, 0f);
        }

        if (viewportMousePosition.x >= 1 - screenDistanceDetection)
        {
            yRotationDelta = Quaternion.Euler(0f, cameraRotationSpeed * Time.deltaTime, 0f);
        }
        else if (viewportMousePosition.x <= screenDistanceDetection)
        {
            yRotationDelta = Quaternion.Euler(0f, -cameraRotationSpeed * Time.deltaTime, 0f);
        }
        Quaternion targetRotation = currentRotation * yRotationDelta * xRotationDelta;
        Vector3 eulerRotation = targetRotation.eulerAngles;
        eulerRotation.z = 0f;
        float clampedXAngle = eulerRotation.x;
        if (clampedXAngle > 180f)
        {
            clampedXAngle -= 360f;
        }
        clampedXAngle = Mathf.Clamp(clampedXAngle, -maxAngle, -minAngle);
        eulerRotation.x = clampedXAngle;
        playerCamera.transform.rotation = Quaternion.Euler(eulerRotation);
    }
    private void FixedUpdate()
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0; 
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        Vector3 targetDirection = forward * _directionXZ.y + right * _directionXZ.x;
        _compRigidbody.velocity = targetDirection.normalized * speed + new Vector3(0, _directionY, 0);
    }
    public bool SetCurrentRobot(RobotCard currentRobot)
    {
        bool established = false;
        if (currentData == null)
        {
            currentData = currentRobot;
            Robot robot = currentData.RobotPrefab.GetComponent<Robot>();
            robot.Player = this.gameObject.GetComponent<PlayerController>();
            this.currentRobot = Instantiate(currentRobot.RobotPrefab, hitInfo.point, currentRobot.RobotPrefab.transform.rotation);
            established = true;
            return established;
        }
        else
        {
            established = false;
            return established;
        }
    }
    public void AddMoney(int amount)
    {
        Money = Money + amount;
    }
    public void SubstractMoney(int amount)
    {
        Money = Money - amount;
    }
    public int Money
    {
        get { return money; }
        private set
        {
            money = value;
            uiManager.UpdateMoneyDisplay(money);
        }
    }
}
