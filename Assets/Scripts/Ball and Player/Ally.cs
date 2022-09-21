using UnityEngine;

public class Ally : MonoBehaviour
{
    [SerializeField] Animator anim;
    private Bixos thisPlayer;
    
    private void Awake()
    {
        thisPlayer = SpriteRotation.Player;
    }

    private void Start()
    {
        Idle();
    }

    public void Pull()
    {
        switch (thisPlayer)
        {
            case Bixos.Capivara:
                anim.Play("CapiPull");
                break;
            case Bixos.Mico:
                anim.Play("MicoPull");
                break;
            case Bixos.Onca:
                anim.Play("OncaPull");
                break;
            case Bixos.Tatu:
                anim.Play("TatuPull");
                break;
        }
    }

    public void Kick()
    {
        switch (thisPlayer)
        {
            case Bixos.Capivara:
                anim.Play("CapiKick");
                break;
            case Bixos.Mico:
                anim.Play("MicoKick");
                break;
            case Bixos.Onca:
                anim.Play("OncaKick");
                break;
            case Bixos.Tatu:
                anim.Play("TatuKick");
                break;
        }
    }

    public void Idle()
    {
        switch (thisPlayer)
        {
            case Bixos.Capivara:
                anim.Play("CapiIdle");
                break;
            case Bixos.Mico:
                anim.Play("MicoIdle");
                break;
            case Bixos.Onca:
                anim.Play("OncaIdle");
                break;
            case Bixos.Tatu:
                anim.Play("TatuIdle");
                break;
        }
    }
}
