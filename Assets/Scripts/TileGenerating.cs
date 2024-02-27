using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerating : MonoBehaviour
{
    public GameObject player;
    public Tilemap tilemap;
    public Tile wall;
    public Tile floor;

    public int xGen;
    public int yGenUP;
    public int yGenDOWN;
    public int yRef_UP;
    public int yRef_DOWN;
    float speedGen = 0.1f;
    public bool changedDirection;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        changedDirection = false;
        xGen = 0;
        yGenUP = 0;
        yGenDOWN = 0;

    }

    void generate(){
        basicChecks();
        // Se mudou de direçao ao gerar, gera outro em frente
        // para ficar mais bonito
        if(changedDirection){ 
            changedDirection = false;
            generateTile();
        }else{
            yGenUP += chooseDir(yGenUP);
            yGenDOWN += chooseDir(yGenDOWN);
            generateTile();
        }
    }
    int chooseDir(int yGen){
        int direction = 0;
        // go mid or no?
        float random = Random.Range(0f, 1f);
        // Chance de gerar no meio
        if(random > MidChance(yGen)){
            // failed to go mid, now go up or down?
            random = Random.Range(0f, 1f);
            if(random < upChance(yGen)){
                direction = 1;
            }else{
                direction = -1;
            }
        }
        if(direction != 0)
            changedDirection = true;

        return direction;
    }
    void generateTile(){
        Vector3Int posUP = new Vector3Int(xGen, yGenUP + yRef_UP, 0);
        Vector3Int posDown = new Vector3Int(xGen, yGenDOWN + yRef_DOWN, 0);
        // Verify if it is closing the path
        if(isBlockingDown(posUP)){
            Debug.Log("Closing path WARNING DOWN");
            posUP.y += 1;
            yGenUP += 1;
        }else if(isBlockingUp(posDown)){
            Debug.Log("Closing path WARNING UP");
            posDown.y += -1;
            yGenDOWN += -1;
        }

        tilemap.SetTile(posUP, wall);
        if(tilemap.GetTile(previousTile(posUP)) == null)
            tilemap.SetTile(previousTile(posUP), wall);

        tilemap.SetTile(posDown, wall);
        if(tilemap.GetTile(previousTile(posDown)) == null)
            tilemap.SetTile(previousTile(posDown), wall);

        xGen++;
    }

    Vector3Int previousTile(Vector3Int pos){
        return new Vector3Int(pos.x - 1, pos.y, 0);
    }

    bool isBlockingDown(Vector3Int pos){
        //tilemap.SetTile(new Vector3Int(pos.x - 1, pos.y - 2, 0), floor);
        //Debug.Log("[" + pos + "] BLOCK DOWN: " + tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y - 2, 0)).name);
        return  tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y - 2, 0)) != null; //&&
                //tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y - 2, 0)).name.Equals("Walls_0");
    }

    bool isBlockingUp(Vector3Int pos){
        //Debug.Log("[" + pos + "] BLOCK UP: " + tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y + 2, 0)));
        return  tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y + 2, 0)) != null; //&&
                //tilemap.GetTile(new Vector3Int(pos.x - 1, pos.y + 2, 0)).name.Equals("Walls_0");
    }


    void basicChecks(){
        if(speedGen < 0.1f){
            speedGen = 0.1f;
        }
    }

    float MidChance(int x){
        return 0.75f * Mathf.Pow(2,-Mathf.Abs(x));
    }
    float upChance(int x){
        if(x >= 0)
            return 0.5f * Mathf.Pow(2, -x);
        else
            return 0.5f + 0.5f * Mathf.Pow(2, x);
    }
    void Update()
    {
        if(player.transform.position.x > xGen - 10){
            generate();
        }
    }
}
