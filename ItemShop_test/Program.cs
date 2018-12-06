﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItemShop_test
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

namespace Nampospace
{
    //ゲームマスターがすべてのゲーム要素を管理する。
    class GameMaster
    {
        public int SelectedChara { private set; get; }

        public Charactar sengo, uruoi, nampo;
        public ItemShop itemshop;

        //パーティを作成。クラスにして、順序入れ替えたりできるほうがいい。
        public List<Charactar> MainParty;
        public int PartyGold; //パーティの資金。

        //意味ないターン
        int turn;

 
        //ゲームマスターが作られたときに初期化する。RPG作るの大変だなー
        public GameMaster()
        {
            //アイテム辞書を読み込む
            ItemDictionary itemdic = new ItemDictionary();

            turn = 0;//よくわからないターン数
        

            //キャラクターを適当に作る。特に表示とかはしてない。

            //戦後は弓３個と薬草10個持っている
            sengo = new Charactar("戦後");
            sengo.AddItem(itemdic.GetItem("弓",3));
            sengo.AddItem(itemdic.GetItem("薬草", 5));

            uruoi = new Charactar("うるおい");
            //うるおいは圧を3個持ってる
            uruoi.AddItem(itemdic.GetItem("圧", 3));
            uruoi.AddItem(itemdic.GetItem("薬草", 5));

            //うるおいちゃんだけHPが高い！今回は意味ない
            uruoi.MaxHp += 6;
            uruoi.Hp += 6;

            nampo = new Charactar("Nampo");
            //アイテム辞書からアイテムを追加、間違ったアイテム名を指定したときはテストアイテムがでる。
            nampo.AddItem(itemdic.GetItem("薬草！！",3));

            //お店を作成
            itemshop = new ItemShop("お店１");
            itemshop.AddItem(itemdic.GetItem("薬草", 10));
            itemshop.AddItem(itemdic.GetItem("弓", 5));
            itemshop.AddItem(itemdic.GetItem("矢", 99));
            itemshop.AddItem(itemdic.GetItem("剣", 5));

            //パーティ作成
            MainParty = new List<Charactar>();
            MainParty.Add(sengo);
            MainParty.Add(uruoi);
            MainParty.Add(nampo);

            //ここ微妙。もっといい書き方あると思う。パーティクラスにしたほうがよさそう。
            SelectedChara = 0;
            PartyGold = 103;
        }

        //未実装。適当に呼んでターンを進める的な？進めなかったらfalseとか？
        public bool NextTurn()
        {
            turn++;
            return true;
        }

        //未実装。ゲーム終了時にここを呼び出す。
        public void GameOver()
        {

        }

        //-----------以下表示関係、UIによって追加変更する。

        public void OnLoad(ComboBox comboBox)
        {
            //ロードしたらコンボボックスにキャラクタの名前いれる。
            comboBox.Items.Add(sengo.Name);
            comboBox.Items.Add(uruoi.Name);
            comboBox.Items.Add(nampo.Name);
        }

        public void ShowItem(TextBox textline, List<Item> items)
        {
            //表示する前に初期化
            textline.Text = "";

            foreach (Item i in items)
            {
                if (i != null)
                {
                    textline.Text += i.Name + ":" + i.Count + "個" + "\r\n";

                }
            }

        }
        public void ShowItem(TextBox textline, Charactar chara)
        {
            ShowItem(textline, chara.ItemList);
        }

        //お店のときはお店の倍率にあわせて表示を変える。
        //もっといい方法ありそうだけど、わからん。あとメソッドの名前変えるべきか。
        public void ShowItem(TextBox textline, Charactar chara, ItemShop shop)
        {
            //表示する前に初期化
            textline.Text = "";

            foreach (Item i in chara.ItemList)
            {
                if (i != null)
                {
                    textline.Text += i.Name + ":" + i.Count + "個   " + (int)(i.Value * shop.BuyScale) + " gold/個" + "\r\n";

                }
            }
        }
        public void ShowItem(TextBox textline, ItemShop shop)
        {
            //表示する前に初期化
            textline.Text = "";

            textline.Text += "お店の名前:\r\n「" + shop.Name + "」\r\n\r\n";
            foreach (Item i in shop.ItemList)
            {
                if (i != null)
                {
                    textline.Text += i.Name + ":" + i.Count + "個   " + (int)(i.Value * shop.SellScale) + " gold/個" + "\r\n";

                }
            }
        }

        //キャラクタのアイテムが見える！アイテムショップの情報を入れるといくらで売れるかわかる。
        //ここ微妙。消してもいいかも。パーティクラスにしていい感じに選択したい。
        public void CharaShowItem(TextBox textbox)
        {
            switch (SelectedChara)
            {
                case 0:
                    ShowItem(textbox, sengo);
                    break;
                case 1:
                    ShowItem(textbox, uruoi);
                    break;
                case 2:
                    ShowItem(textbox, nampo);
                    break;
                default: break;

            }

        }
        public void CharaShowItem(TextBox textbox, ItemShop shop)
        {
            switch (SelectedChara)
            {
                case 0:
                    ShowItem(textbox, sengo, shop);
                    break;
                case 1:
                    ShowItem(textbox, uruoi, shop);
                    break;
                case 2:
                    ShowItem(textbox, nampo, shop);
                    break;
                default: break;

            }
        }

