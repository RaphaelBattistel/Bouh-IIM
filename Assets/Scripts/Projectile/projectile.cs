using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {

    // Script a mettre sur votre projectile que vous allez tirer depuis le script attackDIST
    // IL doit y avoir un TRIGGER sur l'objet et un rigidbody

    [SerializeField] private float attractionForce = 10f;
    public float lifeTime = 5.0f;   // Le temps maximal que vivra le projectile (pour être sur qu'il se détruise au bout d'un moment)
    public Possession PossesProj;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.tag != "Player")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Object"))
        {
            PossesProj = collision.gameObject.GetComponent<Possession>();

            if (PossesProj != null)
            {
                // Récupère le joueur dans la scène
                PlayerControllers player = FindObjectOfType<PlayerControllers>();

                // Active la possession
                PossesProj.StartPossession(player);

                // Détruire le projectile
                Destroy(gameObject);
            }
        }
    }

    
}
