using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class CardBook : MonoBehaviour
{

    //Reader
    private string Path { get; set; } //path of full database xml
    private List<Card> CardList { get; set; } //full list of all cards from XML
    private List<List<Card>> CardPages { get; set; } //book of cards organized by pages of ten
    private int CurrentPage { get; set; } //keeps track of location in book
    private const int CARDS_PER_PAGE = 10; //Number of cards per page, must correspond with displayed slots in scene 
    private int NumPages;
    private bool Compiled { get; set; }

    CardBook (String path)
    {
        LoadDatabaseFromFile(path);
    }

    void Start()
	{
        Compiled = false;
        //load all cards in database by path with reader
    }

    public void LoadDatabaseFromFile(String path)
    {
        Path = path;
        DeckReader reader = new DeckReader();
        if (File.Exists(Path))
        {
            List<Card> CardList = reader.load(Path);
        }
        else
        {
            Debug.Log("Error: File not Found");
        }
        CompileBook();
    }

    public void CompileBook()
    {
        NumPages = CardList.Count;
        if (CardList.Count % 10 != 0) //If there are cards remaining, add another page
        {
            NumPages++;
        }
        for (int Page = 0; Page < NumPages - 1; Page++)
            {
            for (int i = 0; i < CARDS_PER_PAGE; i++)
            {
                if (CardList[i + (10 * Page)] != null)
                {
                    CardPages[Page][i] = CardList[i + (10 * Page)];
                }
            }
            }
        Compiled = true;
    }

    public List<Card> GetCardsOfPage(int Page) //returns 10 cards for display later from passed page
    {
            return CardPages[Page];
    }

    public Card GetCardByPageElement(int Page, int Element) //returns 10 cards for display later from passed page
    {
        List<Card> PageArr = GetCardsOfPage(Page);
        return PageArr[Element];

    }

    public void clearPage(int Page)
    {
            CardPages[Page].Clear();
    }

    public void clearCardByPageElement(int Page, int Element)
    {
        CardPages[Page].Clear();
    }




}

