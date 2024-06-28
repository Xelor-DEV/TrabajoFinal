using UnityEngine;
using UnityEngine.InputSystem;

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
    private Vector2 _mousePosition;
    private Vector2 _directionXZ;
    private float _directionY;
    private bool _leftClick = false;
    [Header("Camera Properties")]
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float screenDistanceDetection; // Valores de 0 a 1
    [SerializeField] float speed;
    [Header("Properties")]
    [SerializeField] private int money; // Esta variable representa la cantidad de photon credits que tiene el jugador
    [SerializeField] private RobotCard currentData;
    [SerializeField] private GameObject currentRobot;
    [SerializeField] private LayerMask slabLayer;
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
    public RobotCard Current
    {
        get
        {
            return currentData;
        }
        set
        {
            currentData = value;
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
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100f, slabLayer))
            {
                currentRobot.transform.position = hitInfo.point;
            }
            if (_leftClick == true && currentRobot != null && currentData != null)
            {
                if (hitInfo.collider != null && hitInfo.collider.tag == "Slab")
                {
                    SlabController slab = hitInfo.collider.gameObject.GetComponent<SlabController>();
                    if (grid.Robots[slab.XIndex, slab.YIndex] == null)
                    {
                        Robot tmp = currentRobot.GetComponent<Robot>();
                        grid.Robots[slab.XIndex, slab.YIndex] = tmp;
                        tmp.SetData(currentData);
                        currentRobot.transform.position = grid.Slabs[slab.XIndex, slab.YIndex].transform.position;
                        currentData = null;
                        currentRobot = null;
                        _leftClick = false;
                    }
                }
            }
        }
    }
    private void RotateCamera()
    {
        Vector3 rotation = Vector3.zero;
        Vector2 viewportMousePosition = playerCamera.ScreenToViewportPoint(_mousePosition);
        if (viewportMousePosition.y >= 1 - screenDistanceDetection)
        {
            rotation.x = rotation.x - cameraRotationSpeed * Time.deltaTime;
        }
        else if (viewportMousePosition.y <= screenDistanceDetection)
        {
            rotation.x = rotation.x + cameraRotationSpeed * Time.deltaTime;
        }
        if (viewportMousePosition.x >= 1 - screenDistanceDetection)
        {
            rotation.y = rotation.y + cameraRotationSpeed * Time.deltaTime;
        }
        else if (viewportMousePosition.x <= screenDistanceDetection)
        {
            rotation.y = rotation.y - cameraRotationSpeed * Time.deltaTime;
        }
        playerCamera.transform.eulerAngles = playerCamera.transform.eulerAngles + rotation;
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
            if (currentData.RobotPrefab.CompareTag("SolarisSentinel") == true)
            {
                
                SolarisSentinelController sentinel = currentData.RobotPrefab.GetComponent<SolarisSentinelController>();
                sentinel.Player = this.gameObject.GetComponent<PlayerController>();
                Debug.Log("registrado");
                sentinel.onMoneyGenerated += AddMoney;
            }
            else
            {
                Debug.Log("f2");
            }
            this.currentRobot = Instantiate(currentRobot.RobotPrefab);
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
