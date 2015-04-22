using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MojBroj
{
    public partial class MainForm : Form
    {
        private Regex numberListRegex;
        private Regex numberOnlyRegex;

        public MainForm()
        {
            numberListRegex = new Regex(@"^[0-9]+(,(\ )?[0-9]+)*?$");
            numberOnlyRegex = new Regex(@"^\d+$");

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            List<int> lista = new List<int>();

            string[] numbers = txtNumbers.Text.Split(',');

            foreach (string number in numbers)
            {
                lista.Add(Convert.ToInt32(number));
            }

            Evaluator ev = new Evaluator(lista);

            List<string> expressionList = ev.GetExpressions(Convert.ToInt32(txtTarget.Text));

            foreach (string expression in expressionList)
            {
                listBox1.Items.Add(expression);
            }
        }

        

        private void txtNumbers_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = numberListRegex.IsMatch(txtNumbers.Text);
        }

        private void txtTarget_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = numberOnlyRegex.IsMatch(txtTarget.Text);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(listBox1.SelectedItem.ToString());
            MessageBox.Show("Kopirano '" + listBox1.SelectedItem.ToString() + "' u Clipboard.");
        }
    }
}
