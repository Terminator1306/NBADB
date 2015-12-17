using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfApplication1
{
    class DBHelper
    {
        private string connection_str;
        public DBHelper(string dbname,string id ,string pwd)
        {
            string ConnStr = String.Format("server='localhost';" +
               "port='3306';" +
               "UId={0};" +
               "Password={1};", id, pwd);
            MySqlConnection conn = new MySqlConnection(ConnStr);
            conn.Open();
            string sql = String.Format("select count(*) from information_schema.SCHEMATA WHERE SCHEMA_NAME='{0}'", dbname);
            MySqlCommand command = new MySqlCommand(sql, conn);
            int n = int.Parse(command.ExecuteScalar().ToString());
            if(n==0)
            {
                initDB();
            }
            conn.Close();
            connection_str=String.Format("server='localhost';" +
                "Database={2}"+
               "port='3306';" +
               "UId={0};" +
               "Password={1};", id, pwd,dbname);
        }

        private void initDB()
        {
            string SQLStr;
            SQLStr = "create table if not exists Square (" +
                "name varchar(100) not null, " +
                "city varchar(100), " +
                "capacity int(10), " +
                "setupdate date, " +
                "picture varchar(300), " +
                "primary key(name)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists Uniform (" +
                "uniformid int(5) not null, " +
                "color int(1) not null, " +
                "sponsor varchar(100), " +
                "primary key(uniformid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists MainJudge (" +
                "judgeid int(5) not null, " +
                "name varchar(50) not null, " +
                "gender int(1) not null, " +
                "birthday date, " +
                "picture varchar(300), " +
                "primary key(judgeid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists MainCoach (" +
                "coachid int(5) not null, " +
                "name varchar(50) not null, " +
                "gender int(1) not null, " +
                "birthday date, " +
                "picture varchar(300), " +
                "primary key(coachid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists Team (" +
                "teamid int(5) not null, " +
                "name varchar(50) not null, " +
                "country varchar(50), " +
                "city varchar(50), " +
                "founddate date, " +
                "disbanddate date, " +
                "primary key(teamid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists Player (" +
                "playerid int(5) not null, " +
                "name varchar(50) not null, " +
                "position int(1) not null, " +
                "birthday date, " +
                "height int(3), " +
                "weight int(3), " +
                "armspan int(3), " +
                "jump int(3), " +
                "picture varchar(300), " +
                "careerbegin date, " +
                "careerend date, " +
                "primary key(playerid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists Game (" +
                "gameid int(10) not null, " +
                "type int(1) not null, " +
                "date date, " +
                "gamecondition int(1), " +
                "progress int(1), " +
                "primary key(gameid)" +
                ")";
            exeSql(SQLStr);


            SQLStr = "create table if not exists Coaching (" +
                "teamid int(5) not null, " +
                "coachid int(5) not null, " +
                "starttime date not null, " +
                "endtime date, " +
                "primary key(teamid, coachid, starttime)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists UniformBelongs (" +
                "uniformid int(5) not null, " +
                "teamid int(5) not null, " +
                "starttime date not null, " +
                "endtime date, " +
                "primary key(uniformid, teamid, starttime)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists UsedUniform (" +
                "gameid int(10) not null, " +
                "teamid int(5) not null, " +
                "uniformid int(5) not null, " +
                "primary key(gameid, teamid, uniformid)" +
                ")";
            exeSql(SQLStr); 
            SQLStr = "create table if not exists UsedSquare (" +
                "teamid int(5) not null, " +
                "squarename varchar(100), " +
                "primary key(teamid, squarename)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists PlayerBelongs (" +
                "teamid int(5) not null, " +
                "playerid int(5) not null, " +
                "starttime date not null, " +
                "endtime date, " +
                "number int(5) not null, " +
                "primary key(teamid, playerid, starttime)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists PlayerData (" +
                "gameid int(10) not null, " +
                "playerid int(5) not null, " +
                "first int(1), " +
                "playtime int(3), " +
                "2pnum int(3), " +
                "2phits int(3), " +
                "3pnum int(3), " +
                "3phits int(3), " +
                "penaltyNum int(3), " +
                "penaltyHits int(3), " +
                "turnover int(3), " +
                "assist int(3), " +
                "frontreb int(3), " +
                "backreb int(3), " +
                "blockshot int(3), " +
                "steal int(3), " +
                "foul int(3), " +
                "primary key(gameid, playerid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists GameSchedule (" +
                "gameid int(10) not null, " +
                "hometeamid int(5) not null, " +
                "visitingteamid int(5) not null, " +
                "primary key(gameid, hometeamid, visitingteamid)" +
                ")";
            exeSql(SQLStr);
            SQLStr = "create table if not exists JuegeSchedule (" +
                "gameid int(10) not null, " +
                "judgeid int(5) not null, " +
                "primary key(gameid, judgeid)" +
                ")";
            exeSql(SQLStr);
        }

        public void exeSql(string sql)
        {
            MySqlConnection con = new MySqlConnection(connection_str);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public MySqlConnection getCon()
        {
            return new MySqlConnection(connection_str);
        }
    }
}
