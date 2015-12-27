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
        public static string uid = "root";
        public static string pwd = "8745928aa";
        private string connection_str;
        public DBHelper(string dbname)
        {
            string ConnStr = String.Format("server='localhost';" +
               "port='3306';" +
               "UId={0};" +
               "Password={1};", uid, pwd);
            MySqlConnection conn = new MySqlConnection(ConnStr);
            conn.Open();
            string sql = String.Format("select count(*) from information_schema.SCHEMATA WHERE SCHEMA_NAME='{0}'", dbname);
            MySqlCommand command = new MySqlCommand(sql, conn);
            int n = int.Parse(command.ExecuteScalar().ToString());
            conn.Close();
            connection_str = String.Format("server='localhost';" +
                "Database={2};" +
               "port='3306';" +
               "UId={0};" +
               "Password={1};", uid, pwd, dbname);
            if (n == 0)
            {
                sql = "create database if not exists " + dbname;
                conn.Open();
                command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                initDB();
            }
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
            SQLStr = "create table if not exists JudgeSchedule (" +
                "gameid int(10) not null, " +
                "judgeid int(5) not null, " +
                "primary key(gameid, judgeid)" +
                ")";
            exeSql(SQLStr);
            initData();
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

        private void initFunction()
        {

        }

        private void initView()
        {

        }

        private void initData()
        {
            //Records of playerdata
            string sql =
            "INSERT INTO `playerdata` VALUES ('1', '15', '0', '27', '3', '2', '7', '3', '0', '0', '0', '1', '2', '1', '0', '0', '1');" +
            "INSERT INTO `playerdata` VALUES ('1', '16', '1', '37', '6', '3', '8', '2', '0', '0', '4', '4', '0', '0', '0', '3', '3');" +
            "INSERT INTO `playerdata` VALUES ('1', '18', '0', '3', '1', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0');" +
            "INSERT INTO `playerdata` VALUES ('1', '21', '1', '27', '9', '3', '0', '0', '2', '2', '1', '1', '1', '5', '0', '1', '2');" +
            "INSERT INTO `playerdata` VALUES ('1', '22', '0', '17', '0', '0', '1', '0', '0', '0', '6', '4', '0', '0', '0', '1', '4');" +
            "INSERT INTO `playerdata` VALUES ('1', '23', '1', '25', '9', '5', '0', '0', '0', '0', '0', '0', '1', '6', '3', '0', '4');" +
            "INSERT INTO `playerdata` VALUES ('1', '24', '1', '29', '5', '2', '5', '1', '9', '8', '4', '3', '1', '4', '0', '2', '3');" +
            "INSERT INTO `playerdata` VALUES ('1', '25', '1', '31', '7', '5', '9', '4', '3', '3', '0', '6', '2', '5', '0', '3', '0');" +
            "INSERT INTO `playerdata` VALUES ('1', '27', '0', '23', '10', '3', '1', '0', '2', '1', '3', '2', '3', '7', '0', '1', '5');" +
            "INSERT INTO `playerdata` VALUES ('1', '29', '0', '23', '6', '2', '0', '0', '4', '3', '1', '0', '1', '1', '1', '0', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '1', '1', '36', '4', '2', '6', '3', '0', '0', '0', '4', '2', '3', '0', '3', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '2', '1', '29', '5', '2', '7', '3', '0', '0', '1', '1', '1', '3', '0', '2', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '3', '0', '19', '2', '0', '1', '0', '0', '0', '2', '3', '0', '1', '0', '1', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '4', '0', '21', '5', '4', '2', '1', '4', '1', '1', '2', '0', '4', '1', '0', '0');" +
            "INSERT INTO `playerdata` VALUES ('2', '7', '1', '26', '10', '3', '0', '0', '18', '10', '1', '1', '9', '6', '2', '0', '5');" +
            "INSERT INTO `playerdata` VALUES ('2', '8', '1', '36', '15', '5', '9', '2', '10', '9', '3', '6', '2', '4', '1', '1', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '9', '1', '28', '6', '4', '0', '0', '4', '2', '1', '1', '7', '5', '4', '2', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '10', '0', '19', '6', '2', '1', '1', '4', '1', '3', '2', '2', '2', '0', '0', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '11', '0', '6', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '12', '0', '2', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0');" +
            "INSERT INTO `playerdata` VALUES ('2', '13', '0', '16', '3', '0', '5', '2', '0', '0', '1', '1', '0', '2', '0', '0', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '14', '0', '2', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '0', '0', '0');" +
            "INSERT INTO `playerdata` VALUES ('2', '15', '0', '16', '1', '1', '0', '0', '6', '5', '1', '0', '0', '0', '0', '0', '1');" +
            "INSERT INTO `playerdata` VALUES ('2', '16', '0', '26', '5', '1', '4', '0', '0', '0', '4', '7', '0', '3', '0', '1', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '20', '1', '39', '9', '6', '5', '0', '0', '0', '2', '4', '2', '2', '0', '2', '3');" +
            "INSERT INTO `playerdata` VALUES ('2', '21', '1', '28', '8', '5', '0', '0', '2', '1', '1', '0', '2', '3', '1', '1', '5');" +
            "INSERT INTO `playerdata` VALUES ('2', '23', '1', '22', '7', '2', '0', '0', '0', '0', '0', '1', '0', '5', '1', '0', '6');" +
            "INSERT INTO `playerdata` VALUES ('2', '24', '1', '31', '4', '3', '5', '2', '3', '2', '2', '3', '0', '5', '1', '0', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '25', '1', '32', '10', '7', '6', '2', '7', '2', '2', '3', '1', '7', '1', '0', '2');" +
            "INSERT INTO `playerdata` VALUES ('2', '27', '0', '23', '14', '5', '1', '1', '6', '5', '1', '2', '3', '7', '0', '1', '5');" +
            "INSERT INTO `playerdata` VALUES ('2', '29', '0', '22', '4', '1', '0', '0', '0', '0', '1', '2', '3', '3', '1', '0', '1');";
           
            //Records of game
            sql += "INSERT INTO `game` VALUES ('1', '1', '2015-12-13', null, null);" +
                "INSERT INTO `game` VALUES ('2', '1', '2015-12-18', null, null);";

            //Records of gameschedule
            sql += "INSERT INTO `gameschedule` VALUES ('1', '1', '2');" +
                "INSERT INTO `gameschedule` VALUES ('2', '2', '1');";

            //Records of player
            sql += "INSERT INTO `player` VALUES ('1', '特雷沃-阿里扎', '3', '1985-06-20', '203', '98', '205', '85', null, null, null);" +
            "INSERT INTO `player` VALUES ('2', '帕特里克-贝弗利', '2', '1988-07-12', '185', '84', '185', '90', null, null, null);" +
            "INSERT INTO `player` VALUES ('3', '泰-劳森', '1', '1987-07-12', '180', '88', '182', '84', null, null, null);" +
            "INSERT INTO `player` VALUES ('4', '泰伦斯-琼斯', '4', '1992-01-09', '206', '114', '207', '83', null, null, null);" +
            "INSERT INTO `player` VALUES ('5', '萨姆-德克尔', '3', '1994-05-08', '206', '104', '206', '84', null, null, null);" +
            "INSERT INTO `player` VALUES ('6', '马库斯-索顿', '2', '1987-06-05', '193', '93', '195', '75', null, null, null);" +
            "INSERT INTO `player` VALUES ('7', '德怀特-霍华德', '5', '1985-12-08', '211', '120', '212', '93', null, null, null);" +
            "INSERT INTO `player` VALUES ('8', '詹姆斯-哈登', '2', '1989-08-26', '196', '100', '202', '89', null, null, null);" +
            "INSERT INTO `player` VALUES ('9', '克林特-卡培拉', '1', '1994-09-20', '208', '109', '210', '78', null, null, null);" +
            "INSERT INTO `player` VALUES ('10', '多纳塔斯-莫泰尤纳斯', '4', '1990-09-20', '213', '101', '213', '79', null, null, null);" +
            "INSERT INTO `player` VALUES ('11', '杰森-特里', '2', '1977-09-15', '188', '84', '189', '80', null, null, null);" +
            "INSERT INTO `player` VALUES ('12', '迈克-丹尼尔斯', '3', '1993-02-09', '198', '93', '200', '76', null, null, null);" +
            "INSERT INTO `player` VALUES ('13', '科里-布鲁尔', '3', '1986-03-05', '206', '84', '209', '79', null, null, null);" +
            "INSERT INTO `player` VALUES ('14', '蒙特雷斯-哈雷尔', '4', '1994-01-26', '203', '109', '204', '80', null, null, null);" +
            "INSERT INTO `player` VALUES ('15', '尼克-杨', '2', '1985-06-01', '201', '95', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('16', '德拉吉洛-拉塞尔', '1', '1996-02-23', '196', '88', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('17', '布兰顿-巴斯', '4', '1985-04-30', '203', '113', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('18', '安东尼-布朗', '3', '1992-10-10', '196', '95', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('19', '莱恩-凯莉', '4', '1991-04-09', '211', '104', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('20', '乔丹-卡拉克森', '1', '1992-10-10', '196', '84', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('21', '拉里-南斯', '4', '1993-01-01', '206', '104', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('22', '马塞洛-胡而塔斯', '1', '1983-05-25', '191', '91', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('23', '罗伊-希伯特', '5', '1986-12-11', '218', '122', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('24', '路易斯-威廉姆斯', '2', '1986-10-27', '185', '79', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('25', '科比-布莱恩特', '2', '1978-08-23', '198', '96', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('26', '塔勒克-布莱克', '5', '1991-11-22', '206', '113', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('27', '朱利叶斯-兰德尔', '4', '1994-11-13', '206', '113', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('28', '慈世平', '4', '1979-11-03', '201', '113', null, null, null, null, null);" +
            "INSERT INTO `player` VALUES ('29', '罗伯特-萨克雷', '5', '1989-06-06', '213', '122', null, null, null, null, null);";
           
            //Records of playerbelongs
            sql +="INSERT INTO `playerbelongs` VALUES ('1', '1', '2014-07-14', null, '1');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '2', '2013-01-07', null, '2');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '3', '2015-07-20', null, '3');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '4', '2013-01-06', null, '6');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '5', '2013-06-01', null, '7');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '6', '2013-07-08', null, '10');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '7', '2014-02-03', null, '12');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '8', '2013-06-07', null, '13');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '9', '2015-07-10', null, '15');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '10', '2013-05-01', null, '20');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '11', '2013-05-01', null, '31');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '12', '2013-05-01', null, '32');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '13', '2013-05-01', null, '33');"+
            "INSERT INTO `playerbelongs` VALUES ('1', '14', '2013-05-01', null, '35');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '15', '2013-05-01', null, '0');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '16', '2013-05-01', null, '1');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '17', '2013-05-01', null, '2');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '18', '2013-05-01', null, '3');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '19', '2013-05-01', null, '4');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '20', '2013-05-01', null, '6');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '21', '2013-05-01', null, '7');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '22', '2013-05-01', null, '9');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '23', '2013-05-01', null, '17');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '24', '2013-05-01', null, '23');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '25', '1996-07-08', null, '24');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '26', '2013-05-01', null, '28');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '27', '2013-05-01', null, '30');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '28', '2013-05-01', null, '37');"+
            "INSERT INTO `playerbelongs` VALUES ('2', '29', '2013-05-01', null, '50');";

            //record of team
            sql+="INSERT INTO `team` VALUES ('1', '火箭队', '美国', '休斯顿', '1967-01-01', null);"+
            "INSERT INTO `team` VALUES ('2', '湖人队', '美国', '洛杉矶', '1948-01-01', null);";

            exeSql(sql);
        }
    }
}
