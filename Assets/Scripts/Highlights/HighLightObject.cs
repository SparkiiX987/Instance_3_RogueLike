using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightObject : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private float distance;
    [SerializeField] private Transform player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        print(player);
        if(player == null)
        { StartCoroutine(GetPlayer()); }
    }

    private IEnumerator GetPlayer()
    {
        print("dans la coroutine");
        do
        {
            print("cherche le player");
            GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo is null) { yield break; }
            player = playerGo.transform;
            if (player is not null)
            {
                print("trouver");
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        } while (player == null);
    }

    private void Update()
    {
        if (player == null) { print("returned"); return; }
        
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? sprites[0] : sprites[1];
    }
}
