using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    public int numParticles;

    public GameObject particle1;
    public GameObject particle2;
    public GameObject particle3;
    public GameObject particle4;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision2D){
        
        if (collision2D.tag == "Attack"){
            createParticles();
            Destroy(gameObject);
        }
    }

    private void createParticles(){
        while (numParticles > 0){
            numParticles --;
            int particula = Random.Range(0,4);

            Vector2 posicion = transform.position;
            posicion.x += Random.Range(-5,5);
            posicion.y += Random.Range(-5,5);

            switch (particula){
                case 0:
                    GameObject laParticula = Instantiate(particle1, posicion, Quaternion.identity);
                    laParticula.layer = 6;
                    break;
                case 1:
                    GameObject laParicula2 = Instantiate(particle2, posicion, Quaternion.identity);
                    laParicula2.layer = 6;
                    break;
                case 2:
                    GameObject laParicula3 = Instantiate(particle3, posicion, Quaternion.identity);
                    laParicula3.layer = 6;
                    break;
                case 3:
                    GameObject laParicula4= Instantiate(particle4, posicion, Quaternion.identity);
                    laParicula4.layer = 6;
                    break;
            }
        }
    }
}
