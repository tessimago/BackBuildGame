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
    public float midChanceDec = 2f;
    float speedGen = 0.1f;
    bool changedDirection_UP;
    bool changedDirection_DOWN;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        changedDirection_UP = false;
        changedDirection_DOWN = false;
        xGen = 0;
        yGenUP = 0;
        yGenDOWN = 0;

    }

    void generate(){
        basicChecks();
        generateUP();
        generateDOWN();
        
        xGen++;
    }
    void generateUP(){
        // Se mudou de direçao ao gerar, gera outro em frente
        // para ficar mais bonito
        if(changedDirection_UP){ 
            changedDirection_UP = false;
            generateTile_UP();
        }else{
            yGenUP += chooseDir(yGenUP, true);
            generateTile_UP();
        }
    }

    void generateDOWN(){
        // Se mudou de direçao ao gerar, gera outro em frente
        // para ficar mais bonito
        if(changedDirection_DOWN){ 
            changedDirection_DOWN = false;
            generateTile_DOWN();
        }else{
            yGenDOWN += chooseDir(yGenUP, false);
            generateTile_DOWN();
        }
    }

    int chooseDir(int yGen, bool isUp){
        int direction = 0;
        // go mid or no?
        float random = Random.Range(0f, 1f);
        // Chance de gerar no meio
        Debug.Log("Chance de gerar no meio: " + MidChance(yGen));
        Debug.Log("random: " + random);
        if(random > MidChance(yGen)){
            // failed to go mid, now go up or down?
            random = Random.Range(0f, 1f);
            if(random < upChance(yGen)){
                direction = 1;
            }else{
                direction = -1;
            }
        }
        if(direction != 0 && isUp)
            changedDirection_UP = true;
        else if (direction != 0 && !isUp)
            changedDirection_DOWN = true;

        return direction;
    }

    void generateTile_UP(){
        Vector3Int posUP = new Vector3Int(xGen, yGenUP + yRef_UP, 0);
        // Verify if it is closing the path
        if(isBlockingDown(posUP)){
            Debug.Log("Closing path WARNING DOWN");
            posUP.y += 1;
            yGenUP += 1;
        }

        tilemap.SetTile(posUP, wall);
        if(tilemap.GetTile(previousTile(posUP)) == null)
            tilemap.SetTile(previousTile(posUP), wall);
    }

    void generateTile_DOWN(){
        Vector3Int posDown = new Vector3Int(xGen, yGenDOWN + yRef_DOWN, 0);
        // Verify if it is closing the path
        if(isBlockingUp(posDown)){
            Debug.Log("Closing path WARNING UP");
            posDown.y += -1;
            yGenDOWN += -1;
        }

        tilemap.SetTile(posDown, wall);
        if(tilemap.GetTile(previousTile(posDown)) == null)
            tilemap.SetTile(previousTile(posDown), wall);

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
        return 0.75f * Mathf.Pow(midChanceDec,-Mathf.Abs(x));
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
