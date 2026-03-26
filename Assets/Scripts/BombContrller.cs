using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;


public class BombContrller : MonoBehaviour
{
    [Header("Bomb Settings")]
    public GameObject bombPrefab;
    public KeyCode placeBombKey = KeyCode.Space;
    public float bombFuseTime = 3f;
    public int bombAmount = 1; 
    private int bombsRemaining;
    [Header("Explosion Settings")]

    public LayerMask explosionLayerMask;

    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRange = 1;
    [Header("Destructable Settings")]
    public Destructable destructablePrefab;
    public Tilemap destructableTiles;
    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }
    private void Update()
    {
        if (Input.GetKeyDown(placeBombKey) && bombsRemaining > 0)
        {
            StartCoroutine(PlaceBomb());
            
        }
    }
    private IEnumerator PlaceBomb()     
    {
       Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRange);
        Explode(position, Vector2.down, explosionRange);
        Explode(position, Vector2.left, explosionRange);
        Explode(position, Vector2.right, explosionRange);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int range)
    {
        if (range <= 0)
        {
            return;
        }

        position += direction;

        if(Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructble(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(range > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, range - 1);
    }
    private void ClearDestructble(Vector2 position)
    {
        Vector3Int cellPosition = destructableTiles.WorldToCell(position);
        TileBase tile = destructableTiles.GetTile(cellPosition);

        if (tile != null)
        {
            Instantiate(destructablePrefab, position, Quaternion.identity);
            destructableTiles.SetTile(cellPosition, null);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

}
