using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Const 
{
    public static string IS_FIRST_TIME_OPEN_SCENE_HOME ="IS_FIRST_TIME_OPEN_SCENE_HOME"; 
    public static string IS_NEED_RESUME_GAME           = "IS_NEED_RESUME_GAME";
    public static string DATA_RESUME                   = "DATA_RESUME";
    public static string IS_PREMIUM                    = "IS_PREMIUM";
    public static string IS_COLLECTER_REWARD_PREMIUM   = "IS_COLLECTER_REWARD_PREMIUM";
    public static string LAST_DAY_LOGIN                = "LAST_DAY_LOGIN";
    public static string IS_BOUGHT_PREMIUM_YEAR        = "IS_BOUGHT_PREMIUM_YEAR";
    public static string IS_FINISH_TUTORIALONBOARD      = "IS_FINISH_TUTORIALONBOARD";
    public static string COUNT_TIME_STARTGAME           = "COUNT_TIME_STARTGAME";
}

public static class GameDataES3
{
    public static bool IS_FIRST_TIME_OPEN_SCENE_HOME
    {
        get
        {
           return  ES3.Load<bool>(Const.IS_FIRST_TIME_OPEN_SCENE_HOME, true);}
        set {  ES3.Save(Const.IS_FIRST_TIME_OPEN_SCENE_HOME, value);}
    }
    
    public static bool IS_NEED_RESUME_GAME
    {
        get
        {
            return  ES3.Load<bool>(Const.IS_NEED_RESUME_GAME, false);}
        set {  ES3.Save(Const.IS_NEED_RESUME_GAME, value);}
    }

    // public static DataResume DATA_RESUME
    // {
    //     get
    //     {
    //         return  ES3.Load<DataResume>(Const.DATA_RESUME, new DataResume());
    //     }
    //     set
    //     {
    //         ES3.Save<DataResume>(Const.DATA_RESUME, value);
    //     }
    // }
    
    public static bool IS_PREMIUM
    {
        get
        {
            return  ES3.Load<bool>(Const.IS_PREMIUM, false);}
        set {  ES3.Save(Const.IS_PREMIUM, value);}
    }
    public static bool IS_COLLECTER_REWARD_PREMIUM
    {
        get
        {
            return  ES3.Load<bool>(Const.IS_COLLECTER_REWARD_PREMIUM, false);
            
        }
        set {  ES3.Save(Const.IS_COLLECTER_REWARD_PREMIUM, value);}
    }
    
    public static string LAST_DAY_LOGIN
    {
        get
        {
            return  ES3.Load<string>(Const.LAST_DAY_LOGIN, defaultValue:string.Empty);}
        set {  ES3.Save(Const.LAST_DAY_LOGIN, value);}
    }
    
    public static bool IS_BOUGHT_PREMIUM_YEAR
    {
        get
        {
            return  ES3.Load<bool>(Const.IS_BOUGHT_PREMIUM_YEAR, false);
            
        }
        set {  ES3.Save(Const.IS_BOUGHT_PREMIUM_YEAR, value);}
    }

    public static bool IS_FINISH_TUTORIALONBOARD
    {
        get
        {
            return  ES3.Load<bool>(Const.IS_FINISH_TUTORIALONBOARD, false);
            
        }
        set {  ES3.Save(Const.IS_FINISH_TUTORIALONBOARD, value);}
        
    }
    public static int COUNT_TIME_STARTGAME
    {
        get
        {
            return  ES3.Load<int>(Const.COUNT_TIME_STARTGAME, 0);
            
        }
        set {  ES3.Save(Const.COUNT_TIME_STARTGAME, value);}
        
    }

}
    
    
    
    
