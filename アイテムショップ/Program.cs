using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace アイテムショップ
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
        //バグ仕込む用//ここが更新点
        Item leaf;
               
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
            MainParty = new List<Charactar>();

            SelectedChara = 0;

            turn = 0;//よくわからないターン数

            //薬草5個！バグ用
            leaf = new Item("薬草", 5);

            //キャラクターを適当に作る。特に表示とかはしてない。

            //戦後は弓３個と薬草10個持っている
            sengo = new Charactar("戦後");
            sengo.AddItem(new Item("弓", 3));
            //＜＜バグの元＞＞。薬草を5個持たせているようにみえる。***********************実はうるおいちゃんの薬草と共有されてる。
            sengo.AddItem(leaf);

            uruoi = new Charactar("うるおい");
            //＜＜バグの元＞＞。薬草を5個持たせているようにみえる。************************実はうるおいちゃんの戦後と共有されてる。
            uruoi.AddItem(leaf);
            //うるおいは圧を3個持ってる
            uruoi.AddItem(new Item("圧", 3));

            //うるおいちゃんだけHPが高い！今回は意味ない
            uruoi.MaxHp += 6;
            uruoi.Hp += 6;

            nampo = new Charactar("Nampo");
            //Nampoは薬草を3個持ってる
            nampo.AddItem(new Item("薬草", 3));

            //お店を作成
            itemshop = new ItemShop("お店１");
            itemshop.AddItem(new Item("薬草",5));
            itemshop.AddItem(new Item("弓", 5));
            itemshop.AddItem(new Item("矢", 99));
            itemshop.AddItem(new Item("カイトシールド", 10));

            //パーティ作成
            MainParty.Add(sengo);
            MainParty.Add(uruoi);
            MainParty.Add(nampo);

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

        public void ShowItem(TextBox textline, Dictionary<string, Item> items)
        {
            //表示する前に初期化
            textline.Text = "";

            foreach (Item i in items.Values)
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
        public void ShowItem(TextBox textline, Charactar chara,ItemShop shop)
        {
            //表示する前に初期化
            textline.Text = "";

            foreach (Item i in chara.ItemList.Values)
            {
                if (i != null)
                {
                    textline.Text += i.Name + ":" + i.Count + "個   " + i.Value * shop.BuyScale + " gold/個" + "\r\n";

                }
            }
        }
        public void ShowItem(TextBox textline, ItemShop shop)
        {
            //表示する前に初期化
            textline.Text = "";

            textline.Text += "お店の名前:\r\n「" + shop.Name + "」\r\n\r\n";
            foreach (Item i in shop.ItemList.Values)
            {
                if (i != null)
                {
                    textline.Text += i.Name + ":" + i.Count + "個   " + i.Value * shop.SellScale + " gold/個" + "\r\n";

                }
            }
        }
  
        //キャラクタのアイテムが見える！アイテムショップの情報を入れるといくらで売れるかわかる。
        //ここ微妙。消してもいいかも。
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
        public void CharaShowItem(TextBox textbox,ItemShop shop)
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

    }

    class ItemShop : Charactar
    {
        //キャラクタークラスを継承してアイテムショップクラスを作成。
        //アイテムショップはキャラクターなので、実はHPなど設定されている！
        //キャラクタークラスを戦闘できるようにすると、必然的にアイテムショップとも戦闘できる！
        //良い継承かはわからん。店を拡張するなら店の中に店員がいてほしいよね。
        //とりあえず、継承のためこれにした。

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
        //オーバーロードしたけど使わないんじゃね？
        public bool SellToChara(Item item , Charactar chara,ref int partygold)
        {
            //ショップのリストにアイテムがあるか？かつ、売りたい個数以上持っているか？かつ、パーティゴールドはあるか？
            if (this.ItemList.ContainsKey(item.Name) && this.ItemList[item.Name].Count >= item.Count && partygold >= (item.Value * SellScale * item.Count))
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

        public bool BuyFromChara(Item item, Charactar chara,ref int partygold)
        {
            if (chara.ItemList.ContainsKey(item.Name) && chara.ItemList[item.Name].Count >= item.Count && Gold >= item.Value * BuyScale *item.Count)
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

    class Item
    {
        //itemIDは使ってない。
        int ItemID { get; }
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
        public Item(string name)
        {
            this.Name = name;
            this.count = 1;
            this.Value = 10;
        }
        public Item(string name, int count)
        {
            this.Name = name;
            this.count = count;
            this.Value = 10;
        }
        public Item(string name, int count,int value)
        {
            this.Name = name;
            this.Count = count;
            this.Value = value;
        }

    }

    class Charactar
    {
        public string Name { get; }
        public Dictionary<string,Item> ItemList { get; }//アイテムリストは見るだけ。
        public int MaxHp { set; get; }
        public int Hp { set; get; }
        public int MaxMp { set; get; }
        public int Mp { set; get; }

        public int AttackPoint { set; get; }
        public int DeffencePoint { set; get; }

        //名前だけ必ず決めるようにする。
        public Charactar(string name)
        {
            this.Name = name;
            ItemList = new Dictionary<string,Item>();
            MaxHp = 10;
            Hp = 10;
            MaxMp = 5;
            Mp = 5;
            AttackPoint = 2;
            DeffencePoint = 1;
        }

        //アイテムを持たせたり減らしたりする。持ち物制限とかはここら辺でやる。(10種類以上は持たせないみたいな)
        public void AddItem(Item i)
        {
            if (ItemList.ContainsKey(i.Name))
            {
                //キャラのアイテムリストにアイテムがあれば個数分増やす
                ItemList[i.Name].Count += i.Count;
            }
            else
            {
                //キャラのアイテムリストにアイテムがなければ、アイテムを増やす。
                ItemList.Add(i.Name, i);
            }
        }
        public bool RemoveItem(Item i)
        {
            if (ItemList.ContainsKey(i.Name))
            {
                //キャラのアイテムリストにアイテムがあれば個数分減らす。
                //なお、countは0以下にならないので減らすだけなら問題ない。売るときは問題になる。
                ItemList[i.Name].Count -= i.Count;

                //個数が0なら消す
                CheckItemlist();

                return true;

            } else
            {
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
        public bool RemoveItem(Item i , int count)
        {
            i.Count = count;
            return RemoveItem(i);
          
        }

        //アイテムリストのアイテム個数0だった場合は削除する,もっといい書き方あるはず
        protected void CheckItemlist()
        {
            List<string> temps = new List<string>();

            foreach (Item items in ItemList.Values )
            {
                if (items.Count <= 0)
                {
                    //ItemList.Remove(name);
                    temps.Add(items.Name);
                }
            }

            foreach(string name in temps)
            {
                ItemList.Remove(name);
            }
        }

    }

}
