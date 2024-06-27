using UnityEngine;
public class BaseController : MonoBehaviour
{
    [SerializeField] private Base this_Base;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if(bullet != null) 
            {
                this_Base.ReceiveDamage(bullet.Damage);
            }
        }
    }
}
