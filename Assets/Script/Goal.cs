using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public Text _GoalText; // ゴール時に表示するテキスト

    void Start()
    {
        _GoalText.gameObject.SetActive(false); // 最初は非表示にする
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // プレイヤーがゴールに触れたら
        {
            _GoalText.gameObject.SetActive(true); // テキストを表示
        }
    }
}
