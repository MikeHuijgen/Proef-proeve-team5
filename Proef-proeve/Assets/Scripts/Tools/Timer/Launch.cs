using UnityEngine;

public class Launch : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Transform startLocation;
    [SerializeField] private Transform endLocation;
    [SerializeField] private GameObject canon;

    [SerializeField] float travelTime;

    private float timer;
    private float elapsedTime;
    private float planetDistance;
    private float journey;

    private bool isTravel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        planetDistance = Vector3.Distance(startLocation.position, endLocation.position);
    }

    // Update is called once per frame
    private void Update()
    {
    // Check if travel is true, and if it is Gameobject start traveling.
        if (isTravel)
        {
            elapsedTime = (Time.time - timer) * speed;
            Traveling();
        }
    }

    private void Traveling()
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
        }
    }
}
