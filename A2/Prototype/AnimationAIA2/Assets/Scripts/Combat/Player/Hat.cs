using UnityEngine;

public class Hat : MonoBehaviour
{
    CombatStateManager stateManager;
    
    public float damage;
    public float kunaiSpeed;
    public float kunaiTime;
    public bool hatReturn, hatStay;
    public Vector3 playerPosition;
    
    private Rigidbody kunaiRB;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kunaiRB = GetComponent<Rigidbody>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        stateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatStateManager>();
        kunaiTime = 0f;
        hatReturn = false;
        hatStay = false;
    }

    void Update()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        kunaiTime += Time.deltaTime;
        if (kunaiTime > 1f)
        {
            hatStay = true;
        }
        
        if (kunaiTime > 2f)
        {
            hatReturn = true;
            hatStay = false;
        }
    }
    
    void FixedUpdate()
    {
        if (hatReturn == false &&  hatStay == false)
        {
            kunaiRB.MovePosition(kunaiRB.position + transform.forward * kunaiSpeed * Time.fixedDeltaTime);
        }
        else if (hatStay)
        {
            kunaiRB.linearVelocity = Vector3.zero;
            kunaiRB.angularVelocity = Vector3.zero;
        }
        else if (hatReturn)
        {
            kunaiRB.MovePosition(Vector3.MoveTowards(kunaiRB.position, playerPosition, kunaiSpeed * Time.fixedDeltaTime));
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            stateManager.playerHat.SetActive(true);
            Destroy(gameObject);
        }
    }
}
