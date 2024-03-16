using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public GameObject[] options;
    
    public Card_Info[] allCards;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    Card_Info[] currOptions;
    public void RandomizeOptions(){
        if(options.Length == 0)
        {
            // Get all childreen
            options = new GameObject[transform.childCount];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = transform.GetChild(i).gameObject;
            }
        }
        
        currOptions = new Card_Info[transform.childCount];
        for(int i = 0; i < options.Length; i++){
            Card_Info card = GetRandomCard();
            setCardInfo(card, options[i]);
            currOptions[i] = card;
        }
    }


    Card_Info GetRandomCard(){
        Debug.Log("Choosing rangom card...");
        return allCards[Random.Range(0, allCards.Length)];
    }

    void setCardInfo(Card_Info info, GameObject buttonCard){
        Debug.Log("Setting...");
        buttonCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = info.name;
        buttonCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = info.effect;
    }

    public void ChooseCard(int optionNumber){
        setEffects(currOptions[optionNumber].effects_Info);
        gameObject.SetActive(false);
    }

    void setEffects(Effects_info effect){
        Debug.Log(effect._speedInc);
        var player_stats = player.GetComponent<PlayerStats>();
        var player_shooting = player.GetComponent<ShootingScript>();
        var player_movement = player.GetComponent<PlayerMov>();

        player_stats.maxHealth += effect._maxhpInc*player_stats.maxHealth/100;
        player_stats.health += effect._maxhpInc*player_stats.health/100;
        player_stats.updateBar();
        
        player_shooting.bulletDmg += effect._dmgInc*player_shooting.bulletDmg/100;
        player_shooting.spreadVar += effect._spreadShootInc*player_shooting.spreadVar/100f;
        player_shooting.shootDelay -= effect._shootSpeedInc*player_shooting.shootDelay/100f;
        player_shooting.bulletSpeed += effect._bulletSpeedInc*player_shooting.bulletSpeed/100f;
        player_shooting.bulletSpeed = Mathf.Min(player_shooting.bulletSpeed, 100);
        player_shooting.bulletKnock += effect._bulletKnockback*player_shooting.bulletKnock/100f;

        player_movement.speed += effect._speedInc*player_movement.speed/100f;
        Debug.Log("Effects applied");
    }
}
