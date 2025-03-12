using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { Init, Playing, Dead}
public enum Carril { Left, Center, Right };
public class GameManager : MonoBehaviour
{
    GameState state;
    public List<GameObject> allPaths;
    public int firstPaths;
    public List<int> firstIndexEnabled;

    public CharacterController player;
    Vector3 moveDir;
    public float speed, speedHorizontal, gravity, jumpForce;
    public int score;
    float timer;
    public int lifes;
    int maxLifes;

    Transform lastPath, currentPath, parentCarril, childCarril;
    public Carril currentCarril;
    [SerializeField] List<GameObject> paths = new List<GameObject>();
    bool turnActive, pathT, isT;
    Transform newPaths, newEnds;
    
    public GameObject InitText;
    public Animator anim;
    public Text scoreText;
    public Image lifeBar;

    public bool isInvincible = false;
    public float invincibilityDuration = 2.0f; // Duración de la invencibilidad en segundos
    float invincibilityTimer; 


    void Start()
    {
        state = GameState.Init;
        maxLifes = lifes;
        lastPath = transform;
        FirstGenerate();
    }

    void FirstGenerate()
    {
        for (int i = 0; i < firstPaths; i++)
        {
            GameObject newPath = Instantiate(allPaths[GetRandomIndexPath()], lastPath.position, lastPath.rotation);
            newPath.AddComponent<PathControl>().Init(this);
            lastPath = newPath.transform.Find("Ends").GetChild(0);
            paths.Add(newPath);
        }
        currentPath = paths[0].transform;
        currentCarril = Carril.Center;

        parentCarril = currentPath.Find("Paths").GetChild(0);
        childCarril = parentCarril.GetChild((int)currentCarril);

        player.transform.SetParent(parentCarril);
        player.transform.forward = parentCarril.forward;
    }
    int GetRandomIndexPath()
    {
        int randomIndex = Random.Range(0, allPaths.Count);
        while(firstIndexEnabled.Contains(randomIndex) == false)
        {
            randomIndex = Random.Range(0, allPaths.Count);
        }
        return randomIndex;
    }
    public void GetRandomNewPath(GameObject oldPath)
    {
        speed += 1.5f; //sube la velocidad por momentos
        
        paths.Remove(oldPath);
        Destroy(oldPath);

        if(pathT == false)
        {
            int randomIndex = Random.Range(0, allPaths.Count);
            GameObject newPath = Instantiate(allPaths[randomIndex], lastPath.position, lastPath.rotation);
            newPath.AddComponent<PathControl>().Init(this);

            lastPath = newPath.transform.Find("Ends").GetChild(0);
            paths.Add(newPath);

            if(randomIndex == 1) pathT = true;
        }
        currentPath = paths[0].transform;
        parentCarril = currentPath.Find("Paths").GetChild(0);

        player.transform.SetParent(parentCarril);
        player.transform.forward = parentCarril.forward;
    }
    void SetNewPathT()
    {
        pathT = false;
        isT = false;
        lastPath = newEnds;

        for (int i = 0; i < (firstPaths -1); i++)
        {
            GameObject newPath = Instantiate(allPaths[GetRandomIndexPath()], lastPath.position, lastPath.rotation);
            newPath.AddComponent<PathControl>().Init(this);
            lastPath = newPath.transform.Find("Ends").GetChild(0);
            paths.Add(newPath);
        }
    }
    
    void Update()
    {
        switch (state)
        {
            case GameState.Init:
                if(Input.GetKeyDown(KeyCode.W))
                {
                    InitText.SetActive(false);
                    //Desactivar txto inicio pulsa w para comenzar
                    state = GameState.Playing;
                }
                break;
            case GameState.Playing:
                timer += Time.deltaTime;
                if(timer > 1)
                {
                    score++;
                    scoreText.text = score.ToString();
                    timer = 0;
                }

                moveDir = new Vector3(0, moveDir.y, speed);
                moveDir = player.transform.TransformDirection(moveDir);

                if (player.isGrounded)
                {
                    if (Input.GetButton("Jump"))
                    {
                        moveDir.y = jumpForce;
                    }
                }
                else
                {
                    moveDir.y -= gravity * Time.deltaTime;
                }

                if (Input.GetButtonDown("Horizontal"))
                {
                    int indexButton = (int)Input.GetAxisRaw("Horizontal");
                    switch (currentCarril)
                    {
                        case Carril.Left:
                            if (indexButton > 0) currentCarril = Carril.Center;
                            if (indexButton < 0 && turnActive)
                            {
                                parentCarril = newPaths;
                                childCarril = parentCarril.GetChild((int)currentCarril);

                                player.transform.SetParent(parentCarril);
                                player.transform.forward = parentCarril.forward;
                                if (isT) SetNewPathT();
                            }
                            break;
                        case Carril.Center:
                            if (indexButton < 0) currentCarril = Carril.Left;
                            else if (indexButton > 0) currentCarril = Carril.Right;
                            break;
                        case Carril.Right:
                            if (indexButton < 0) currentCarril = Carril.Center;
                            if (indexButton > 0 && turnActive)
                            {
                                parentCarril = newPaths;
                                childCarril = parentCarril.GetChild((int)currentCarril);

                                player.transform.SetParent(parentCarril);
                                player.transform.forward = parentCarril.forward;
                                if (isT) SetNewPathT();
                            }
                            break;
                    }
                    childCarril = parentCarril.GetChild((int)currentCarril);
                }
                if(player.transform.position.y < 0)
                {
                    SetDeadPlayer();
                }
                if (isInvincible)
                {
                    invincibilityTimer -= Time.deltaTime;
                    if (invincibilityTimer <= 0)
                    {
                        isInvincible = false;
                    }
                }

                break;
            case GameState.Dead:
                moveDir *= 0;
                break;
        }

        
        
    }
    private void FixedUpdate()
    {
        player.Move(moveDir * Time.deltaTime);

        if(childCarril != null)
        {
            Vector3 finalPosition = player.transform.localPosition;
            finalPosition.x = childCarril.localPosition.x;
            player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition, finalPosition, speedHorizontal * Time.deltaTime);
        }
    }
    public void GetScore(int value){
        score += value;
        scoreText.text = score.ToString();
    }
    public void GetDamage()
{
    if (!isInvincible)
    {
        anim.GetComponent<DamageControl>().ActiveDamage();
        lifes--;
        lifeBar.fillAmount = (float)lifes / (float)maxLifes;
        if (lifes <= 0) SetDeadPlayer();
    }
}
    void SetDeadPlayer()
    {
        GetComponent<DataManager>().GameOver(score);
        state = GameState.Dead;
    }
    public void ActiveTurn(bool active, Transform newPath)
    {
        turnActive = active;
        newPaths = newPath;
    }

    public void ActiveTurn(bool active, Transform newPath, Transform newEnd)
    {
        turnActive = active;
        newPaths = newPath;
        isT = active;
        newEnds = newEnd;
    }
    public void ActivateInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        anim.GetComponent<InvControl>().ActiveInv();
        // Aquí puedes añadir efectos visuales o sonoros para indicar que el jugador es invencible temporalmente
    }
}
