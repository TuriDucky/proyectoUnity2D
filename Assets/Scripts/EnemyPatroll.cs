using UnityEngine;

public class EnemyPatroll : MonoBehaviour
{
    public Collider2D coll;
    public bool hasDetectedPlayer;
    private void OnTriggerEnter2D(Collider2D collider2D){
        
        if(collider2D.gameObject.tag == "Player"){
            coll = collider2D;
            hasDetectedPlayer = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collider2D){
        
        if(collider2D.gameObject.tag == "Player"){
            coll = collider2D;
            hasDetectedPlayer = false;
        }
    }

    public bool getDetected(){
        return hasDetectedPlayer;
    }

    public Collider2D getplayer(){
        return coll;
    }
}
