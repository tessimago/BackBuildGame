using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerating : MonoBehaviour
{
    public GameObject player;
    public Tilemap tilemap;
    public Tilemap tileBackground;
    public Tile wall;
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
            Debug.Log("Floor set");
            tileBackground.SetTile(posUP, pickFloor());
            posUP.y -= 1;
            i++;
            if(i > 15){
                break;
            }
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
