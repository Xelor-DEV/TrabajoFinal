using UnityEngine;
using System.Collections;
public class BeetleBlasterController: Robot
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletInterval;
    private void Start()
    {
        StartCoroutine(GenerateBullets());
    }
    private IEnumerator GenerateBullets()
    {
        Bullet tmp = bullet.GetComponent<Bullet>();
        tmp.SetDamage(damage);
        Instantiate(bullet, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(bulletInterval);
        StartCoroutine(GenerateBullets());
    }
}