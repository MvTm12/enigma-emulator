using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class PlugBoard : Form
    {
        private char name;
        private string permutation;
        private string originPermutation;
        private Regex regex;

        /* --------------------------------------Design Form-------------------------------*/
        /*------For Form draggable pannel------*/
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        /*------end of draggable pannel inits------*/

        /*------Form Rounded Corners Inits------*/
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect,     // x-coordinate of upper-left corner
                int nTopRect,      // y-coordinate of upper-left corner
                int nRightRect,    // x-coordinate of lower-right corner
                int nBottomRect,   // y-coordinate of lower-right corner
                int nWidthEllipse, // height of ellipse
                int nHeightEllipse // width of ellipse
            );
        /*------End Form Rounded Corners Inits------*/
        /* ----------------------------------End Design Form-------------------------------*/

        /*PlugBoard Constructors*/
        public PlugBoard()
        {
            InitializeComponent();
            /* Form Rounded Corners Inits*/
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            /* End Form Rounded Corners Inits*/
            this.name = 'P';
            this.permutation = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.originPermutation = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.regex = new Regex(@"^[a-zA-Z ]*$");
            this.InputTextBox.CharacterCasing = CharacterCasing.Upper;
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.BackColor = this.InputTextBox.BackColor;
        }
        public PlugBoard(char _name, string _permutation)
        {
            InitializeComponent();
            this.name = _name;
            this.permutation = _permutation;
            this.originPermutation = _permutation;
            this.regex = new Regex(@"^[a-zA-Z ]*$");
            this.InputTextBox.CharacterCasing = CharacterCasing.Upper;
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.BackColor = this.InputTextBox.BackColor;
        }
        /*End of PlugBoard Constructors*/

        /*------Text boxes change functions------*/
        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            checkReg(InputTextBox,regex);
        }
        /*------End of Text boxes change functions------*/

        /*------Button click event functions------*/
        private void RunButton_Click(object sender, EventArgs e)
        {
            this.permutation = resetPermutation();
            permutationConfiguration();
            this.OutputTextBox.Text = this.permutation;
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        /*------End of Button click event functions------*/

        /*Utilities Functions*/
        private void checkReg(TextBox textBoxToCheck, Regex regex)
        {
            if (!regex.Match(textBoxToCheck.Text).Success && textBoxToCheck.Text != null)
            {
                MessageBox.Show("This textbox accepts only english alphabetical characters");
                textBoxToCheck.Text = textBoxToCheck.Text.Remove(textBoxToCheck.Text.Length - 1);
                textBoxToCheck.SelectionStart = textBoxToCheck.Text.Length;
            }
        }
        private string resetPermutation()
        {
            return this.originPermutation;
        }
        public string getPermutation()
        {
            return this.permutation;
        }
        private void replaceAlphaBetString(string alphaBetString, char firstChar, char secondChar)
        {
            char[] AlphaBetStringCharSet = alphaBetString.ToCharArray();

            AlphaBetStringCharSet[firstChar - 65] = secondChar;
            AlphaBetStringCharSet[secondChar - 65] = firstChar;

            this.permutation = new string(AlphaBetStringCharSet);
        }
        private void permutationConfiguration()
        {
            string _tempTextBoxText = this.InputTextBox.Text.Replace(" ", "");
            if( checkIfPairs(_tempTextBoxText) && 
                checkIfApearMoreThenOnce(_tempTextBoxText) && 
                checkTextLength(_tempTextBoxText,20) )
            {
                for (int i = 0; i < _tempTextBoxText.Length; i = i + 2)
                {
                    replaceAlphaBetString(this.permutation, _tempTextBoxText[i], _tempTextBoxText[i + 1]);
                }
            }
        }
        private bool checkIfPairs(string text)
        {
            if (text.Length % 2 != 0)
            {
                MessageBox.Show("The configuration must include pairs of chars!");
                return false;
            }
            return true;
        }
        private bool checkIfApearMoreThenOnce(string text)
        {
            if (text.Distinct().Count() != text.Count())
            {
                MessageBox.Show("There is a char that write twice!");
                return false;
            }
            return true;
        }
        private bool checkTextLength(string text, int textLength)
        {
            if (text.Length > textLength)
            {
                MessageBox.Show("The configuration must have less than 10 pairs!");
                return false;
            }
            return true;
        }
        /*End of Utilities Functions*/

        /* Form Panel dragable functions*/
        private void PlagBourdPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        /* End of Form Panel dragable functions*/

    }
}
