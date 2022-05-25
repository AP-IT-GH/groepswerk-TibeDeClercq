using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLoss : MonoBehaviour
{
    public AudioSource Alarm;
    public GameObject Nuke;
    public AudioSource Nuke_Sound;
    public GameObject ShockWave_Particle;
    public GameObject Explosion_Particle;
    public AudioSource Explosion_Sound_1;
    public AudioSource Explosion_Sound_2;
    public AudioSource Explosion_Sound_3;
    public AudioSource Explosion_Sound_4;
    public GameObject Player;

    private bool start;
    // Start is called before the first frame update
    void Start()
    {
        this.start = true;
    }

    private void FixedUpdate()
    {
        if (this.start)
        {
            StartCoroutine(test());
            this.start = false;
        }
    }

    private IEnumerator test()
    {
        if (this.start)
        {
            this.Alarm.Play();
            yield return new WaitForSeconds(5);
            this.SpawnNuke();
            yield return new WaitForSeconds(5);
            this.Alarm.Stop();
            for(int i = 0; i < 100; i++)
            {
                this.SpawnExplosion();
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void SpawnNuke()
    {
        GameObject nuke = Instantiate(Nuke, this.transform.localPosition, this.transform.localRotation);
        this.Nuke_Sound.Play();
        GameObject shockWave = Instantiate(ShockWave_Particle, this.transform.localPosition, this.transform.localRotation);
    }

    private void SpawnExplosion()
    {
        int x_offset = Random.Range(-4, 5);
        int y_offset = Random.Range(0, 5);
        int z_offset = Random.Range(-4, 5);

        Vector3 offset = new Vector3(x_offset, y_offset, z_offset);

        GameObject explosion = Instantiate(Explosion_Particle, Player.transform.position + offset, Player.transform.rotation);

        int choice = Random.Range(0, 4);

        switch (choice)
        {
            case 0:
                AudioSource explosionSound1 = Instantiate(Explosion_Sound_1, Player.transform.position + offset, Player.transform.rotation);
                explosionSound1.Play();
                break;
            case 1:
                AudioSource explosionSound2 = Instantiate(Explosion_Sound_2, Player.transform.position + offset, Player.transform.rotation);
                explosionSound2.Play();

                break;
            case 2:
                AudioSource explosionSound3 = Instantiate(Explosion_Sound_3, Player.transform.position + offset, Player.transform.rotation);
                explosionSound3.Play();
                break;
            case 3:
                AudioSource explosionSound4 = Instantiate(Explosion_Sound_4, Player.transform.position + offset, Player.transform.rotation);
                explosionSound4.Play();
                break;
            default:
                break;
        }
    }
}