        //キャラクタ選択する。何かのイベントで呼び出す。get,setでもいい。
        public void SelectChara(int select)
        {
            //変な数字が入らないようにする
            if (select >= 0 && select <= MainParty.Count)
            {
                SelectedChara = select;
            }

        }

        //キャラクタのステータス表示
        public void CharaShowStats(TextBox textbox,Charactar chara)
        {
            string temp = "";
            temp += "名前 :" + chara.Name + "\r\n";
            temp += "HP : " + chara.Hp +  "/" + chara.MaxHp + "\r\n";
            temp += "MP : " + chara.Mp + "/" + chara.MaxMp + "\r\n";
            temp += "攻撃 : " + chara.AttackPoint + "point\r\n";
            temp += "防御 : " + chara.DeffencePoint + "point\r\n";
            textbox.Text = temp;
        }
    }

    //アイテムショップ（キャラクタを継承している）
    class ItemShop : Charactar
    {
        //キャラクタクラスを継承してアイテムショップクラスを作成。
        //アイテムショップはキャラクタなので、実はHPなど設定されている！
        //キャラクタクラスを戦闘できるようにすると、必然的にアイテムショップとも戦闘できる！
        //良い継承かはわからん。店を拡張するなら店の中に店員がいてほしいよね。
        //とりあえず、継承の勉強のためこれにした。

        public int Gold { get; set; } //アイテムショップはお金を持っている。
        public double BuyScale { get; private set; } //アイテムショップが買いつけるときの倍率
        public double SellScale { get; private set; } //アイテムショップが売りつけるときの倍率

        public ItemShop(string name) : base(name)
        {
            Gold = 100;
            BuyScale = 0.5;　//買取は半額
            SellScale = 2; //売るときは倍

        }
        //お店はキャラクタに対してアイテムをいくつか売る。
        //売りたい個数を指定して売るが、持ち物にそれがなかったらfalseとする。
        //オーバーロードしたけど使わないんじゃね？←消した。
        public bool SellToChara(Item item, Charactar chara, ref int partygold)
        {
            //ショップのリストにアイテムがあるか？かつ、売りたい個数以上持っているか？かつ、パーティゴールドはあるか？

            if (this.ItemList.Exists(n => n.ItemID == item.ItemID) &&
                this.ItemList.Find(n => n.ItemID == item.ItemID).Count >= item.Count &&
                partygold >= (item.Value * SellScale * item.Count))
            {
                //自分のアイテムを個数分減らす。
                this.RemoveItem(item);

                //キャラクタのアイテムを増やす
                chara.AddItem(item);

                //アイテムの価値に倍率をかけた分お店のお金が増える小数点はないと思うが丸め込む。
                //四捨五入かは忘れた。調べて。
                Gold += ((int)(item.Value * SellScale) * item.Count);
                partygold -= ((int)(item.Value * SellScale) * item.Count);

                return true;

            }
            else
            {
                //売れねーよ
                return false;
            }

        }

        public bool BuyFromChara(Item item, Charactar chara, ref int partygold)
        {

            if (chara.ItemList.Exists(n => n.ItemID == item.ItemID) && 
                chara.ItemList.Find(n => n.ItemID == item.ItemID).Count >= item.Count && 
                Gold >= item.Value * BuyScale * item.Count)
            {
                //キャラのアイテムを減らす。
                chara.RemoveItem(item);

                //自分のアイテムを増やす
                this.AddItem(item);

                //アイテムの価値に倍率をかけた分お店のお金が増える小数点はないと思うが丸め込む。
                //四捨五入かは忘れた。調べて。
                Gold -= ((int)(item.Value * BuyScale) * item.Count);
                partygold += ((int)(item.Value * BuyScale) * item.Count);

                return true;

            }
            else
            {
                //買えねーよ
                return false;
            }
        }

    }

    //アイテム
    class Item
    {
        //itemIDは使ってない。
        public int ItemID { get; }
        public string Name { get; }
        //アイテム個数
        int count;
        //ここがプロパティ
        public int Count
        {
            set
            {
                //0より小さいものをいれようとしたら0にする。
                this.count = value < 0 ? 0 : value;
            }
            get
            {
                return this.count;
            }
        }

        //アイテムの価値・コスト
        public int Value { get; }

        //コンストラクタのオーバーロード
        //new Item(～)を好きにできるようにしておく。いらないやつもあるだろう。
        public Item(string name)
        {
            this.ItemID = 0;
            this.Name = name;
            this.count = 1;
            this.Value = 10;
        }
        public Item(string name, int count):this(name)
        {
            this.count = count;
            this.Value = 10;
        }
        public Item(string name, int count, int value):this(name,count)
        {
            this.Value = value;
        }
        
