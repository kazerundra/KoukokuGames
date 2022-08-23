using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator character;
    public CharacterMovement enemy;
    public Transform movePos;
    public Tower currentTower;
    public float speed = 5f;
    public bool animOver=true;
    public CharStat charStat;
    public void JumpTo(Transform targetPos)
    {
        character.SetTrigger("jump");
        movePos = targetPos;
    }
    public bool GetDead()
    {
        return character.GetBool("dead");
    }
      

     void StartVerticalMove()
    {
        character.SetBool("vertMove",true);
    }
    void LandingOver()
    {
        character.SetBool("isLanding", false);
        animOver = true;
    }

    public void MoveHorizontal()
    {
        character.SetBool("walking", true);
    }

    public void Attack()
    {
        animOver = false;
        character.SetTrigger("attack");
    }

    public bool WinBattle()
    {
        if(GetComponent<CharStat>().power > enemy.GetComponent<CharStat>().power)
        {
            Attack();
            return true;
        }
        else
        {
            enemy.Attack();
            return false;
        }
    }
    
    void AnimOver()
    {
        animOver = true;
        
    }
    public void TakeDamage()
    {
        character.SetBool("dead", true);
    }

    void EnemyTakeDamage()
    {
        enemy.TakeDamage();
    }
    // Start is called before the first frame update
    void Awake()
    {
        character = GetComponent<Animator>();
        charStat = GetComponent<CharStat>();
    }

    public void FlipChar()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        Transform charStat = GetComponent<CharStat>().powerText.transform;
        charStat.transform.localScale = new Vector3(-1, 1, 1);
    }


    public void AssignTower(Tower tower, CharacterMovement player)
    {
        currentTower = tower;
        enemy = tower.enemy;
        enemy.enemy = player;
    }
    // Update is called once per frame
    void Update()
    {
        if (character.GetBool("vertMove"))
        {
            var step = speed * Time.deltaTime;
            Debug.Log(movePos);
            transform.position = Vector3.MoveTowards(transform.position, movePos.position, step);
            if (Vector3.Distance(transform.position, movePos.position) < 0.001f)
            {
                
                character.SetBool("vertMove", false);
                character.SetBool("isLanding", true);
            }
        }

        if (character.GetBool("walking"))
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movePos.position, step);
            if (Vector3.Distance(transform.position, movePos.position) < 0.001f)
            {
                character.SetBool("walking", false);
            }
        }
    }
}
