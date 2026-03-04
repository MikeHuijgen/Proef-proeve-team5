using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] Transform startLocation;
    [SerializeField] Transform endLocation;
    [SerializeField] GameObject canon;

    [SerializeField] float travelTime;

    float timer;
    float elapsedTime;
    float planetDistance;
    float journey;

    bool isTravel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planetDistance = Vector3.Distance(startLocation.position, endLocation.position);
    }

    // Update is called once per frame
    void Update()
    {
    // Check if travel is true, and if it is Gameobject start traveling.
        if (isTravel)
        {
            elapsedTime = (Time.time - timer) * speed;
            Traveling();
        }
    }

    void Traveling()
    {
        // check the distandce between planets and devide it with the time it will take to get there.
        journey = elapsedTime / planetDistance;
        transform.position = Vector3.Lerp(startLocation.position, endLocation.position, journey);
        // reset the journey if it is done.
        if (journey >= 1f)
        {
            isTravel = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if player makes collision with somting, travel is true than.
        if (collision.gameObject == canon)
        {
            timer = Time.time;
            isTravel =  true;
            Debug.Log("travel");
        }
    }
}
