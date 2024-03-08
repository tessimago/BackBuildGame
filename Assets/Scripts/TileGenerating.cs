using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerating : MonoBehaviour
{
    public EnemyGenerator enemyGen;
    public GameObject player;
    public Tilemap tilemap;
    public Tilemap tileBackground;
    public Tile wall;
    public Tile defaultWall;
    public Tile[] floor;
    public Tile[] corners;

    public int xGen;
    public int yGenUP;
    public int yGenDOWN;
    public int yRef_UP;
    public int yRef_DOWN;
    public float midChance = 0.5f;
    float speedGen = 0.1f;
    bool changedDirection_UP;
    bool changedDirection_DOWN;
    public int simulatedMid = 0;
    public float enemySpawnChance = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        enemyGen = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
        Debug.Log(enemyGen);
        player = GameObject.Find("Player");
        changedDirection_UP = false;
        changedDirection_DOWN = false;
        xGen = 0;
        yGenUP = 0;
        yGenDOWN = 0;
        generateStartWall();
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
        if(changedDirection_UP || xGen <= 2){ 
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
        if(changedDirection_DOWN || xGen <= 2){ 
            changedDirection_DOWN = false;
            generateTile_DOWN();
        }else{
            yGenDOWN += chooseDir(yGenDOWN, false);
            generateTile_DOWN();
        }
    }

    int chooseDir(int yGen, bool isUp){
        int direction = 0;
        // go mid or no?
        float random = Random.Range(0f, 1f);
        // Chance to go to the middle
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
        // Set the correct tiles depending on their positioning
        tilemap.SetTile(posUP, wall);
        if(tilemap.GetTile(previousTile(posUP)) == null){
            if(tilemap.GetTile(previousTile(posUP + Vector3Int.down)) != null){
                tilemap.SetTile(previousTile(posUP), corners[1]);
                tilemap.SetTile(previousTile(posUP + Vector3Int.down), corners[3]);
            }
            else{
                tilemap.SetTile(previousTile(posUP), corners[2]);
                tilemap.SetTile(previousTile(posUP + Vector3Int.up), corners[0]);
            }
        }

        // Generate the floor
        int i = 0;
        posUP.y -= 1;
        posUP.x -= 1;
        while(tilemap.GetTile(posUP) == null || i <= 1){
            tileBackground.SetTile(posUP, pickFloor());
            // Chance to spawn enemy
            float random = Random.Range(0f, 1f);
            if(random < enemySpawnChance && xGen > 30){
                Vector3Int cellPosition = tilemap.WorldToCell(posUP);
                Debug.Log(enemyGen);
                enemyGen.spawnRandomEnemy(cellPosition);
            }
            
            posUP.y -= 1;
            if(xGen == 0 && i >= (yRef_UP-yRef_DOWN)){ // Trying to get the correct number of floors os the start
                break;
            }else if(i > 50){ // Just to ensure the game doesn't get stuck in an infinite loop
                break;
            }
            i++;
        }
    }
    Tile pickFloor(){
        if(Random.Range(0f, 1f) < 0.95f){
            return floor[0];
        }
        // Else return one of the others
        return floor[Random.Range(1, floor.Length)];
    }
    void generateTile_DOWN(){
        Vector3Int posDown = new Vector3Int(xGen, yGenDOWN + yRef_DOWN, 0);
        // Verify if it is closing the path
        if(isBlockingUp(posDown)){
            Debug.Log("Closing path WARNING UP");
            posDown.y += -1;
            yGenDOWN += -1;
        }
        // Set the correct tiles depending on their positioning 
        tilemap.SetTile(posDown, wall);
        if(tilemap.GetTile(previousTile(posDown)) == null){
            if(tilemap.GetTile(previousTile(posDown + Vector3Int.down)) != null){
                tilemap.SetTile(previousTile(posDown), corners[1]);
                tilemap.SetTile(previousTile(posDown + Vector3Int.down), corners[3]);
            }
            else{
                tilemap.SetTile(previousTile(posDown), corners[2]);
                tilemap.SetTile(previousTile(posDown + Vector3Int.up), corners[0]);
            }
        }

    }
    void generateStartWall(){
        int i = yRef_UP+1;
        while(i > yRef_DOWN-2){
            tilemap.SetTile(new Vector3Int(-1, i, 0), defaultWall);
            i--;
        }
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

    float MidChance(int y){
        if(midChance > 1f)
            midChance = 1f;
        else if(midChance < 0f)
            midChance = 0f;
        
        return - Mathf.Abs(Mathf.Atan(y - simulatedMid) * 2 * midChance/Mathf.PI) + midChance;
    }
    // The more u up, less chance to go up and vice versa
    float upChance(int y){
        return - (Mathf.Atan(y - simulatedMid)/Mathf.PI) + 0.5f;
    }
    void Update()
    {
        if(player.transform.position.x > xGen - 15){
            generate();
            //simulatedMid = Mathf.RoundToInt(5*Mathf.Sin(xGen * Mathf.PI/20));
        }
    }
}
