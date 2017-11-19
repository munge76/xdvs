using UnityEngine;

public class TutorialSprite : MonoBehaviour
{

    [SerializeField] private tk2dBaseSprite sprite;
    private SpriteFromRes spriteFromRes;

    public tk2dBaseSprite Sprite { get { return sprite; } }

	public void Initialize ()
	{
	    SetSpriteFromRes();
	}

    private void SetSpriteFromRes()
    {
        spriteFromRes = GetComponent<SpriteFromRes>();

        if (spriteFromRes == null)
            return;

        string texName = null;
        Vector2 dimensions = Vector2.zero;

        switch (GameData.ClearGameFlags(GameData.CurrentGame))
        {
            case Game.WWT2:
                texName = "tutorCharacter";
                dimensions = new Vector2(304, 302);
                break;
        }

        spriteFromRes.SetTexture(texName, dimensions.x, dimensions.y);
    }
}