        public Item(int ID, string name, int count, int value) : this(name, count, value)
        {
            this.ItemID = ID;
            
        }
    }

    //キャラクタ
    class Charactar
    {
        public string Name { get; }
        public List<Item> ItemList { get; }//アイテムリストは見るだけ。
        public int MaxHp { set; get; }
        public int Hp { set; get; }
        public int MaxMp { set; get; }
        public int Mp { set; get; }

        public int AttackPoint { set; get; }
        public int DeffencePoint { set; get; }

        //名前だけ必ず決めるようにする。HPとかは適当に決めた。そのうち使うか？
        public Charactar(string name)
        {
            this.Name = name;
            ItemList = new List<Item>();
            MaxHp = 10;
            Hp = 10;
            MaxMp = 5;
            Mp = 5;
            AttackPoint = 2;
            DeffencePoint = 1;
        }

        //アイテムを持たせたり減らしたりする。持ち物制限とかはここら辺でやる。(10種類以上は持たせないみたいな)
        //オペレーターのオーバーライドですっきりにしたいけど、読んでないからとりあえずこれ。一応消せなかったらfalseが戻り値だけど、使ってない。
        public void AddItem(Item i)
        {
            

            if (ItemList.Exists(n => n.ItemID == i.ItemID))
            {
                //キャラのアイテムリストにアイテムがあれば個数分増やす。
                ItemList.Find(n => n.ItemID == i.ItemID).Count += i.Count;
            }
            else
            {
                //キャラのアイテムリストにアイテムがなければ、アイテムを増やす。
                ItemList.Add(i);
            }
        }
        public bool RemoveItem(Item i)
        {
            
            if (ItemList.Exists(n => n.ItemID == i.ItemID))
            {
                //キャラのアイテムリストにアイテムがあれば個数分減らす。
                //なお、countは0以下にならないので減らすだけなら問題ない。売るときは問題になる。
                ItemList.Find(n => n.ItemID == i.ItemID).Count -= i.Count;

                //個数が0なら消す
                CheckItemlist();

                return true;

            }
            else
            {
                //キャラのアイテムにアイテムがなければ何もしない。
                return false;
            }
        }

        //アイテムを指定して個数を増やしたり、減らしたりする。
        //メソッド名はオーバーロード
        public void AddItem(Item i, int count)
        {
            i.Count = count;
            AddItem(i);
        }
        public bool RemoveItem(Item i, int count)
        {
            i.Count = count;
            return RemoveItem(i);

        }

        //アイテムリストのアイテム個数0だった場合は削除する,もっといい書き方あるはず
        protected void CheckItemlist()
        {
            var temps = new List<int>();
            int tempindex = 0;

            foreach (Item items in ItemList)
            {
                
                if (items.Count <= 0)
                {
                    //ItemList.Remove(name);
                    temps.Add(tempindex);
                }
                tempindex++;
            }

            foreach (int index in temps)
            {
                ItemList.RemoveAt(index);
            }
        }

    }

    //アイテム辞書。アイテムの設定などはここで最初に定義しておく。
    class ItemDictionary
    {
        static int ItemID;
        Dictionary<string, Item> ItemDic;

        public ItemDictionary()
        {
            ItemDic = new Dictionary<string, Item>();
            ItemID = 0;

            ItemAdd(new Item(ItemID,"テストアイテム", 1, 1));
            ItemAdd(new Item(ItemID, "薬草", 1, 10));
            ItemAdd(new Item(ItemID, "毒消し草", 1, 15));
            ItemAdd(new Item(ItemID, "剣", 1, 30));
            ItemAdd(new Item(ItemID, "盾", 1, 10));
            ItemAdd(new Item(ItemID, "圧", 1, 50));
            ItemAdd(new Item(ItemID, "弓", 1, 20));
            ItemAdd(new Item(ItemID, "矢", 1, 1));

        }
        //このメソッドでアイテムIDを順番につけてもらう。
        //キャラクタのアイテムはList<Item>で管理して、ソートはID順にする？
        private void ItemAdd(Item item)
        {
            ItemDic.Add(item.Name, new Item(ItemID,item.Name,1,item.Value));
            ItemID++;
        }

        public Item GetItem(string name)
        {
            if (ItemDic.ContainsKey(name))
            {
                return new Item(ItemDic[name].ItemID,ItemDic[name].Name, 1, ItemDic[name].Value);
            }
            else
            {
                return new Item(0,ItemDic["テストアイテム"].Name, 1, ItemDic["テストアイテム"].Value);
            }
        }

        public Item GetItem(string name,int count)
        {
            if (ItemDic.ContainsKey(name))
            {
                return new Item(ItemDic[name].ItemID,ItemDic[name].Name, count, ItemDic[name].Value);
            }
            else
            {
                return new Item(ItemDic["テストアイテム"].Name, count, ItemDic["テストアイテム"].Value);
            }
        }
    }

}