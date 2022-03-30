using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ChatSide { Side1, Side2 }

public class Chat : MonoBehaviour
{
    [SerializeField] private GameObject scrollVeiwContent;
    [SerializeField] private Transform hero1Pos;
    [SerializeField] private Transform hero2Pos;
    private GameObject instHero1;
    private GameObject instHero2;
    [SerializeField] private Transform[] buttonPositions;
    [SerializeField] private GameObject chatBubblePrefab;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Text factionName;
    private List<GameObject> conversations;
    private List<GameObject> options;

    public void Initailize(GameObject hero1, GameObject hero2, string faction = "")
    {
        instHero1 = Instantiate(hero1, hero1Pos);
        instHero1.transform.localScale = new Vector3(20f, 20f, 0f);
        instHero1.transform.localPosition = Vector3.zero;

        instHero2 = Instantiate(hero2, hero2Pos);
        instHero2.transform.localScale = new Vector3(20f, 20f, 0f);
        instHero2.transform.localPosition = Vector3.zero;

        factionName.text = faction;
    }

    public void MakeConversation(ChatSide side, string convoText)
    {
        GameObject conversation = Instantiate(chatBubblePrefab, scrollVeiwContent.transform);
        conversations.Add(conversation);

        Text chatBubbleText = conversation.GetComponentInChildren<Text>();

        chatBubbleText.alignment = side == ChatSide.Side1 ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;

        chatBubbleText.text = convoText;
    }


    public void AddOptionAndConversation(string optionText, int buttonPostion, string convoTextSide1, string convoTextSide2, Action nextStep)
    {
        GameObject option = Instantiate(buttonPrefab, buttonPositions[buttonPostion].transform);
        options.Add(option);

        Text optionTextComponent = option.GetComponentInChildren<Text>();
        optionTextComponent.text = optionText;

        option.GetComponent<Button>().onClick.AddListener(() =>
        {
            MakeConversation(ChatSide.Side1, convoTextSide1);
            MakeConversation(ChatSide.Side2, convoTextSide2);
            nextStep();
        });
    }


}

public class test
{
    [SerializeField] Chat chat;
    GameObject hero1, hero2;

    public void Start()
    {
        chat.Initailize(hero1, hero2, "Fara Empire");

        chat.MakeConversation(ChatSide.Side1, "Had an enjoyable ride with you!");

        chat.MakeConversation(ChatSide.Side2, "It was nice having you on board");

        if(UnityEngine.Random.Range(0,100) < 10)
        {
            chat.AddOptionAndConversation("wanna join?", 0, "We would be happy if you joined us", "I would be happy to join you", AnswerJoinUs);
        }
        else
        {
            chat.AddOptionAndConversation("wanna join?", 0, "We would be happy if you joined us", "no sorry I have other things to do.", AnswerGoodbye);
        }
    }

    public void AnswerJoinUs()
    {

    }

    public void AnswerGoodbye()
    {

    }
}
