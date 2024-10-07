using UnityEngine;

public class EnemyPatroll : MonoBehaviour
{
    public static Collider2D coll;
    public static bool hasDetectedPlayer;
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
}
