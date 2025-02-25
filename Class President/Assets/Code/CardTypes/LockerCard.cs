using UnityEngine;

public class LockerCard : Card
{

    public override void Use(){
        turnManager.UseLockerCard(_myCardData.cardValue);
        Destroy(gameObject);
    }
}
