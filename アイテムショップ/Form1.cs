﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace アイテムショップ
{
    public partial class Form1 : Form
    {
        Nampospace.GameMaster gm;

        public Form1()
        {
            InitializeComponent();
            gm = new Nampospace.GameMaster();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gm.OnLoad(comboBox1);
            comboBox1.SelectedIndex = gm.SelectedChara;

            //同じ４行が別の場所に後の２箇所書かれていて、修正とか不便
            gm.CharaShowItem(textBox1, gm.itemshop);
            gm.ShowItem(textBox2, gm.itemshop);
            textBox3.Text = "持ち金：" + gm.itemshop.Gold.ToString() + "gold";
            textBox4.Text = "持ち金：" + gm.PartyGold.ToString() + "gold";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gm.SelectChara(comboBox1.SelectedIndex);

            gm.CharaShowItem(textBox1,gm.itemshop);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //このままだと薬草しか売り買いできない。選択できるようにしたい
            gm.itemshop.BuyFromChara(new Nampospace.Item("薬草"),gm.MainParty[gm.SelectedChara],ref gm.PartyGold);

            //同じ箇所
            gm.CharaShowItem(textBox1, gm.itemshop);
            gm.ShowItem(textBox2, gm.itemshop);
            textBox3.Text = "持ち金：" + gm.itemshop.Gold.ToString() + "gold";
            textBox4.Text = "持ち金：" + gm.PartyGold.ToString() + "gold";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //このままだと薬草しか売り買いできない。選択できるようにしたい
            gm.itemshop.SellToChara(new Nampospace.Item("薬草"), gm.MainParty[gm.SelectedChara],ref gm.PartyGold);

            //同じ箇所
            gm.CharaShowItem(textBox1, gm.itemshop);
            gm.ShowItem(textBox2, gm.itemshop);
            textBox3.Text = "持ち金：" + gm.itemshop.Gold.ToString() + "gold";
            textBox4.Text = "持ち金：" + gm.PartyGold.ToString() + "gold";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
