using UnityEngine;
public class BaseController : MonoBehaviour
{
    [SerializeField] private Base thisBase;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if(bullet != null) 
            {
                thisBase.ReceiveDamage(bullet.Damage);
            }
        }
    }
}
