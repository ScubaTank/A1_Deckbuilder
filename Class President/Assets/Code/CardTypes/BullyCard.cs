using UnityEngine;
public class BullyCard : Card
{

    public override void Use(){
        turnManager.UseBullyCard(_myCardData.cardValue);
        Destroy(gameObject);
    }
}
