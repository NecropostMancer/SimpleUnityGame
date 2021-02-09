using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIBundle : UIBundle
{

    private Canvas UICanvasRoot;
    //private GameObject ammoInspector;
    public AmmoInspector m_AmmoInspectorRef; 
    //private GameObject aim;
    public AimController m_AimRef;



    public HPBar m_HP;

    public Animator m_WaveUp;
    public Animator m_WaveBottom;
    public TextMeshProUGUI m_UpText;
    public TextMeshProUGUI m_BottomText;
    public Text m_FireMode;
    
    private int curAmmo;
    private int maxAmmo;
    private int maxMaga;
    


    // Start is called before the first frame update
    void Start()
    {
        GameUIManager.instance.InstallUI(this);
        UICanvasRoot = this.GetComponent<Canvas>();
        
    }

    public void AimChange(float str)
    {
        m_AimRef.Expand(str);
        
    }
    public void ResetAim(float a)
    {
        m_AimRef.ResetAim(a);
    }
    public void AmmoReset(int maxAmmo,int maxBack, int curAmmo)
    {
        this.maxAmmo = maxAmmo;
        this.curAmmo = curAmmo;
        this.maxMaga = maxBack;
        
        m_AmmoInspectorRef.SetcurMaga(maxMaga);
        m_AmmoInspectorRef.SetmaxAmmo(maxAmmo);
        m_AmmoInspectorRef.SetcurAmmo(curAmmo);
    }
    public void AmmoChange(bool isShooting)
    {

        if (isShooting)
        {
            curAmmo--;
            m_AmmoInspectorRef.SetcurAmmo(curAmmo);
        }
        else
        {
            curAmmo = maxAmmo;
            maxMaga--;
            
            m_AmmoInspectorRef.SetcurMaga(maxMaga);
            m_AmmoInspectorRef.SetcurAmmo(curAmmo);
        }
        
            
        
        
    }
    public void FireModeChange(Weapon.FireMode fireMode)
    {
        switch (fireMode)
        {
            case Weapon.FireMode.AUTO:
                m_FireMode.text = "A";
                break;
            case Weapon.FireMode.TRIPLE:
                m_FireMode.text = "T";
                break;
            case Weapon.FireMode.SINGLE:
                m_FireMode.text = "S";
                break;
        }
    }
    public void ModHP(float v)
    {
        m_HP.ModHP(v);
    }


    public void SetHP(float v)
    {
        m_HP.SetHP(v);
        
    }

    public void ModArmor(float v)
    {
        m_HP.ModArmor(v);
        
    }

    public void ResetArmor(float v)
    {
        m_HP.ResetArmor(v);
        
    }

    public void SetMax(float _maxHP, float _maxArmor)
    {
        m_HP.SetMax(_maxHP, _maxArmor);
    }
    //oooooof no
    public override void SendCommand(UICommand command)
    {
        BattleManager.WaveUIMsg pack = null;

        if (typeof(BattleManager.WaveUIMsg).IsEquivalentTo(command.GetType()))
        {
            pack = (BattleManager.WaveUIMsg)command;

            if (pack != null)
            {
                if (pack.g_EndType)
                {
                    if (!pack.g_IsLast)
                    {
                        m_UpText.text = "WAVE " + pack.g_Wave.ToString() + " END";
                        m_BottomText.text = roundEndText_Rare[Random.Range(0, roundEndText_Rare.Length)];
                    }
                    else

                    {
                        m_UpText.text = "FINAL WAVE END";
                        m_BottomText.text = roundEndText_Rare[Random.Range(0, roundEndText_Rare.Length)];
                    }

                }
                else
                {
                    if (pack.g_IsLast)
                    {
                        m_UpText.text = "FINAL WAVE";
                        m_BottomText.text = roundStartText_Rare[Random.Range(0, roundStartText_Rare.Length)];
                    }
                    else

                    {
                        m_UpText.text = "WAVE " + pack.g_Wave.ToString();
                        m_BottomText.text = roundStartText[Random.Range(0, roundStartText.Length)];
                    }
                }
                m_WaveBottom.SetTrigger("run");
                m_WaveUp.SetTrigger("run");
            }
        }
        else if(typeof(BattleManager.WaveUIMsg).IsEquivalentTo(command.GetType()))
        {
            var pack2 = (BattleManager.HitMsg)command;
            if (pack2 != null)
            {
                m_AimRef.Hit();
            }
        }
        
        
    }

    public void Update()
    {
        
    }

    public void AttachCamera(Camera camera)
    {
        UICanvasRoot.worldCamera = camera;
    }

    

    public static string[] roundStartText = new string[]
    {
        "Get Ready!",
        "Kill Kill Kill!",
        "Prepare to Die!",
        "Go Get 'Em!",
        "Take 'Em Out!",
        "Lock n' Load!",
        "You Got Work to Do!",
        "Here we go!",
        "Ruin Them",
        "Make em' bleed",
        "Go! Go! Go!",
        "Awww Yea!",
        "Make It Look Good",
        "They're Ready For You",
        "Don't Hold Back!",
        "Time to Step It Up!",
        "Oh, It's On Now!",
        "Give Them Hell!",
        "Unleash the Beast!",
        "Don't Miss!",
        "I'm on the edge of my seat!",
        "OH CRAP!",
        "Here we go!",
        "Rise n' Shine!",
        "It begins",
        "You know what to do",
        "Clobberin' Time!",
        "Time to get serious!",
        "Make them pay!",
        "Take it to them!",
        "Bring the Pain!",
        "Stomp their lights out!",
        "Go. Play. Have fun.",
        "Don't get too dirty!",
        "Put up yer dukes!",
        "Here they come!",
        "Hurry it up, why doncha?",
        "Good luck, sport",
        "Go for broke!",
        "Go for the gold!",
        "Never give up!",
        "Don't back down!",
        "You can't stop now!",
        "We're just getting started",
        "Let's get em' fellas!",
        "Better start running!",
        "Playtime's over",
        "There's no sense in trying",
        "Try hard!",
        "Quit now while you still can",
        "Party time!",
        "It's go time",
        "Maybe this will stop you",
        "This'll shut you up!",
        "Welcome to the Danger Zone!",
        "Now you're in for it!",
        "Here comes the hurt",
        "It never ends!",
        "The crowd is restless!",
        "We're hungry for death!",
        "Oh boy, oh boy!",
        "Show 'Em Who's Boss!",
        "Eye on the Prize!"
    };

    // Token: 0x04000B45 RID: 2885
    public static string[] roundStartText_Rare = new string[]
    {
        "Tomorrow brings new hope",
        "Oh, Hello",
        "I'll leave them some nice flowers",
        "Boop!",
        "Tee hee okay!",
        "You just don't get it, do ya?",
        "You're my special little one",
        "A gender-neutral congrats to you",
        "Ehhheheheh",
        "Gadzooks!",
        "Spank them rotten!",
        "Who said that?",
        "Prepare for ramming speed!",
        "Give them lemonade!",
        "Make some widows",
        "Go kick a baby",
        "Don't do drugs",
        "Kick the habit",
        "Git Gud",
        "The vote counts",
        "This is getting alarming!",
        "Oh SHEESH!!",
        "One two three four!",
        "There are ghosts in this stage",
        "You're a bad mama jama",
        "GOTCHA!",
        "Feed me bodies!",
        "Nice work...NOT",
        "Crap, crap, crap...!",
        "Have you been drinking?"
    };

    // Token: 0x04000B46 RID: 2886
    public static string[] roundEndText = new string[]
    {
        "You wrecked em!",
        "You're the boss!",
        "Way to go!",
        "NICE",
        "So slick",
        "Just right",
        "Annihilation!",
        "Too good",
        "Unstoppable!",
        "SMASHING",
        "Nice work",
        "That'll do",
        "Enemies Dispached",
        "Pretty confident!",
        "Passable performance",
        "Good job, pal",
        "A+ for effort",
        "Lookin' good!",
        "Hope you're proud of yourself",
        "Oh, sick!",
        "You nailed it",
        "Phenomenal!",
        "Look at the aftermath!",
        "Messed 'em up good",
        "Applause all around!",
        "How did you like that?",
        "And there they go",
        "Out for the count",
        "They're on the run!",
        "Didn't stand a chance",
        "No hope for badguys",
        "Take it easy, tiger!",
        "I'm so pumped!",
        "How'd you manage that?",
        "Who taught you to do that?",
        "Piece of cake",
        "Easy as pie",
        "Bursting right through!",
        "Need me to make it harder?",
        "You think that was easy?",
        "That looked like it tickled",
        "That shut them up!",
        "Where'd everyone go?",
        "Ain't gonna be so easy next time!",
        "Barely adequate",
        "Hopeless!",
        "Say uncle!",
        "Try harder",
        "He's on fire!",
        "Boom goes the dynamite!",
        "There's no do-overs",
        "Ready to quit?",
        "C'mon, seriously?",
        "Holy cow!",
        "Woah!",
        "Tubular!",
        "Superb!",
        "There's no stopping you!",
        "How was that?",
        "I can't believe it!",
        "How did you survive?!",
        "Boom Shakalaka!",
        "Done and done.",
        "The humanity!",
        "Cha-CHING!",
        "This ain't over!",
        "They're down for the count"
    };

    // Token: 0x04000B47 RID: 2887
    public static string[] roundEndText_Rare = new string[]
    {
        "Go wash your hands",
        "Filthy slattern!",
        "Docking your pay",
        "Is that a new haircut!!!",
        "Go home",
        "You're a cutie",
        "That was WEIRD",
        "There's no excuse for what I just saw",
        "Ewwww",
        "You think you're special?",
        "Oh honey that's precious",
        "Awwwww! Adorable!",
        "Fiddlesticks!",
        "Ya blew it",
        "Subscribe to learn more!",
        "Rotten scoundrel!",
        "Who do you think you are?",
        "Did someone say something?",
        "My children!!!",
        "Nooooooooooooooo!!",
        "Gosh!!",
        "Did...did we win?",
        "We so sorry",
        "We're sorry about that",
        "We'll try harder next time",
        "I think you're cheating",
        "I smell granny's brisket",
        "You get a golf clap",
        "Shiny star for you",
        "Putting your name on the board",
        "You take that back",
        "Toodles!",
        "Just another rerun",
        "D'you see that squirrel?",
        "Say \"excuse me\"",
        "Rude.",
        "...or is it?!",
        "I lost my glasses!",
        "Have you seen my glasses?",
        "I'm tickled pink",
        "Like and Subscribe"
    };
}
