using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InitDataBase
{
    class Program
    {
        public static string ConnStr =
               "Database='nbadb';" +
               "server='localhost';" +
               "port='3306';" +
               "UId='root';" +
               "Password='root';";
        static void Main(string[] args)
        {
            createDB();
            MySqlConnection conn = new MySqlConnection(ConnStr);
            conn.Open();
            string SQLStr;
            SQLStr = "create table if not exists Square ("+
                "name varchar(100) not null, "+
                "city varchar(100), " +
                "capacity int(10), "+
                "setupdate date, "+
                "picture varchar(300), " +
                "primary key(name)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists Uniform ("+
                "uniformid int(5) not null, "+
                "color int(1) not null, "+
                "sponsor varchar(100), "+
                "primary key(uniformid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists MainJudge ("+
                "judgeid int(5) not null, "+
                "name varchar(50) not null, "+
                "gender int(1) not null, "+
                "birthday date, "+
                "picture varchar(300), "+
                "primary key(judgeid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists MainCoach ("+
                "coachid int(5) not null, "+
                "name varchar(50) not null, "+
                "gender int(1) not null, "+
                "birthday date, "+
                "picture varchar(300), "+
                "primary key(coachid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists Team ("+
                "teamid int(5) not null, "+
                "name varchar(50) not null, "+
                "country varchar(50), "+
                "city varchar(50), "+
                "founddate date, "+
                "disbanddate date, "+
                "primary key(teamid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists Player ("+
                "playerid int(5) not null, "+
                "name varchar(50) not null, "+
                "position int(1) not null, "+
                "birthday date, "+
                "height int(3), "+
                "weight int(3), "+
                "armspan int(3), "+
                "jump int(3), "+
                "picture varchar(300), "+
                "careerbegin date, "+
                "careerend date, "+
                "primary key(playerid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists Game ("+
                "gameid int(10) not null, "+
                "type int(1) not null, "+
                "date date, "+
                "gamecondition int(1), "+
                "progress int(1), "+
                "primary key(gameid)" +
                ")";
            createTable(SQLStr, conn);


            SQLStr = "create table if not exists Coaching (" +
                "teamid int(5) not null, " +
                "coachid int(5) not null, " +
                "starttime date not null, " +
                "endtime date, " +
                "primary key(teamid, coachid, starttime)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists UniformBelongs (" +
                "uniformid int(5) not null, " +
                "teamid int(5) not null, " +
                "starttime date not null, "+
                "endtime date, "+
                "primary key(uniformid, teamid, starttime)"+
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists UsedUniform (" +
                "gameid int(10) not null, " +
                "teamid int(5) not null, " +
                "uniformid int(5) not null, " +
                "primary key(gameid, teamid, uniformid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists UsedSquare (" +
                "teamid int(5) not null, " +
                "squarename varchar(100), " +
                "primary key(teamid, squarename)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists PlayerBelongs (" +
                "teamid int(5) not null, " +
                "playerid int(5) not null, " +
                "starttime date not null, " +
                "endtime date, " +
                "number int(5) not null, " +
                "primary key(teamid, playerid, starttime)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists PlayerData ("+
                "gameid int(10) not null, "+
                "playerid int(5) not null, "+
                "first int(1), "+
                "playtime int(3), "+
                "2pnum int(3), "+
                "2phits int(3), "+
                "3pnum int(3), "+
                "3phits int(3), "+
                "penaltyNum int(3), "+
                "penaltyHits int(3), "+
                "turnover int(3), "+
                "assist int(3), "+
                "frontreb int(3), "+
                "backreb int(3), "+
                "blockshot int(3), "+
                "steal int(3), "+
                "foul int(3), " +
                "primary key(gameid, playerid)"+
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists GameSchedule ("+
                "gameid int(10) not null, "+
                "hometeamid int(5) not null, "+
                "visitingteamid int(5) not null, "+
                "primary key(gameid, hometeamid, visitingteamid)" +
                ")";
            createTable(SQLStr, conn);
            SQLStr = "create table if not exists JuegeSchedule (" +
                "gameid int(10) not null, " +
                "judgeid int(5) not null, " +
                "primary key(gameid, judgeid)" +
                ")";
            createTable(SQLStr, conn);
            conn.Close();

            return;
        }

        static void createTable(string SQLStr, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand(SQLStr, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        static void createDB() {
            string connstr = "data source='localhost';" +
                "port='3306';" +
                "user id='root';" +
                "password='root';";
            string s = "create database if not exists nbadb";
            MySqlConnection conn = new MySqlConnection(connstr);
            MySqlCommand cmd = new MySqlCommand(s, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }
    }
}
