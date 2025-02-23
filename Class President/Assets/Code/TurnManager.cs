using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("Card Transforms")]
    [SerializeField] private Transform _playerHandHolder;

    [Header("Cards")]
    [SerializeField] private Card _popularity;
    [SerializeField] private Card _bully;
    [SerializeField] private Card _nerd;
    [SerializeField] private Card _locker;
    [SerializeField] private Card _challenge;

    [Header("Hands")]
    private List<Card> _playerHand = new List<Card>();
    private List<Card> _aiHand = new List<Card>();

    void Start()
    {
        DrawFirstHand();
    }
    void DrawFirstHand()
    {
            //give 3 cards to the player
            for(int i = 0; i < 3; i++)
            {
                //create a random card 
                var newCard = Instantiate(DrawRandomCard(), _playerHandHolder);
                //set it's pos to playerdeck
                newCard.transform.position = _playerHandHolder.transform.position;
                _playerHand.Add(newCard);
            }
            //give 3 cards to the ai
            for(int i = 0; i < 3; i++){
                //have some sort of sprite that shows how many cards the ai has? we don't really need to instantiate them...
                _playerHand.Add(DrawRandomCard());
            }

            //then, proceed to let the player start first
            PlayerTurn();
            
    }


    private Card DrawRandomCard()
    {
        int i = Random.Range(0, 4);

        switch(i){
            case 0:
                return _popularity;
            case 1:
                return _bully;
            case 2:
                return _nerd;
            case 3: 
                return _locker;
            case 4:
                return _challenge;
            default:
                Debug.Log("Error! Drew invalid card!");
                return _popularity;
        }
    }
    void PlayerTurn(){
        //first, draw a card for the player (if the playerhand isn't full)
        //then, let the player use (or discard) any of the cards they have in their deck.
        //then, once player has used their action, checkwin and switch to ai
        
        if(!CheckWin())
        {
            //If there is no win, game continues (duh)
            AiTurn();
        }
        
    }

    bool CheckWin(){
        //check influence of each player, then count nerdcards of each hand.
        //if there is a win:
        //  -set PlayerWin to either true or false
        //  -change scene to WinScene
        //  -display who won.
        //  -return true
        return false;
        
    }

    void AiTurn(){
        //if influence is less than max, try to use an influence card
        //if has bully, use bully
        //if has challenge, use challenge
        //if has locker, use locker

        if(!CheckWin()){
            PlayerTurn();
        }

    }
}
